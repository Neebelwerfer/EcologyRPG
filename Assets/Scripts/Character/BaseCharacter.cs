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
        public Stats stats;
        public Rigidbody Rigidbody { get { return rb; } }
        public CharacterStates state = CharacterStates.active;

        List<Debuff> debuffs = new List<Debuff>();
        List<Buff> buffs = new List<Buff>();

        Resource Health;

        Rigidbody rb;


        public virtual void Start()
        {
            stats = new Stats();
            Health = stats.GetResource("health");
            rb = GetComponent<Rigidbody>();
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

        public virtual void ApplyBuff(Buff buff)
        {
            Debug.Log("Applying Buff");
            buffs.Add(buff);
            buff.OnApply(this);
        }

        public virtual void RemoveBuff(Buff buff)
        {
            Debug.Log("Removing Buff");
            buffs.Remove(buff);
            buff.OnRemoved(this);
        }

        public virtual void Update()
        {
            List<Debuff> debuffsToRemove = new List<Debuff>();
            List<Buff> buffsToRemove = new List<Buff>();

            foreach (Debuff debuff in debuffs)
            {
                debuff.OnUpdate(this, Time.deltaTime);
                debuff.remainingDuration -= Time.deltaTime;
                if (debuff.remainingDuration <= 0)
                {
                    debuffsToRemove.Add(debuff);
                }
            }

            foreach (Buff buff in buffs)
            {
                buff.OnUpdate(this, Time.deltaTime);
                buff.remainingDuration -= Time.deltaTime;
                if (buff.remainingDuration <= 0)
                {
                    buffsToRemove.Add(buff);
                }
            }

            foreach (Debuff debuff in debuffsToRemove)
            {
                RemoveDebuff(debuff);
            }

            foreach (Buff buff in buffsToRemove)
            {
                RemoveBuff(buff);
            }
        }
    }
}
