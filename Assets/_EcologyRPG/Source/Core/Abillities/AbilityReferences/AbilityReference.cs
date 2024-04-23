using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Scripting;
using MoonSharp.Interpreter;
using System;
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
        [Min(0)]
        public float Cooldown;

        public bool customActivationTest = false;
        [TextArea(3, 10)]
        public string CanActivateString = 
@"function CanActivate()
    return true
end
";

        public Script Behaviour => behaviour;
        public AbilityData AbilityData => abilityData;
        public BaseCharacter Owner { get; private set; }

        protected Script behaviour;
        protected AbilityData abilityData;
        [SerializeReference]
        public GlobalVariable[] globalVariablesOverride;
        [HideInInspector]  public CastState State = CastState.Ready;

        [HideInInspector] public float RemainingCooldown => remainingCooldown;

        float remainingCooldown = 0;

        public virtual void Init(BaseCharacter owner)
        {
            Owner = owner;
            this.abilityData = AbilityManager.Current.GetAbility(AbilityID);
            this.behaviour = abilityData.LoadBehaviour();
            LoadGlobals();
        }

        void LoadGlobals()
        {
            foreach (var global in abilityData._DefaultGlobalVariables)
            {
                if(globalVariablesOverride == null || !HasOverride(global.Name))
                {
                    behaviour.Globals[global.Name] = global.GetDynValue();
                }
                else
                {
                    var overriden = Array.Find(globalVariablesOverride, element => element.Name == global.Name);
                    behaviour.Globals[global.Name] = overriden.GetDynValue();
                }
            }
        }

        bool HasOverride(string name)
        {
            return Array.Exists(globalVariablesOverride, element => element.Name == name);
        }

        public void StartCooldown(float cooldown)
        {
            State = CastState.Cooldown;
            remainingCooldown = cooldown;
            Debug.Log("Cooldown started");
        }

        public void StartCooldown()
        {
            StartCooldown(Cooldown);
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
                if(!customActivationTest) return true;

                var canCast = behaviour.DoString(CanActivateString);
                if (canCast.IsNotNil() && canCast.Boolean)
                {
                    return canCast.Boolean;
                } else return true;
            }
            return false;
        }

        public virtual void OnCastCancelled()
        {
            behaviour.Call(behaviour.Globals["OnCastCancelled"]);
            State = CastState.Ready;
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