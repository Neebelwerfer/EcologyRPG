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
        public bool AllowHolding;

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
            caster.owner.StartCoroutine(HandleCast(caster));
        }   

        public virtual bool CanActivate(CasterInfo caster)
        {
            if (state != AbilityStates.ready)
            {
                Debug.Log("Ability not ready");
                return false;
            }

            if(caster.owner.state != CharacterStates.active)
            {
                Debug.Log("Character not active");
                return false;
            }

            if (caster.owner.stats.GetResource(ResourceName) < ResourceCost)
            {
                Debug.Log("Not enough resource");
                return false;
            }

            if (remainingCooldown > 0)
            {
                Debug.Log("On cooldown");
                return false;
            }

            return true;
        }

        public virtual IEnumerator HandleCast(CasterInfo caster)
        {
            caster.owner.state = CharacterStates.casting;
            state = AbilityStates.casting;

            var resource = caster.owner.stats.GetResource(ResourceName);
            resource -= ResourceCost;

            CastStarted(caster);

            yield return null;

            if(AllowHolding)
            {
                while(caster.activationInput.action.IsPressed())
                {
                    Debug.Log("Holding");
                    OnHold(caster);
                    yield return null;
                }
            }
            CastEnded(caster);

            caster.owner.state = CharacterStates.active;
            
            if(Cooldown > 0)
            {
                state = AbilityStates.cooldown;
                remainingCooldown = Cooldown;
                caster.owner.StartCoroutine(CooldownTimer());
            } else
            {
                state = AbilityStates.ready;
            }
        }

        public abstract void CastStarted(CasterInfo caster);

        public abstract void OnHold(CasterInfo caster);

        public abstract void CastEnded(CasterInfo caster);

        IEnumerator CooldownTimer()
        {
            while (remainingCooldown > 0)
            {
                remainingCooldown -= Time.deltaTime;
                yield return null;
            }
            remainingCooldown = 0;
            state = AbilityStates.ready;
        }
    }
}