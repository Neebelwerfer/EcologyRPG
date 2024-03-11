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

    public enum DamageType
    {
        Physical,
        Water,
        Toxic
    }

    public struct CasterInfo
    {
        public BaseCharacter owner;
        public Vector3 castPos;
        public InputAction activationInput;
    }

    public struct DamageInfo
    {
        public DamageType type;
        public float damage;
        public BaseCharacter source;
    }

    public abstract class BaseAbility : ScriptableObject
    {
        public string DisplayName;
        public float ResourceCost = 0;
        public string ResourceName;
        public float Cooldown = 0;
        public float CastTime = 0;
        public bool AllowHolding;
        public Sprite Icon;

        [HideInInspector] public float remainingCooldown = 0;
        [HideInInspector] public AbilityStates state = AbilityStates.ready;

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

        public virtual bool Activate(CasterInfo caster)
        {
            if (!CanActivate(caster)) return false;
            Debug.Log("CASTING " + DisplayName);
            caster.owner.StartCoroutine(HandleCast(caster));
            return true;
        }   

        public virtual bool CanActivate(CasterInfo caster)
        {
            if (state == AbilityStates.cooldown)
            {
                Debug.Log("On cooldown");
                return false;
            }

            if (state == AbilityStates.casting)
            {
                Debug.Log("Ability already casting");
                return false;
            }

            if (ResourceName != "" && caster.owner.Stats.GetResource(ResourceName) < ResourceCost)
            {
                Debug.Log("Not enough resource");
                return false;
            }
            return true;
        }

        public virtual IEnumerator HandleAsSecondaryCast(CasterInfo caster)
        {
            CastStarted(caster);

            yield return new WaitForSeconds(CastTime);

            CastEnded(caster);
        }

        public virtual IEnumerator HandleCast(CasterInfo caster)
        {
            caster.owner.state = CharacterStates.casting;
            state = AbilityStates.casting;

            if(ResourceCost > 0)
            {
                InitialCastCost(caster);
            }

            CastStarted(caster);

            Debug.Log("Cast time: " + CastTime);
            yield return new WaitForSeconds(CastTime);

            if(AllowHolding && CastTime == 0)
            {
                while(caster.activationInput.IsPressed())
                {
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
                AbilityManager.instance.RegisterAbilityOnCooldown(this);
            } else
            {
                state = AbilityStates.ready;
            }
        }

        protected static void Cast(CasterInfo caster, BaseAbility ability)
        {
            caster.owner.StartCoroutine(ability.HandleAsSecondaryCast(caster));
        }

        public static void ApplyEffect(CasterInfo caster, BaseCharacter target, CharacterEffect effect)
        {
            var instancedEffect = Instantiate(effect);
            instancedEffect.Owner = caster.owner;
            target.ApplyEffect(caster, instancedEffect);
        }


        /// <summary>
        /// Called when the cast is started to deduct the resource cost
        /// </summary>
        /// <param name="caster"></param>
        public virtual void InitialCastCost(CasterInfo caster)
        {
            var resource = caster.owner.Stats.GetResource(ResourceName);
            resource -= ResourceCost;
        }

        /// <summary>
        /// Called when the cast is started
        /// </summary>
        /// <param name="caster"></param>
        public abstract void CastStarted(CasterInfo caster);

        /// <summary>
        /// Called when the cast is held
        /// </summary>
        /// <param name="caster"></param>
        public abstract void OnHold(CasterInfo caster);

        /// <summary>
        /// Called when the cast has ended
        /// </summary>
        /// <param name="caster"></param>
        public abstract void CastEnded(CasterInfo caster);
    }
}