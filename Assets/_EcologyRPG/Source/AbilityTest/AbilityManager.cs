using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;
using System;
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

        public static void Create()
        {
            Current ??= new AbilityManager();
        }

        public static void Delete()
        {
            Current = null;
        }
    }
}