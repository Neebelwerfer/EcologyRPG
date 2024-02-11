using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    }
}
