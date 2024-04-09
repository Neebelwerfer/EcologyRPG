using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Events;
using EcologyRPG.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = Unity.Mathematics.Random;

namespace EcologyRPG.Core.Character
{
    public enum CharacterStates
    {
        active,
        casting,
        dodging,
        disabled,
        dead
    }

    public enum Faction
    {
        player,
        enemy,
        neutral
    }

    public abstract class BaseCharacter
    {
        [CharacterTag]
        public List<string> Tags = new List<string>();

        protected Faction faction = Faction.neutral;

        public string GUID { get; private set; }
        public Faction Faction { get { return faction; } }
        public virtual GameObject GameObject { get 
            { 
                if(CharacterBinding != null) return CharacterBinding.gameObject;
                else return null;
            } 
        }
        public virtual Transform CastTransform { get { return CharacterBinding.CastingPoint; } }
        public virtual Vector3 CastPos { get { return CharacterBinding.CastingPoint.position; } }
        public virtual CharacterTransform Transform { get { return transform; } }
        public int Level { get { return level; } }
        public Rigidbody Rigidbody { get { return CharacterBinding.Rigidbody; } }
        public CapsuleCollider Collider { get { return CharacterBinding.Collider; } }
        public Animator Animator { get { return CharacterBinding.Animator; } }
        public Stats Stats { get; private set; }

        public Random Random { get 
            {
                return new Random((uint)UnityEngine.Random.Range(uint.MinValue, uint.MaxValue));
            }
        }

        public CharacterStates state = CharacterStates.active;
        public bool CanMove { get { return canMove; } }
        public bool CanRotate { get { return canRotate; } }

        protected bool IsPaused { get { return CharacterBinding == null; } }

        public UnityEvent<BaseCharacter> OnCharacterCollision = new();
        readonly List<Condition> effects = new();
        readonly List<Condition> fixedUpdateEffects = new();

        protected int level;
        protected AttributeModification[] levelMods;
        protected bool canMove = true;
        protected bool canRotate = true;
        protected CharacterTransform transform;
        protected Resource Health;
        protected CharacterBinding CharacterBinding { get; private set; }


        public BaseCharacter()
        {
            GUID = Guid.NewGuid().ToString();
            Characters.Instance.AddCharacter(this);
            transform = new CharacterTransform();
            Stats = new Stats();
            Health = Stats.GetResource("health");
            level = 1;
            InitLevel();
        }

        ~BaseCharacter()
        {
            Characters.Instance.RemoveCharacter(this);
        }

        public virtual void SetBinding(CharacterBinding binding)
        {
            CharacterBinding = binding;
            binding.OnCollisionEnterEvent.AddListener(OnCollisionEnter);
            CharacterBinding.SetCharacter(this);
            transform.SetBinding(binding);
        }

        public virtual void RemoveBinding()
        {
            CharacterBinding.SetCharacter(null);
            CharacterBinding = null;
            transform.RemoveBinding();
        }

        public virtual void ApplyDamage(DamageInfo damageInfo)
        {
            if(state == CharacterStates.dead || state == CharacterStates.dodging)
            {
                return;
            }

            damageInfo.damage = CalculateDamage(damageInfo);
            Health -= damageInfo.damage;
            var damageEvent = new DamageEvent { damageTaken = damageInfo.damage, source = damageInfo.source, target = this, damageType = damageInfo.type, premitigationDamage = damageInfo.damage, Point = Transform.Position };
            EventManager.Defer("DamageEvent", damageEvent);

            if (Health.CurrentValue <= 0)
            {
                Die();
            }
        }

        protected virtual float CalculateDamage(DamageInfo damageInfo)
        {
            if (damageInfo.type == DamageType.Physical)
            {
                var armor = Stats.GetStat("armor").Value;
                var resistance = Stats.GetAttribute("resistance").Value;
                float damage = damageInfo.damage - (armor + (resistance / (3 / 2 * armor)));
                if (damage < damageInfo.damage * 0.1)
                {
                    return damageInfo.damage * 0.1f;
                }
                return damage;
            }
            else if (damageInfo.type == DamageType.Water)
            {
                var waterArmor = Stats.GetStat("waterArmor").Value;
                var insolation = Stats.GetAttribute("insolation").Value;
                var damage = damageInfo.damage - (waterArmor + (insolation / (3 / 2 * waterArmor)));
                if (damage < damageInfo.damage * 0.1)
                {
                    return damageInfo.damage * 0.1f;
                }
                return damage;
            }
            else if (damageInfo.type == DamageType.Toxic)
            {
                return damageInfo.damage;
            }
            return damageInfo.damage;
        }

        public virtual void Die()
        {
            effects.Clear();
            fixedUpdateEffects.Clear();
            state = CharacterStates.dead;
        }

        protected virtual void InitLevel()
        {
            levelMods = new AttributeModification[Stats._attributes.Count];
            for (int i = 0; i < Stats._attributes.Count; i++)
            {
                AttributeModification attMod = new AttributeModification(Stats._attributes[i].Data.name, Level, this);
                Stats._attributes[i].AddModifier(attMod);
                levelMods[i] = attMod;
            }
        }

        public virtual void ApplyCondition(CastInfo caster, Condition effect)
        {
            if(state == CharacterStates.dead)
            {
                return;
            }
            effect.Owner = caster.owner;

            if(effect is IUpdateCondition)
            {
                for (int i = 0; i < effects.Count; i++)
                {
                    if (effects[i].Owner == caster.owner && effects[i].ID.Equals(effect.ID))
                    {
                        effects[i].OnReapply(this);
                        return;
                    }
                }
                effects.Add(effect);
            }
            else if(effect is IFixedUpdateCondition)
            {
                for (int i = 0; i < fixedUpdateEffects.Count; i++)
                {
                    if (fixedUpdateEffects[i].Owner == caster.owner && fixedUpdateEffects[i].ID.Equals(effect.ID))
                    {
                        fixedUpdateEffects[i].OnReapply(this);
                        return;
                    }
                }
                fixedUpdateEffects.Add(effect);
            }
            effect.remainingDuration = effect.duration;
            effect.OnApply(caster, this);
        }

        public virtual void RemoveCondition(Condition effect)
        {
            if(effect is IUpdateCondition)
            {
                effects.Remove(effect);
            }
            else if (effect is IFixedUpdateCondition)
            {
                fixedUpdateEffects.Remove(effect);
            }
            effect.OnRemoved(this);
        }

        public virtual Condition[] GetCondition()
        {
            return effects.ToArray();
        }

        public virtual Condition GetCondition(BaseCharacter owner, string ID)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].ID.Equals(ID) && effects[i].Owner == owner)
                {
                    return effects[i];
                }
            }
            return null;
        }

        public virtual Condition GetCondition(BaseCharacter owner, Type type)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].GetType() == type && effects[i].Owner == owner)
                {
                    return effects[i];
                }
            }
            return null;
        }

        public virtual void StopRotation()
        {
            canRotate = false;
        }

        public virtual void StartRotation()
        {
            canRotate = true;
        }

        public virtual void StopMovement()
        {
            canMove = false;
        }

        public virtual void StartMovement()
        {
            canMove = true;
        }

        public abstract void Move(Vector3 direction, float speed);

        public virtual void Update()
        {
            if (IsPaused) return;
            for (int i = effects.Count -1 ; i >= 0; i--)
            {
                Condition effect = effects[i];
                effect.OnUpdate(this, Time.deltaTime);
                effect.remainingDuration -= Time.deltaTime;
                if (effect.remainingDuration <= 0)
                {
                    RemoveCondition(effect);
                }
            }
        }

        public virtual void FixedUpdate()
        {
            if (IsPaused) return;
            for (int i = fixedUpdateEffects.Count - 1; i >= 0; i--)
            {
                Condition effect = fixedUpdateEffects[i];
                effect.OnUpdate(this, Time.deltaTime);
                effect.remainingDuration -= Time.deltaTime;
                if (effect.remainingDuration <= 0)
                {
                    RemoveCondition(effect);
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == GameObject.layer)
            {
                if (collision.gameObject.TryGetComponent(out CharacterBinding characterBinding))
                {
                    OnCharacterCollision?.Invoke(characterBinding.Character);
                }
            }
        }

        public Coroutine StartCoroutine(string methodName)
        {
            return CharacterBinding.StartCoroutine(methodName);
        }

        public Coroutine StartCoroutine(string methodName, object value)
        {
            return CharacterBinding.StartCoroutine(methodName, value);
        }

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return CharacterBinding.StartCoroutine(routine);
        }

        public void StopCoroutine(string methodName)
        {
            CharacterBinding.StopCoroutine(methodName);
        }

        public void StopCoroutine(IEnumerator routine)
        {
            CharacterBinding.StopCoroutine(routine);
        }

        public void StopAllCoroutines()
        {
            CharacterBinding.StopAllCoroutines();
        }

        public bool TryGetComponent<T>(out T component)
        {
            return CharacterBinding.TryGetComponent(out component);
        }

        public static bool IsLegalMove(BaseCharacter character, Vector3 dir, float speed)
        {
            var origin = character.Transform.Position;
            var checkPos = origin + dir * (speed * 2);
            checkPos.y += 1f;
            Debug.DrawRay(checkPos, Vector3.down * 1, Color.red);
            if (Physics.Raycast(checkPos, Vector3.down, out var hit, 30, AbilityManager.GroundMask))
            {
                if (hit.distance < 2f)
                    return true;
            }
            return false;
        }
    }
}
