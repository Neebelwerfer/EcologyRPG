using EcologyRPG.AbilityScripting;
using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Systems;
using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EcologyRPG.Core.Abilities
{
    public class AbilityManager : SystemBehavior, IUpdateSystem, IDisposable
    {
        public const string AbilityPath = "Abilities/";
        public const string AbilityFullpath = "Assets/_EcologyRPG/Resources/Abilities/";
        public const string AbilityDataPath = "Assets/_EcologyRPG/Resources/Abilities/AbilityData.json";
        public const string AbilityScriptExtension = ".lua";

        public static Vector3 IndicatorOffset = new Vector3(0, 0.2f, 0);

        public static IndicatorMesh IndicatorMesh { get; private set; }
        public static LayerMask TargetMask { get; private set; }
        public static LayerMask GroundMask { get; private set; }
        public static LayerMask WalkableGroundLayer { get; private set; }
        public static bool UseToxic { get; set; } = false;
        public static UnityEvent<bool> OnToxicModeChanged;
        public static string ToxicResourceName { get; set; } = "Toxic Water";


        public static AbilityManager Current;

        AbilityData[] abilities;
        List<AbilityReference> OnCooldown = new();

        public bool Enabled => OnCooldown.Count > 0;

        AbilityManager() 
        { 
            UserData.RegisterProxyType<ResourceContext, Resource>(r => new ResourceContext(r));
            UserData.RegisterProxyType<CharacterContext, BaseCharacter>(c => new CharacterContext(c));
            UserData.RegisterProxyType<StatContext, Stat>(s => new StatContext(s));
            UserData.RegisterProxyType<IndicatorMeshContext, IndicatorMesh>(s => new IndicatorMeshContext(s));
            UserData.RegisterType<Vector3Context>();
            UserData.RegisterType<CastContext>();
            UserData.RegisterAssembly();
            ProjectileDatabase.Load();
            
            abilities = AbilityData.LoadAll();
        }

        public static void SetSettings(LayerMask targetMask, LayerMask groundMask, LayerMask walkableGroundLayer, IndicatorMesh indicatorMesh)
        {
            IndicatorMesh = indicatorMesh;
            TargetMask = targetMask;
            GroundMask = groundMask;
            WalkableGroundLayer = walkableGroundLayer;
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
                if (ability.State == CastState.Ready)
                {
                    OnCooldown.RemoveAt(i);
                }
            }
        }

        public AbilityData GetAbility(uint ID)
        {
            foreach (var ability in abilities)
            {
                if (ability.ID == ID)
                {
                    return ability;
                }
            }
            return null;
        }

        public void CastAbility(AbilityReference ability, CastContext context)
        {
            var scriptContext = ability.Behaviour;
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
            scriptContext.Globals["Physical"] = DamageType.Physical;
            scriptContext.Globals["Water"] = DamageType.Water;
            scriptContext.Globals["Toxic"] = DamageType.Toxic;
            Targets.AddToGlobal(scriptContext);
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

        public static DamageInfo CalculateDamage(BaseCharacter caster, DamageType damageType, float BaseDamage, bool allowVariance = true, bool useWeaponDamage = false)
        {
            DamageInfo damageInfo = new()
            {
                type = damageType,
                source = caster
            };

            Stat ad;
            if (useWeaponDamage)
            {
                ad = caster.Stats.GetStat("weaponDamage");
            }
            else
            {
                ad = caster.Stats.GetStat("abilityDamage");
            }
            var damageVariance = allowVariance ? caster.Random.NextFloat(0.9f, 1.1f) : 1;
            damageInfo.damage = (BaseDamage * ad.Value) * damageVariance;

            return damageInfo;
        }

        public override void Dispose()
        {

        }
    }
}