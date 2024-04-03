using EcologyRPG.Core.Abilities.AbilityComponents;
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EcologyRPG.Core.Abilities
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

    public abstract class AbilityDefintion : ScriptableObject
    {
        public string DisplayName;
        [Tooltip("The cooldown of this ability")]
        public float Cooldown = 0.5f;
        [Tooltip("The cast time of this ability. Cast Started will always be called first, and then after the Cast Time has been waited Cast ended will be called")]
        public float CastWindupTime = 0;
        public Sprite Icon;

        public List<BuffCondition> BuffsOnCast;

        [HideInInspector] public float remainingCooldown = 0;
        [HideInInspector] public AbilityStates state = AbilityStates.ready;
        BaseCharacter owner;

        public List<AbilityComponent> CastWindUp = new List<AbilityComponent>();

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

        public virtual bool Activate(CastInfo castInfo)
        {
            if (!CanActivate(castInfo.owner)) return false;
            EventManager.Defer("OnAbilityCast", new AbilityCastEvent(castInfo, this));
            castInfo.owner.StartCoroutine(HandleCast(castInfo));
            return true;
        }   

        public virtual bool CanActivate(BaseCharacter caster)
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

            if (caster.state != CharacterStates.active)
            {
                return false;
            }

            return true;
        }

        public virtual IEnumerator HandleCast(CastInfo caster)
        {
            caster.owner.state = CharacterStates.casting;
            state = AbilityStates.casting;

            CastStarted(caster);

            yield return new WaitForSeconds(CastWindupTime);

            CastEnded(caster);

            caster.owner.state = CharacterStates.active;
            
            if(Cooldown > 0)
            {
                state = AbilityStates.cooldown;
                remainingCooldown = Cooldown;
                AbilityManager.Instance.RegisterAbilityOnCooldown(this);
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


        [ContextMenu("Delete")]
        protected virtual void Delete()
        {
            DestroyImmediate(this, true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

    }
}