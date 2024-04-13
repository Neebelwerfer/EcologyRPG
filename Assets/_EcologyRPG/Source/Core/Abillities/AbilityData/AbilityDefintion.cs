using EcologyRPG.Core.Abilities.AbilityComponents;
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
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
        public Vector3 targetPoint;
        public InputAction activationInput;
    }

    public struct DamageInfo
    {
        public DamageType type;
        public float damage;
        public object source;
    }

    public abstract class AbilityDefintion : ScriptableObject
    {
        public string GUID { get { return guid; } }
        public string DisplayName;
        [Tooltip("The cooldown of this ability")]
        public float Cooldown = 0.5f;
        [Tooltip("The cast time of this ability. Cast Started will always be called first, and then after the Cast Time has been waited Cast ended will be called")]
        public float CastWindupTime = 0;
        public Sprite Icon;

        public UnityEvent AbilityChanged = new UnityEvent();
        public List<BuffCondition> BuffsOnCast;

        [HideInInspector] public float remainingCooldown = 0;
        [HideInInspector] public AbilityStates state = AbilityStates.ready;
        BaseCharacter owner;

        public List<AbilityComponent> CastWindUp = new List<AbilityComponent>();
        private string guid;

        public virtual void Initialize(BaseCharacter owner, AbilityDefintion prefabAbility)
        {
            guid = prefabAbility.GUID;
            this.owner = owner;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (guid == null || guid == "")
            {
                guid = System.Guid.NewGuid().ToString();
            }
        }

        public virtual void CopyComponentsTo(AbilityDefintion ability)
        {
            ability.CastWindUp = new List<AbilityComponent>();
            for (int i = 0; i < CastWindUp.Count; i++)
            {
                var newEffect = CastWindUp[i].GetCopy(ability);
                ability.CastWindUp.Add(newEffect);
            }
        }

        public virtual AbilityDefintion GetCopy(Object owner)
        {
            return Instantiate(this);
        }
#endif

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
                    OnReady();
                }
            }
        }

        protected virtual void OnReady()
        {

        }

        public virtual void PutOnCooldown(float timer)
        {
            if(timer <= 0)
            {
                state = AbilityStates.ready;
                return;
            }

            state = AbilityStates.cooldown;
            remainingCooldown = timer;
            AbilityManager.Instance.RegisterAbilityOnCooldown(this);
        }

        public virtual void PutOnCooldown()
        {
            PutOnCooldown(Cooldown);
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
                return false;
            }

            if (state == AbilityStates.casting)
            {
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

            CastFinished(caster);

            caster.owner.state = CharacterStates.active;
            
            PutOnCooldown();
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

        public virtual void CastCancelled(CastInfo caster)
        {
            state = AbilityStates.ready;
        }


        /// <summary>
        /// Called when the cast has ended
        /// </summary>
        /// <param name="caster"></param>
        public virtual void CastFinished(CastInfo caster)
        {
        }

#if UNITY_EDITOR

        [ContextMenu("Delete")]
        public virtual void Delete()
        {
            foreach (var effect in CastWindUp)
            {
                effect.Delete();
            }
            DestroyImmediate(this, true);
        }
#endif
    }
}