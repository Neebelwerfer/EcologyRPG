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

    public class AbilityReference
    {
        public readonly Script behaviour;
        public readonly AbilityData abilityData;
        public readonly BaseCharacter Owner;
        public CastState State = CastState.Ready;

        public float RemainingCooldown => remainingCooldown;

        float remainingCooldown = 0;

        public AbilityReference(AbilityData abilityData, BaseCharacter owner)
        {
            Owner = owner;
            this.abilityData = abilityData;
            this.behaviour = abilityData.LoadBehaviour();

        }

        public void StartCooldown()
        {
            State = CastState.Cooldown;
            remainingCooldown = abilityData.Cooldown;
            Debug.Log("Starting cooldown");
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
            if (State == CastState.Ready)
            {
                var castContext = CreateCastContext();
                behaviour.Globals["Context"] = castContext;
                var canCast = behaviour.Call(behaviour.Globals["CanActivate"]);
                if (canCast.Boolean)
                {
                    HandleCast(castContext);
                }
            }
        }
    }
}