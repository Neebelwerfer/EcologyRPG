using EcologyRPG.AbilityScripting;
using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;

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
            UserData.RegisterProxyType<CharacterContext, BaseCharacter>(c => CharacterContext.GetOrCreate(c));
            UserData.RegisterProxyType<StatContext, Stat>(s => new StatContext(s));
            UserData.RegisterProxyType<IndicatorMeshContext, IndicatorMesh>(s => new IndicatorMeshContext(s));
            UserData.RegisterProxyType<BasicProjectileContext, BasicProjectileBehaviour>(s => new BasicProjectileContext(s));
            UserData.RegisterProxyType<CurvedProjectileContext, CurvedProjectileBehaviour>(s => new CurvedProjectileContext(s));
            UserData.RegisterProxyType<StatModifierContext, StatModification>(s => new StatModifierContext(s));
            UserData.RegisterType<QuaternionContext>();
            UserData.RegisterType<PhysicsContext>();
            UserData.RegisterType<Vector3Context>();
            UserData.RegisterType<CastContext>();
            UserData.RegisterType<EventArgs>();
            UserData.RegisterAssembly();

        }

        public Script CreateScript()
        {
            var script = new Script(CoreModules.Preset_HardSandbox);
            ScriptUtility.AddContext(script);
            script.Globals["Vector3"] = typeof(Vector3Context);
            script.Globals["Physics"] = typeof(PhysicsContext);
            script.Globals["StatModifier"] = (Func<CastContext, string, float, int, StatModifierContext>)CreateStatModifier;
            QuaternionContext.Register(script);
            ProjectileUtility.AddToGlobal(script);
            Targets.AddToGlobal(script);

            return script;
        }


        static StatModifierContext CreateStatModifier(CastContext context, string StatName, float Value, int ModType)
        {
            return new StatModifierContext(new StatModification(StatName, Value, (StatModType)ModType, context.GetOwner()));
        }
    }
}