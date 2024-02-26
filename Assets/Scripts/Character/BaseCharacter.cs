using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class BaseCharacter : MonoBehaviour
    {
        public GameObject AbilityPoint;

        public Stats stats;
        public Rigidbody Rigidbody { get { return rb; } }
        public CharacterStates state = CharacterStates.active;

        List<CharacterEffect> debuffs = new List<CharacterEffect>();

        Resource Health;

        Rigidbody rb;


        public virtual void Start()
        {
            stats = new Stats();
            Health = stats.GetResource("health");
            rb = GetComponent<Rigidbody>();
            if(AbilityPoint == null) AbilityPoint = gameObject;
        }

        public virtual void ApplyDamage(BaseCharacter damageDealer, float damage)
        {
            Debug.Log("Applying " + damage + " damage to " + gameObject.name);
            Health -= damage;
            if (Health.CurrentValue <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            state = CharacterStates.dead;
        }

        public virtual void ApplyCharacterModification(CharacterEffect mod)
        {
            Debug.Log("Applying CharacterModification " + mod.displayName);
            debuffs.Add(mod);
            mod.OnApply(this);
        }

        public virtual void RemoveCharacterModification(CharacterEffect mod)
        {
            Debug.Log("Removing CharacterModification " + mod.displayName);
            debuffs.Remove(mod);
            mod.OnRemoved(this);
        }

        public virtual void Update()
        {
            for (int i = debuffs.Count -1 ; i >= 0; i--)
            {
                CharacterEffect debuff = debuffs[i];
                debuff.OnUpdate(this, Time.deltaTime);
                debuff.remainingDuration -= Time.deltaTime;
                if (debuff.remainingDuration <= 0)
                {
                    RemoveCharacterModification(debuff);
                }
            }
        }
    }
}
