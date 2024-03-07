using Character.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.SearchService;
using UnityEngine;
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

        public int Level { get { return level; } }
        public Rigidbody Rigidbody { get { return rb; } }

        public Stats Stats { get; private set; }

        public Random Random { get 
            {
                return new Random((uint)UnityEngine.Random.Range(uint.MinValue, uint.MaxValue));
            }
        }

        public CharacterStates state = CharacterStates.active;

        readonly List<CharacterEffect> effects = new List<CharacterEffect>();

        protected int level;
        protected AttributeModification[] levelMods;
        Resource Health;
        Rigidbody rb;


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
            Debug.Log("Applying " + damageInfo.damage + " damage to " + gameObject.name);
            Health -= damageInfo.damage;
            var damageEvent = new DamageEvent { damageTaken = damageInfo.damage, source = damageInfo.source, target = this, damageType = damageInfo.type, premitigationDamage = damageInfo.damage };
            EventManager.Defer("DamageEvent", damageEvent, DeferredEventType.Update);

            if (Health.CurrentValue <= 0)
            {
                Die();
            }
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

        public virtual void ApplyEffect(CasterInfo caster, CharacterEffect mod)
        {
            Debug.Log("Applying CharacterModification " + mod.displayName + " with duration " + mod.duration);
            effects.Add(mod);
            mod.OnApply(caster, this);
        }

        public virtual void RemoveCharacterModification(CharacterEffect mod)
        {
            Debug.Log("Removing CharacterModification " + mod.displayName);
            effects.Remove(mod);
            mod.OnRemoved(this);
        }

        public virtual void Update()
        {
            for (int i = effects.Count -1 ; i >= 0; i--)
            {
                CharacterEffect effect = effects[i];
                effect.OnUpdate(this, Time.deltaTime);
                effect.remainingDuration -= Time.deltaTime;
                if (effect.remainingDuration <= 0)
                {
                    RemoveCharacterModification(effect);
                }
            }
        }
    }
}
