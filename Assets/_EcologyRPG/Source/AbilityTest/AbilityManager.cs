using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Systems;
using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace EcologyRPG.AbilityScripting
{
    public class AbilityManager : SystemBehavior, IUpdateSystem
    {
        public const string AbilityPath = "Abilities/";
        public const string AbilityFullpath = "Assets/_EcologyRPG/Resources/Abilities/";
        public const string AbilityDataPath = "Assets/_EcologyRPG/Resources/Abilities/AbilityData.json";
        public const string AbilityScriptExtension = ".lua";

        public static AbilityManager Current;

        AbilityData[] abilities;
        List<AbilityReference> OnCooldown = new();

        public bool Enabled => true;

        AbilityManager() 
        { 
            UserData.RegisterProxyType<ResourceContext, Resource>(r => new ResourceContext(r));
            UserData.RegisterProxyType<CharacterContext, BaseCharacter>(c => new CharacterContext(c));
            UserData.RegisterProxyType<StatContext, Stat>(s => new StatContext(s));
            UserData.RegisterType<Vector3Context>();
            UserData.RegisterType<CastContext>();
            UserData.RegisterAssembly();
            
            abilities = AbilityData.LoadAll();
        }

        public static void Create()
        {
            Current ??= new AbilityManager();
        }

        public static void Delete()
        {
            Current = null;
        }

        public void OnUpdate()
        {
            for (int i = OnCooldown.Count - 1; i >= 0; i--)
            {
                var ability = OnCooldown[i];
                ability.Update();
                //Debug.Log(ability.RemainingCooldown);
                if (ability.State == CastState.Ready)
                {
                    OnCooldown.RemoveAt(i);
                }
            }
        }

        public AbilityData GetAbility(uint ID)
        {
            return Array.Find(abilities, a => a.ID == ID);
        }

        public void CastAbility(AbilityReference ability, CastContext context)
        {
            var scriptContext = ability.behaviour;
            var OnCast = scriptContext.Globals.Get("OnCast");
            var res = scriptContext.CreateCoroutine(OnCast);
            context.GetOwner().StartCoroutine(Cast(res, ability));
        }

        public void CastAbility(AbilityData Ability, CastContext context, Script scriptContext)
        {
            scriptContext.Globals["Context"] = context;            
            var OnCast = scriptContext.Globals.Get("OnCast");
            var coroutine = scriptContext.CreateCoroutine(OnCast);
            context.GetOwner().StartCoroutine(Cast(coroutine, Ability));   
        }

        IEnumerator Cast(DynValue abilityContext, AbilityReference reference)
        {
            reference.State = CastState.Casting;
            foreach (var res in abilityContext.Coroutine.AsTypedEnumerable())
            {
                if (res.Type == DataType.Number)
                {
                    yield return new WaitForSeconds((float)res.Number);
                }
            }
            reference.StartCooldown();
            OnCooldown.Add(reference);
        }

        IEnumerator Cast(DynValue abilityContext, AbilityData data)
        {
            foreach (var res in abilityContext.Coroutine.AsTypedEnumerable())
            {
                if (res.Type == DataType.Number)
                {
                    yield return new WaitForSeconds((float)res.Number);
                }
            }
        }

        public static Script CreateContext()
        {
            var scriptContext = new Script(CoreModules.Preset_HardSandbox);
            scriptContext.Globals["Delay"] = (Func<float, DynValue>)Delay;
            scriptContext.Globals["Log"] = (Action<string>)Log;
            scriptContext.Globals["Vector3"] = (Func<float, float, float, Vector3Context>)Vector3Context._Vector3;
            scriptContext.Globals["Cast"] = (Action<int, CastContext>)Cast;
            scriptContext.Globals["CreateLineIndicator"] = (Action<CastContext, float, float, float>)Targets.CreateLineIndicator;
            scriptContext.Globals["GetTargetsInLine"] = (Func<CastContext, float, float, List<BaseCharacter>>)Targets.GetTargetsInLine;
            return scriptContext;
        }

        internal static void Cast(int abilityID, CastContext context)
        {
            Current.CastAbility(Current.GetAbility((uint)abilityID), context, CreateContext());
        }

        internal static DynValue Delay(float seconds)
        {
            return DynValue.NewYieldReq(new DynValue[] { DynValue.NewNumber(seconds) });
        }

        internal static void Log(string message)
        {
            Debug.Log(message);
        }
    }
}