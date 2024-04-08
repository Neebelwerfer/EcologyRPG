using EcologyRPG.Core.Abilities.AbilityComponents;
using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities.Conditions
{
    public class DashCondition : BuffCondition, IFixedUpdateCondition
    {
        public DirectionMode directionMode = DirectionMode.Mouse;
        public float dashRange = 10f;
        public bool StopOnHit = true;

        public List<AbilityComponent> OnFirstHitEffects;
        public List<AbilityComponent> OnHitEffects;

        Vector3 direction;

        float dodgeSpeed;
        bool firstHit = true;
        Collider[] hits;
        List<BaseCharacter> haveHit;

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
            target.Rigidbody.excludeLayers = Game.Settings.EntityMask;
            target.Rigidbody.isKinematic = false;
            hits = new Collider[6];
            haveHit = new List<BaseCharacter>();
        }

        void OnHit(BaseCharacter target)
        {
            CastInfo info = new()
            {
                owner = Owner,
                castPos = Owner.Transform.Position
            };
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
            if (BaseCharacter.IsLegalMove(target, direction.normalized, dodgeSpeed * deltaTime))
            {
                target.Rigidbody.velocity = dodgeSpeed * direction.normalized;
            } 
            else
            {
                target.Rigidbody.velocity = Vector3.zero;
            }

            var numHits = Physics.OverlapSphereNonAlloc(target.Transform.Position, target.Collider.radius + 1, hits, Game.Settings.EntityMask);

            if(numHits > 0)
            {
                for (int i = 0; i < numHits; i++)
                {
                    var hit = hits[i];
                    if (hit == null) continue;
                    if (hit.gameObject == target.GameObject) continue;

                    if (!hit.TryGetComponent<CharacterBinding>(out var hitChar)) continue;
                    if (hitChar.Character == target) continue;
                    if(haveHit.Contains(hitChar.Character)) continue;
                    haveHit.Add(hitChar.Character);
                    OnHit(hitChar.Character);
                }
            }
        }

        public override void OnRemoved(BaseCharacter target)
        {
            target.Rigidbody.excludeLayers = 0;
            target.Rigidbody.velocity = Vector3.zero;
            target.state = CharacterStates.active;
            target.OnCharacterCollision.RemoveListener(OnHit);
        }
    }
}