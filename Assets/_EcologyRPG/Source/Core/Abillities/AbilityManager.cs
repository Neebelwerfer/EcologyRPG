using EcologyRPG.Core.Systems;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace EcologyRPG.Core.Abilities
{
    public class AbilityManager : SystemBehavior, IUpdateSystem, IDisposable
    {
        public static AbilityManager Instance;

        public static Vector3 IndicatorOffset = new Vector3(0, 0.2f, 0);

        public static IndicatorMesh IndicatorMesh { get; private set; }
        public static LayerMask TargetMask { get; private set; }
        public static LayerMask GroundMask { get; private set; }
        public static LayerMask WalkableGroundLayer { get; private set; }
        public static bool UseToxic { get; set; } = false;
        public static UnityEvent<bool> OnToxicModeChanged;
        public static string ToxicResourceName { get; set; } = "Toxic Water";

        readonly List<AbilityDefintion> CooldownAbilities;

        public bool Enabled => CooldownAbilities.Count > 0;

        public static void Init(LayerMask targetMask, LayerMask targetGroundMask, LayerMask WalkableGroundLayer, IndicatorMesh indicatorMesh)
        {
            AbilityManager.TargetMask = targetMask;
            AbilityManager.GroundMask = targetGroundMask;
            AbilityManager.WalkableGroundLayer = WalkableGroundLayer;
            OnToxicModeChanged ??= new UnityEvent<bool>();
            Instance = new();
            IndicatorMesh = indicatorMesh;
        }

        public AbilityManager()
        {
            CooldownAbilities = new List<AbilityDefintion>();
        }

        public void OnUpdate()
        {
            for (int i = CooldownAbilities.Count - 1; i >= 0; i--)
            {
                if (CooldownAbilities[i] == null)
                {
                    CooldownAbilities.RemoveAt(i);
                    continue;
                }

                if (CooldownAbilities[i].state == AbilityStates.ready)
                {
                    CooldownAbilities.RemoveAt(i);
                    continue;
                }

                CooldownAbilities[i].UpdateCooldown(Time.deltaTime);
            }
        }

        public void RegisterAbilityOnCooldown(AbilityDefintion ability)
        {
            if (!CooldownAbilities.Contains(ability))
            {
                CooldownAbilities.Add(ability);
            }
        }

        public void UnregisterAbilityOnCooldown(AbilityDefintion ability)
        {
            if (CooldownAbilities.Contains(ability))
            {
                CooldownAbilities.Remove(ability);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var ability in CooldownAbilities)
            {
                ability.remainingCooldown = 0;
                ability.state = AbilityStates.ready;
            }
            CooldownAbilities.Clear();
            Instance = null;
        }
    }
}