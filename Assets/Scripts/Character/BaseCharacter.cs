using Character.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Utility;
using Random = Unity.Mathematics.Random;

namespace Character
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
    public class DamageEvent : EventData
    {
        public BaseCharacter target;
        public new BaseCharacter source;
        public Vector3 Point;
        public DamageType damageType;
        public float premitigationDamage;
        public float damageTaken;
    }

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class BaseCharacter : MonoBehaviour
    {
        public GameObject AbilityPoint;
        [CharacterTag]
        public List<string> Tags = new List<string>();

        [SerializeField] Faction faction = Faction.neutral;

        public Faction Faction { get { return faction; } }
        public virtual Vector3 Forward { get { return transform.forward; } }
        public virtual Vector3 Position { get { return transform.position; } }
        public virtual Vector3 CastPos { get { return AbilityPoint.transform.position; } }

        public virtual Transform Transform { get { return transform; } }

        public int Level { get { return level; } }
        public Rigidbody Rigidbody { get { return rb; } }

        public Stats Stats { get; private set; }

        public Random Random { get 
            {
                return new Random((uint)UnityEngine.Random.Range(uint.MinValue, uint.MaxValue));
            }
        }

        public CharacterStates state = CharacterStates.active;
        public bool CanMove { get { return canMove; } }

        public UnityEvent<BaseCharacter> OnCharacterCollision = new();
        readonly List<Condition> effects = new();

        protected int level;
        protected AttributeModification[] levelMods;
        protected bool canMove = true;
        protected Resource Health;
        protected Rigidbody rb;


        public virtual void Start()
        {
            
            CharacterManager.Instance.AddCharacter(this);
            Stats = new Stats();
            Health = Stats.GetResource("health");
            rb = GetComponent<Rigidbody>();
            if(AbilityPoint == null) AbilityPoint = gameObject;

            level = 1;
            InitLevel();
        }

        public virtual void ApplyDamage(DamageInfo damageInfo)
        {
            damageInfo.damage = CalculateDamage(damageInfo);
            Health -= damageInfo.damage;
            var damageEvent = new DamageEvent { damageTaken = damageInfo.damage, source = damageInfo.source, target = this, damageType = damageInfo.type, premitigationDamage = damageInfo.damage, Point = Position };
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
            state = CharacterStates.dead;
        }

        protected virtual void InitLevel()
        {
            levelMods = new AttributeModification[Stats._attributes.Count];
            for (int i = 0; i < Stats._attributes.Count; i++)
            {
                AttributeModification attMod = new AttributeModification(Stats._attributes[i].data.name, Level, gameObject);
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

            for(int i = 0; i < effects.Count; i++)
            {
                if (effects[i].Owner == caster.owner && effects[i].ID.Equals(effect.ID))
                {
                    //Debug.Log("Reapplying CharacterModification " + effect.displayName);
                    effects[i].OnReapply(this);
                    return;
                }
            }
            //Debug.Log("Applying CharacterModification " + effect.displayName + " with duration " + effect.duration);
            effect.remainingDuration = effect.duration;
            effects.Add(effect);
            effect.OnApply(caster, this);
        }

        public virtual void RemoveCondition(Condition effect)
        {
            //Debug.Log("Removing CharacterModification " + effect.displayName);
            effects.Remove(effect);
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

        public virtual void StopMovement()
        {
            canMove = false;
        }

        public virtual void StartMovement()
        {
            canMove = true;
        }

        public virtual void Update()
        {
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

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Entity"))
            {
                OnCharacterCollision?.Invoke(collision.gameObject.GetComponent<BaseCharacter>());
            }
        }
    }

}
