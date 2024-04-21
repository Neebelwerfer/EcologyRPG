using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

namespace EcologyRPG.AbilityTest
{
    public class AbilityManager
    {
        public const string AbilityPath = "Abilities/";
        public const string AbilityFullpath = "Assets/_EcologyRPG/Resources/Abilities/";
        public const string AbilityDataPath = "Assets/_EcologyRPG/Resources/Abilities/AbilityData.json";
        public const string AbilityScriptExtension = ".lua";

        public static AbilityManager Current;
        readonly Script scriptContext;
        AbilityManager() 
        { 
            scriptContext = new Script(CoreModules.Preset_HardSandbox);
            UserData.RegisterProxyType<ResourceContext, Resource>(r => new ResourceContext(r));
            UserData.RegisterProxyType<CharacterContext, BaseCharacter>(c => new CharacterContext(c));
            UserData.RegisterProxyType<StatContext, Stat>(s => new StatContext(s));
            UserData.RegisterType<Vector3Context>();
            UserData.RegisterType<CastContext>();
            UserData.RegisterAssembly();

        }

        internal static DynValue Delay(float seconds)
        {
            return DynValue.NewYieldReq(new DynValue[] { DynValue.NewNumber(seconds) });
        }

        internal static void Log(string message)
        {
            Debug.Log(message);
        }

        public static void Create()
        {
            Current ??= new AbilityManager();
        }

        public static void Delete()
        {
            Current = null;
        }

        public void CastAbility(AbilityData Ability, CastContext context)
        {
            scriptContext.Globals["Context"] = context;
            scriptContext.Globals["Delay"] = (Func<float, DynValue>)Delay;
            scriptContext.Globals["Log"] = (Action<string>)Log;
            scriptContext.Globals["Vector3"] = (Func<float, float, float, Vector3Context>)Vector3Context._Vector3;

            var script = System.IO.File.ReadAllText(Ability.ScriptPath);
            var loaded = scriptContext.DoString(script);
            var canCast = scriptContext.Call(scriptContext.Globals["CanActivate"]);
            if(canCast.Boolean)
            {
                var OnCast = scriptContext.Globals.Get("OnCast");
                var res = scriptContext.CreateCoroutine(OnCast);
                context.GetOwner().StartCoroutine(Cast(res, Ability));   
            }
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

    }
}