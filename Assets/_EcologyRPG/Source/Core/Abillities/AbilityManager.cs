using EcologyRPG.AbilityScripting;
using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Scripting;
using EcologyRPG.Core.Systems;
using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace EcologyRPG.Core.Abilities
{
    class SubAbilityCast
    {
        public int AbilityID;
        public CastContext Context;
    }

    public sealed class AbilityManager : SystemBehavior, IUpdateSystem, IDisposable
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


        readonly AbilityData[] abilities;
        readonly List<AbilityReference> OnCooldown = new();
        readonly Queue<SubAbilityCast> subAbilityCasts = new();
        readonly List<AbilityReference> abilityReferences = new();
        readonly CoroutineHolder CoroutineHolder;
        InputActionReference ReloadAction;

        public bool Enabled => OnCooldown.Count > 0 || subAbilityCasts.Count > 0;

        AbilityManager() 
        {
            ProjectileDatabase.Load();
            abilities = AbilityData.LoadAll();
            var obj = new GameObject("AbilityCoroutineHolder");
            CoroutineHolder = obj.AddComponent<CoroutineHolder>();
        }

        public void RegisterAbilityReference(AbilityReference reference)
        {
            abilityReferences.Add(reference);
        }

        public void UnregisterAbilityReference(AbilityReference reference)
        {
            abilityReferences.Remove(reference);
        }

        public void SetReloadAction(InputActionReference action)
        {
            if (ReloadAction != null)
            {
                ReloadAction.action.performed -= ReloadAbilities;
                ReloadAction.action.Disable();
            }
            ReloadAction = action;
            ReloadAction.action.performed += ReloadAbilities;
            action.action.Enable();
        }

        public void ReloadAbilities(InputAction.CallbackContext ct)
        {
            foreach (var reference in abilityReferences)
            {
                Debug.Log("Reloading " + reference.AbilityData.abilityName);
                reference.RefreshBehaviour();
            }
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

        public void RegisterCooldown(AbilityReference ability)
        {
            OnCooldown.Add(ability);
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
                var ability = GetAbility((int)subAbility.AbilityID);
                if(ability != null)
                    CastAbilityDiscrete(ability, subAbility.Context, ability.LoadBehaviour());
            }

        }

        public AbilityData GetAbility(int ID)
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
            context.GetOwner().StartCoroutine(Cast(res, ability, context));
        }

        public void CastAbilityDiscrete(AbilityData Ability, CastContext context, Script scriptContext)
        {
            scriptContext.Globals["Context"] = context; 
            var OnCast = scriptContext.Globals.Get("OnCast");
            var coroutine = scriptContext.CreateCoroutine(OnCast);
            CoroutineHolder.StartCoroutine(CastDiscrete(coroutine, Ability));   
        }

        IEnumerator Cast(DynValue abilityContext, AbilityReference reference, CastContext context)
        {
            context.GetOwner().state = CharacterStates.casting;
            reference.State = CastState.Casting;
            foreach (var res in abilityContext.Coroutine.AsTypedEnumerable())
            {
                if (res.Type == DataType.Number)
                {
                    yield return new WaitForSeconds((float)res.Number);
                }
            }
            context.GetOwner().state = CharacterStates.active;
            reference.StartCooldown();
        }

        IEnumerator CastDiscrete(DynValue abilityContext, AbilityData data)
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
            var scriptContext = ScriptManager.Current.CreateScript();
            scriptContext.Globals["CastAbility"] = (Action<int, CastContext>)CastSubAbility;
            scriptContext.Globals["Physical"] = DamageType.Physical;
            scriptContext.Globals["Water"] = DamageType.Water;
            scriptContext.Globals["Toxic"] = DamageType.Toxic;
            scriptContext.Globals["CalculateDamage"] = (Func<BaseCharacter, float, bool, bool, float>)CalculateDamage;
            scriptContext.Globals["CreateCastContext"] = (Func<BaseCharacter, Vector3Context, Vector3Context, CastContext>)CreateCastContext;
            VisualUtility.Register(scriptContext);
            
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
            if(ReloadAction != null)
            {
                ReloadAction.action.performed -= ReloadAbilities;
                ReloadAction.action.Disable();
            }
            base.Dispose();
        }
    }

    class CoroutineHolder : MonoBehaviour 
    { 
        
    }
}