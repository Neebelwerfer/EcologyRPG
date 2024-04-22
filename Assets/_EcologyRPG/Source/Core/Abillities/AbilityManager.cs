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
    class SubAbilityCast
    {
        public int AbilityID;
        public CastContext Context;
    }

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
        public static LayerMask CurvedProjectileIgnoreMask { get; private set; }
        public static bool UseToxic { get; set; } = false;
        public static UnityEvent<bool> OnToxicModeChanged;
        public static string ToxicResourceName { get; set; } = "Toxic Water";


        public static AbilityManager Current;


        AbilityData[] abilities;
        List<AbilityReference> OnCooldown = new();
        Queue<SubAbilityCast> subAbilityCasts = new();

        public bool Enabled => OnCooldown.Count > 0 || subAbilityCasts.Count > 0;

        AbilityManager() 
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
            ProjectileDatabase.Load();
            
            abilities = AbilityData.LoadAll();
        }

        public static void SetSettings(LayerMask targetMask, LayerMask groundMask, LayerMask walkableGroundLayer, LayerMask curvedProjectileIgnoreMask, IndicatorMesh indicatorMesh)
        {
            IndicatorMesh = indicatorMesh;
            TargetMask = targetMask;
            GroundMask = groundMask;
            CurvedProjectileIgnoreMask = curvedProjectileIgnoreMask;
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

            while (subAbilityCasts.Count > 0)
            {
                var subAbility = subAbilityCasts.Dequeue();
                var ability = GetAbility((uint)subAbility.AbilityID);
                if(ability != null)
                    CastAbility(ability, subAbility.Context, ability.LoadBehaviour());
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

        public static Script CreateScriptContext()
        {
            var scriptContext = new Script(CoreModules.Preset_HardSandbox);
            scriptContext.Globals["Delay"] = (Func<float, DynValue>)Delay;
            scriptContext.Globals["Log"] = (Action<string>)Log;
            scriptContext.Globals["DrawLine"] = (Action<Vector3, Vector3, Color, float>)DrawLine;
            scriptContext.Globals["DrawRay"] = (Action<Vector3, Vector3, Color, float>)DrawRay;
            scriptContext.Globals["Vector3"] = (Func<float, float, float, Vector3Context>)Vector3Context._Vector3;
            scriptContext.Globals["CastAbility"] = (Action<int, CastContext>)CastSubAbility;
            scriptContext.Globals["Physical"] = DamageType.Physical;
            scriptContext.Globals["Water"] = DamageType.Water;
            scriptContext.Globals["Toxic"] = DamageType.Toxic;
            scriptContext.Globals["CalculateDamage"] = (Func<BaseCharacter, float, bool, bool, float>)CalculateDamage;
            scriptContext.Globals["CreateCastContext"] = (Func<BaseCharacter, Vector3Context, Vector3Context, CastContext>)CreateCastContext;
            ProjectileUtility.AddToGlobal(scriptContext);
            Targets.AddToGlobal(scriptContext);
            return scriptContext;
        }

        internal static CastContext CreateCastContext(BaseCharacter owner, Vector3Context target, Vector3Context origin)
        {
            return new CastContext(owner, target, origin);
        }

        internal static void CastSubAbility(int abilityID, CastContext context)
        {
            Current.subAbilityCasts.Enqueue(new SubAbilityCast() { AbilityID = abilityID, Context = context });
        }

        internal static DynValue Delay(float seconds)
        {
            return DynValue.NewYieldReq(new DynValue[] { DynValue.NewNumber(seconds) });
        }

        static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
        {
            Debug.DrawLine(start, end, color, duration);
        }

        static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration = 0.2f)
        {
            Debug.DrawRay(start, dir, color, duration);
        }

        internal static void Log(string message)
        {
            Debug.Log(message);
        }

        public static float CalculateDamage(BaseCharacter caster, float BaseDamage, bool allowVariance = true, bool useWeaponDamage = false)
        {
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
            return (BaseDamage * ad.Value) * damageVariance;
        }

        public override void Dispose()
        {
            Current = null;
        }
    }
}