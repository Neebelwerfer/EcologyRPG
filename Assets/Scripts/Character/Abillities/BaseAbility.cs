using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Character.Abilities
{
    public enum AbilityStates
    {
        ready,
        casting,
        cooldown
    }

    public class CasterInfo
    {
        public BaseCharacter owner;
        public Vector3 castPos;
        public InputActionReference activationInput;
    }

    public abstract class BaseAbility : ScriptableObject
    {
        public string DisplayName;
        public float ResourceCost;
        public string ResourceName;
        public float Cooldown;
        public float remainingCooldown = 0;
        public AbilityStates state = AbilityStates.ready;

        public virtual void UpdateCooldown(float deltaTime)
        {
            if (state == AbilityStates.ready) return;

            if (remainingCooldown > 0)
            {
                remainingCooldown -= deltaTime;
                if (remainingCooldown <= 0)
                {
                    remainingCooldown = 0;
                    state = AbilityStates.ready;
                }
            }
        }

        public virtual void Activate(CasterInfo caster)
        {
            if (!CanActivate(caster)) return;
            Debug.Log("CASTING " + DisplayName);   
            caster.owner.StartCoroutine(Cast(caster));
        }   

        public virtual bool CanActivate(CasterInfo caster)
        {
            return state == AbilityStates.ready && caster.owner.state == CharacterStates.active && remainingCooldown == 0 && caster.owner.stats.GetResource(ResourceName) > ResourceCost;
        }

        public abstract IEnumerator Cast(CasterInfo caster);
    }
}