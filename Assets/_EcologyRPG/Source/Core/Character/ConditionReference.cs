using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Scripting;
using EcologyRPG.Utility;
using MoonSharp.Interpreter;
using System;
using UnityEngine;


namespace EcologyRPG.Core.Character
{
    public class ConditionReference
    {
        readonly ConditionReferenceData data;

        public float remainingDuration;
        public CastContext CastContext;
        public int ID => data.ID;
        public int ConditionBehaviourID => data.ConditionBehaviourID;
        public bool useFixedUpdate => data.useFixedUpdate;
        public float duration => data.duration;

        ConditionData conditionData;
        Script scriptContext;

        CharacterContext target;

        DynValue OnApplyFunction;
        DynValue OnReapplyFunction;
        DynValue OnUpdateFunction;
        DynValue OnRemovedFunction;

        public ConditionReference(ConditionReferenceData data)
        {
            this.data = data;
            conditionData = AbilityManager.Current.GetCondition(data.ConditionBehaviourID);
            scriptContext = conditionData.LoadBehaviour();

            scriptContext.Globals["Duration"] = data.duration;
            scriptContext.Globals["SetRemainingDuration"] = (Action<float>)SetRemainingDuration;

            OnApplyFunction = scriptContext.Globals.Get("OnApply");
            OnReapplyFunction = scriptContext.Globals.Get("OnReapply");
            OnUpdateFunction = scriptContext.Globals.Get("OnUpdate");
            OnRemovedFunction = scriptContext.Globals.Get("OnRemoved");

            LoadGlobalVariables(scriptContext);
        }

        public void OnApply(CastContext Context, BaseCharacter target)
        {
            Debug.Log("Applying condition " + data.name);
            CastContext = Context;
            this.target = CharacterContext.GetOrCreate(target);
            scriptContext.Globals["Context"] = Context;
            scriptContext.Globals["Target"] = this.target;

            if (OnApplyFunction.Type == DataType.Function)
            {
                scriptContext.Call(OnApplyFunction);
            } 
            else
            {
                Debug.LogWarning("OnApply function not found for condition " + data.name);
            }
        }

        public void OnReapply()
        {
            if (OnReapplyFunction.Type == DataType.Function)
            {
                scriptContext.Call(OnReapplyFunction);
            }
            else
            {
                Debug.LogWarning("OnReapply function not found for condition " + data.name);
            }
        }

        public void OnUpdate(float deltaTime)
        {
            scriptContext.Globals["RemainingDuration"] = remainingDuration;
            if (OnUpdateFunction.Type == DataType.Function)
            {
                scriptContext.Call(OnUpdateFunction, deltaTime);
            }
            else
            {
                Debug.LogWarning("OnUpdate function not found for condition " + data.name);
            }
        }

        public void OnRemoved()
        {
            if (OnRemovedFunction.Type == DataType.Function)
            {
                scriptContext.Call(OnRemovedFunction);
            }
            else
            {
                Debug.LogWarning("OnRemoved function not found for condition " + data.name);
            }
            ConditionReferenceDatabase.ReturnCondition(this);
        }

        public void SetRemainingDuration(float duration)
        {
            remainingDuration = duration;
        }

        void LoadGlobalVariables(Script context)
        {
            foreach (var variable in conditionData._DefaultGlobalVariables)
            {
                if(data.variableOverrides != null && Array.Exists(data.variableOverrides, x => x.Name == variable.Name))
                {
                    var overrideVariable = Array.Find(data.variableOverrides, x => x.Name == variable.Name);
                    context.Globals[variable.Name] = overrideVariable.GetDynValue(context);
                }
                else
                {
                    context.Globals[variable.Name] = variable.GetDynValue(context);
                }
            }
        }


        protected float CalculateDamage(BaseCharacter Owner, float damage, bool allowVariance = false) => AbilityManager.CalculateDamage(Owner, damage, allowVariance);

    }
}