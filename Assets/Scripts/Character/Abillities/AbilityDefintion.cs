using Character.Abilities.AbilityEffects;
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

    public struct CastInfo
    {
        public BaseCharacter owner;
        public Vector3 castPos;
        public Vector3 dir;
        public Vector3 mousePoint;
        public InputAction activationInput;
    }

    public struct DamageInfo
    {
        public DamageType type;
        public float damage;
        public BaseCharacter source;
    }

    public class AbilityCastEvent : EventData
    {
        public CastInfo Caster;
        public AbilityDefintion Ability;

        public AbilityCastEvent(CastInfo caster, AbilityDefintion ability)
        {
            Caster = caster;
            Ability = ability;
        }
    }

    public abstract class AbilityDefintion : ScriptableObject
    {
        public string DisplayName;
        [Tooltip("The cooldown of this ability")]
        public float Cooldown = 0.5f;
        [Tooltip("The cast time of this ability. Cast Started will always be called first, and then after the Cast Time has been waited Cast ended will be called")]
        public float CastTime = 0;
        public Sprite Icon;

        public List<BuffCondition> BuffsOnCast;

        [HideInInspector] public float remainingCooldown = 0;
        [HideInInspector] public AbilityStates state = AbilityStates.ready;
        BaseCharacter owner;

        public List<AbilityEffect> CastWindUp = new List<AbilityEffect>();

        public virtual void Initialize(BaseCharacter owner)
        {
            this.owner = owner;
        }

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

        public virtual bool Activate(CastInfo caster)
        {
            if (!CanActivate(caster)) return false;
            //Debug.Log("CASTING " + DisplayName);
            EventManager.Defer("OnAbilityCast", new AbilityCastEvent(caster, this), DeferredEventType.Update);
            caster.owner.StartCoroutine(HandleCast(caster));
            return true;
        }   

        public virtual bool CanActivate(CastInfo caster)
        {
            if (state == AbilityStates.cooldown)
            {
                //Debug.Log("On cooldown");
                return false;
            }

            if (state == AbilityStates.casting)
            {
                //Debug.Log("Ability already casting");
                return false;
            }
            return true;
        }

        public virtual IEnumerator HandleCast(CastInfo caster)
        {
            caster.owner.state = CharacterStates.casting;
            state = AbilityStates.casting;

            CastStarted(caster);

            yield return new WaitForSeconds(CastTime);

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

        /// <summary>
        /// Called when the cast is started
        /// </summary>
        /// <param name="caster"></param>
        public virtual void CastStarted(CastInfo caster)
        {
            foreach (var effect in CastWindUp)
            {
                effect.ApplyEffect(caster, null);
            }
        }


        /// <summary>
        /// Called when the cast has ended
        /// </summary>
        /// <param name="caster"></param>
        public virtual void CastEnded(CastInfo caster)
        {
        }

    }
}