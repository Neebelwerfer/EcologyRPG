using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;
using UnityEngine;

namespace EcologyRPG.AbilityScripting
{
    public enum CastState
    {
        Ready,
        Casting,
        Cooldown
    }

    public class AbilityReference : ScriptableObject
    {
        public uint AbilityID;
        public Script Behaviour => behaviour;
        public AbilityData AbilityData => abilityData;
        public BaseCharacter Owner { get; private set; }

        protected Script behaviour;
        protected AbilityData abilityData;
        [HideInInspector]  public CastState State = CastState.Ready;

        [HideInInspector] public float RemainingCooldown => remainingCooldown;

        float remainingCooldown = 0;

        public virtual void Init(BaseCharacter owner)
        {
            Owner = owner;
            this.abilityData = AbilityManager.Current.GetAbility(AbilityID);
            this.behaviour = abilityData.LoadBehaviour();
        }

        public void StartCooldown(float cooldown)
        {
            State = CastState.Cooldown;
            remainingCooldown = cooldown;
            Debug.Log("Cooldown started");
        }

        public void StartCooldown()
        {
            StartCooldown(abilityData.Cooldown);
        }

        public void Update()
        {
            if (State == CastState.Cooldown)
            {
                remainingCooldown -= Time.deltaTime;
                if (remainingCooldown <= 0)
                {
                    State = CastState.Ready;
                    Debug.Log("Cooldown finished");
                }
            }
        }

        public virtual bool CanActivate()
        {
            if(State == CastState.Ready && Owner.state == CharacterStates.active)
            {
                return true;
                //var canCast = behaviour.Call(behaviour.Globals["CanActivate"]);
                //return canCast.Boolean;
            }
            return false;
        }

        public virtual void OnCastCancelled()
        {
            behaviour.Call(behaviour.Globals["OnCastCancelled"]);
        }

        public virtual void HandleCast(CastContext castContext)
        {
            AbilityManager.Current.CastAbility(this, castContext);
        }

        public virtual CastContext CreateCastContext()
        {
            return new CastContext(Owner, new Vector3Context(Owner.CastPos), new Vector3Context(Owner.Transform.Forward));
        }

        public void Cast()
        {
            var castContext = CreateCastContext();
            behaviour.Globals["Context"] = castContext;
            if (CanActivate())
            {
                HandleCast(castContext);
            }
        }
    }
}