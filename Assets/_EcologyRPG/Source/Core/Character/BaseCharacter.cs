using EcologyRPG.AbilityScripting;
using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Events;
using EcologyRPG.Core.Scripting;
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
        public virtual Vector3 Velocity { get { return Rigidbody.velocity; }  set { Rigidbody.velocity = value; } }
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
        public bool Invunerable { 
            get 
            { 
                return invunerable; 
            }
            set
            {
                invunerable = value;
            }
        }

        protected bool IsPaused { get { return CharacterBinding == null || state == CharacterStates.dead; } }

        public UnityEvent<BaseCharacter> OnCharacterCollision = new();
        readonly List<ConditionReference> effects = new();
        readonly List<ConditionReference> fixedUpdateEffects = new();

        protected int level;
        protected AttributeModification[] levelMods;
        protected bool canMove = true;
        protected bool canRotate = true;
        protected bool invunerable = false;
        protected CharacterTransform transform;
        protected Resource Health;
        protected CharacterBinding CharacterBinding { get; private set; }
        StatModification CastingSlow;

        protected List<StatModification> statMods = new List<StatModification>();


        public BaseCharacter()
        {
            GUID = Guid.NewGuid().ToString();
            Characters.Instance.AddCharacter(this);
            CastingSlow = new StatModification("movementSpeed", -0.75f, StatModType.PercentMult, this);
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
            state = CharacterStates.active;
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
            if(invunerable || state == CharacterStates.dead || state == CharacterStates.dodging)
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

        //TODO: Add the old uniquestatmodifierhandler to this
        public void AddUniqueStatModifier(StatModification mod, bool minVal)
        {
            var existingMod = statMods.Find(x => x.StatName == mod.StatName);
            if(existingMod != null)
            {
                if (minVal && mod.Value > existingMod.Value) return;
                else if (!minVal && mod.Value < existingMod.Value) return;

                Stats.RemoveStatModifier(existingMod);
                Stats.AddStatModifier(mod);
                statMods.Remove(existingMod);
                statMods.Add(mod);
            }
            else
            {
                Stats.AddStatModifier(mod);
                statMods.Add(mod);
            }

        }

        public void RemoveUniqueStatModifier(StatModification mod)
        {
            var existingMod = statMods.Find(x => x.StatName == mod.StatName);
            if(existingMod != null)
            {
                Stats.RemoveStatModifier(existingMod);
                statMods.Remove(existingMod);
            }
        }

        public void AddStatModifier(StatModification mod)
        {
            Stats.AddStatModifier(mod);
        }

        public void RemoveStatModifier(StatModification mod)
        {
            Stats.RemoveStatModifier(mod);
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

        public ConditionReference GetConditionReference(int ID)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].ID == ID)
                {
                    return effects[i];
                }
            }
            return null;
        }

        public virtual void ApplyCondition(CastContext caster, ConditionReference effect)
        {
            if(state == CharacterStates.dead)
            {
                return;
            }
            effect.CastContext = caster;

            if (effect.useFixedUpdate)
            {
                for (int i = 0; i < fixedUpdateEffects.Count; i++)
                {
                    if (fixedUpdateEffects[i].CastContext.GetOwner() == caster.GetOwner() && fixedUpdateEffects[i].ID == effect.ID)
                    {
                        fixedUpdateEffects[i].OnReapply();
                        return;
                    }
                }
                fixedUpdateEffects.Add(effect);
            }
            else
            {
                for (int i = 0; i < effects.Count; i++)
                {
                    if (effects[i].CastContext.GetOwner() == caster.GetOwner() && effects[i].ID == effect.ID)
                    {
                        effects[i].OnReapply();
                        return;
                    }
                }
                effects.Add(effect);
            }
            effect.remainingDuration = effect.duration;
            effect.OnApply(caster, this);
        }

        public virtual void RemoveCondition(ConditionReference effect)
        {
            if(!effect.useFixedUpdate)
            {
                for (int i = 0; i < effects.Count; i++)
                {
                    if (effects[i].ID.Equals(effect.ID) && effects[i].CastContext.GetOwner() == effect.CastContext.GetOwner())
                    {
                        effects.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                for(int i = 0; i < fixedUpdateEffects.Count; i++)
                {
                    if (fixedUpdateEffects[i].ID.Equals(effect.ID) && fixedUpdateEffects[i].CastContext.GetOwner() == effect.CastContext.GetOwner())
                    {
                        fixedUpdateEffects.RemoveAt(i);
                        break;
                    }
                }
            }
            effect.OnRemoved();
        }

        public virtual ConditionReference[] GetCondition()
        {
            return effects.ToArray();
        }

        public virtual ConditionReference GetCondition(BaseCharacter owner, string ID)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].ID.Equals(ID) && effects[i].CastContext.GetOwner() == owner)
                {
                    return effects[i];
                }
            }
            return null;
        }

        public virtual ConditionReference GetCondition(BaseCharacter owner, Type type)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].GetType() == type && effects[i].CastContext.GetOwner() == owner)
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

        public void SlowMovementSpeed()
        {
            Stats.AddStatModifier(CastingSlow);
        }

        public void RestoreMovementSpeed()
        {
            Stats.RemoveStatModifier(CastingSlow);
        }

        public abstract void Move(Vector3 direction, float speed);
        public abstract void RotateTowards(Vector3 direction);

        public virtual void Update()
        {
            if (IsPaused || effects.Count == 0) return;
            for (int i = effects.Count -1 ; i >= 0; i--)
            {
                ConditionReference effect = effects[i];
                effect.OnUpdate(Time.deltaTime);
                effect.remainingDuration -= Time.deltaTime;
                if (effect.remainingDuration <= 0)
                {
                    RemoveCondition(effect);
                }
            }
        }

        public virtual void FixedUpdate()
        {
            if (IsPaused || fixedUpdateEffects.Count == 0) return;
            for (int i = fixedUpdateEffects.Count - 1; i >= 0; i--)
            {
                ConditionReference effect = fixedUpdateEffects[i];
                effect.OnUpdate(Time.deltaTime);
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
            if (CharacterBinding == null) return null;
            return CharacterBinding.StartCoroutine(methodName);
        }

        public Coroutine StartCoroutine(string methodName, object value)
        {
            if (CharacterBinding == null) return null;
            return CharacterBinding.StartCoroutine(methodName, value);
        }

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            if(CharacterBinding == null) return null;
            return CharacterBinding.StartCoroutine(routine);
        }

        public void StopCoroutine(string methodName)
        {
            if (CharacterBinding != null)
                CharacterBinding.StopCoroutine(methodName);
        }

        public void StopCoroutine(IEnumerator routine)
        {
            if (CharacterBinding != null)
                CharacterBinding.StopCoroutine(routine);
        }

        public void StopAllCoroutines()
        {
            if (CharacterBinding != null)
                CharacterBinding.StopAllCoroutines();
        }

        public bool TryGetComponent<T>(out T component) where T : Component
        {
            if (CharacterBinding == null)
            {
                component = default;
                return false;
            }

            return CharacterBinding.TryGetComponent(out component);
        }

        public virtual void IgnoreCollision()
        {
            if(CharacterBinding == null) return;
            Rigidbody.excludeLayers = AbilityManager.TargetMask;
        }

        public virtual void RestoreCollision()
        {
            if(CharacterBinding == null) return;
            Rigidbody.excludeLayers = 0;
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
