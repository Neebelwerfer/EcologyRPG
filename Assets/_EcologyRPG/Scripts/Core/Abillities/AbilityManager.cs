using EcologyRPG.Core.Systems;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EcologyRPG.Core.Abilities
{
    public class AbilityManager : SystemBehavior, IUpdateSystem, IDisposable
    {
        public static AbilityManager Instance;

        public static LayerMask TargetMask { get; private set; }
        public static LayerMask GroundMask { get; private set; }

        readonly List<AbilityDefintion> CooldownAbilities;

        public bool Enabled => CooldownAbilities.Count > 0;

        public static void Init(LayerMask targetMask, LayerMask targetGroundMask)
        {
            AbilityManager.TargetMask = targetMask;
            AbilityManager.GroundMask = targetGroundMask;
            Instance = new();
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