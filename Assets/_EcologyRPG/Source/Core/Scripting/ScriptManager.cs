using EcologyRPG.AbilityScripting;
using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;
using System;

namespace EcologyRPG.Core.Scripting
{
    public class ScriptManager
    {
        static ScriptManager instance;
        public static ScriptManager Current => instance;

        public static void Create()
        {
            instance = new ScriptManager();
            instance.Initialize();
        }

        public void Initialize()
        {
            UserData.RegisterProxyType<ResourceContext, Resource>(r => new ResourceContext(r));
            UserData.RegisterProxyType<CharacterContext, BaseCharacter>(c => new CharacterContext(c));
            UserData.RegisterProxyType<StatContext, Stat>(s => new StatContext(s));
            UserData.RegisterProxyType<IndicatorMeshContext, IndicatorMesh>(s => new IndicatorMeshContext(s));
            UserData.RegisterProxyType<BasicProjectileContext, BasicProjectileBehaviour>(s => new BasicProjectileContext(s));
            UserData.RegisterProxyType<CurvedProjectileContext, CurvedProjectileBehaviour>(s => new CurvedProjectileContext(s));
            UserData.RegisterType<Vector3Context>();
            UserData.RegisterType<CastContext>();
            UserData.RegisterAssembly();

        }

        public Script CreateScript()
        {
            var script = new Script(CoreModules.Preset_HardSandbox);
            ScriptUtility.AddContext(script);
            script.Globals["Vector3"] = (Func<float, float, float, Vector3Context>)Vector3Context._Vector3;
            ProjectileUtility.AddToGlobal(script);
            Targets.AddToGlobal(script);

            return script;
        }
    }
}