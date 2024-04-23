using EcologyRPG.Core.Character;
using EcologyRPG.Core.Scripting;
using MoonSharp.Interpreter;
using System;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    public enum CastState
    {
        Ready,
        Casting,
        Cooldown
    }

    public class AbilityReference : ScriptableObject
    {
        [AbilityAttribute]
        public int AbilityID;
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
            LoadBehaviour();
            AbilityManager.Current.RegisterAbilityReference(this);
        }

        protected virtual void LoadBehaviour()
        {
            behaviour = abilityData.LoadBehaviour();
            LoadGlobals();
        }

        public void RefreshBehaviour()
        {
            State = CastState.Ready;
            remainingCooldown = 0;
            LoadBehaviour();
        }

        void LoadGlobals()
        {
            for (int i = 0; i < abilityData._DefaultGlobalVariables.Length; i++)
            {
                if (!HasOverride(abilityData._DefaultGlobalVariables[i].Name))
                {
                    behaviour.Globals[abilityData._DefaultGlobalVariables[i].Name] = abilityData._DefaultGlobalVariables[i].GetDynValue();
                }
                else
                {
                    behaviour.Globals[abilityData._DefaultGlobalVariables[i].Name] = globalVariablesOverride[i].GetDynValue();
                }
            }
            //if(globalVariablesOverride == null)
            //{
            //    abilityData.LoadDefaultVariables(behaviour);
            //    return;
            //}

            //foreach (var global in globalVariablesOverride)
            //{
            //    behaviour.Globals[global.Name] = global.GetDynValue();
            //}
        }

        bool HasOverride(string name)
        {
            return Array.Exists(globalVariablesOverride, element => element.Name == name);
        }

        public void StartCooldown(float cooldown)
        {
            State = CastState.Cooldown;
            remainingCooldown = cooldown;
            AbilityManager.Current.RegisterCooldown(this);
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

        private void OnDestroy()
        {
            if(AbilityManager.Current != null)
                AbilityManager.Current.UnregisterAbilityReference(this);
        }
    }
}