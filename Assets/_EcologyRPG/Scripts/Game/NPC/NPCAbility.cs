using EcologyRPG.Core.Abilities;
using UnityEngine;

namespace EcologyRPG.Game.NPC
{
    [CreateAssetMenu(menuName = "Ability/NPC Ability Data", fileName = "New NPC Ability Data")]
    public class NPCAbility : AbilityDefintion
    {
        public BaseAbility Ability;

        public override void CastEnded(CastInfo caster)
        {
            base.CastEnded(caster);
            caster.castPos = caster.owner.CastPos;
            caster.dir = caster.owner.Transform.Forward;
            Ability.Cast(caster);
        }
    }
}