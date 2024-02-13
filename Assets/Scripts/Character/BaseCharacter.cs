using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public enum CharacterStates
    {
        active,
        casting,
        disabled,
        dead
    }

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class BaseCharacter : MonoBehaviour
    {
        public Stats stats;
        public CharacterStates state = CharacterStates.active;

        List<Debuff> debuffs = new List<Debuff>();

        Resource Health;

        public virtual void Start()
        {
            stats = new Stats();
            Health = stats.GetResource("health");
        }

        public virtual void TakeDamage(float damage)
        {
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

        public virtual void ApplyDebuff(Debuff debuff)
        {
            Debug.Log("Applying Debuff");
            debuffs.Add(debuff);
            debuff.OnApply(this);
        }

        public virtual void RemoveDebuff(Debuff debuff)
        {
            Debug.Log("Removing Debuff");
            debuffs.Remove(debuff);
            debuff.OnRemoved(this);
        }

        public virtual void Update()
        {
            List<Debuff> debuffsToRemove = new List<Debuff>();

            foreach (Debuff debuff in debuffs)
            {
                debuff.OnUpdate(this, Time.deltaTime);
                debuff.remainingDuration -= Time.deltaTime;
                if (debuff.remainingDuration <= 0)
                {
                    debuffsToRemove.Add(debuff);
                }
            }

            foreach (Debuff debuff in debuffsToRemove)
            {
                RemoveDebuff(debuff);
            }
        }
    }
}
