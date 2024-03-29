using EcologyRPG.Core.Abilities.AbilityComponents;
using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using System.Collections.Generic;
using UnityEngine;
using EcologyRPG.Game.Abilities.Implementations;

namespace EcologyRPG.Game.Abilities.Conditions
{
    public class DashCondition : BuffCondition
    {
        public DirectionMode directionMode = DirectionMode.Mouse;
        public float dashRange = 10f;
        public bool StopOnHit = true;

        public List<AbilityComponent> OnFirstHitEffects;
        public List<AbilityComponent> OnHitEffects;

        Vector3 direction;

        float dodgeSpeed;
        bool firstHit = true;

        public override void OnApply(CastInfo caster, BaseCharacter target)
        {
            if (directionMode == DirectionMode.Mouse)
            {
                var lookAt = TargetUtility.GetMousePoint(Camera.main);
                lookAt.y = caster.owner.Transform.Position.y;
                direction = (lookAt - caster.owner.Transform.Position);
            }
            else
            {
                direction = caster.owner.Transform.Forward.normalized;
            }
            dodgeSpeed = dashRange / duration;
            target.OnCharacterCollision.AddListener(OnHit);
        }

        void OnHit(BaseCharacter target)
        {
            CastInfo info = new();
            info.owner = Owner;
            info.castPos = Owner.Transform.Position;
            if (StopOnHit) remainingDuration = 0;
            if (firstHit)
            {
                firstHit = false;
                foreach (var effect in OnFirstHitEffects)
                {
                    effect.ApplyEffect(info, target);
                }
            }
            foreach (var effect in OnHitEffects)
            {
                effect.ApplyEffect(info, target);
            }
        }

        public override void OnReapply(BaseCharacter target)
        {

        }

        public override void OnUpdate(BaseCharacter target, float deltaTime)
        {
            if (target.state == CharacterStates.disabled)
            {
                remainingDuration = 0;
                return;
            }
            target.state = CharacterStates.dodging;
            //target.Rigidbody.isKinematic = false;
            //target.Rigidbody.velocity = dodgeSpeed * direction.normalized;
            target.Transform.Position += deltaTime * dodgeSpeed * direction.normalized;
        }

        public override void OnRemoved(BaseCharacter target)
        {
            target.state = CharacterStates.active;
            target.OnCharacterCollision.RemoveListener(OnHit);
        }
    }
}