using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Abilities
{
    public enum AbilityStates
    {
        ready,
        casting,
        cooldown
    }

    public class BaseAbility
    {
        public string name;

        public AbilityStates state = AbilityStates.ready;

        public void Activate(BaseCharacter caster)
        {
            if (!CanActivate(caster)) return;
        }

        public bool CanActivate(BaseCharacter caster)
        {
            return state == AbilityStates.ready && caster.state == CharacterStates.active;
        }
    }
}