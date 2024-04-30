using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Core.Scripting
{
    public class ColliderArrayPool
    {
        Stack<Collider[]> pool = new Stack<Collider[]>();

        readonly int Size;

        public ColliderArrayPool(int size)
        {
            Size = size;
        }

        public Collider[] Get()
        {
            if(pool.Count > 0)
            {
                return pool.Pop();
            }
            return new Collider[Size];
        }

        public void Return(Collider[] array)
        {
            pool.Push(array);
        }
    }

    public class PhysicsContext
    {
        static readonly ColliderArrayPool colliderArrayPool = new ColliderArrayPool(10);

        public static CharacterContext[] OverlapSphere(CastContext context, Vector3Context position, float radius)
        {

            var hits = colliderArrayPool.Get();
            var numHits = Physics.OverlapSphereNonAlloc(position.Vector, radius, hits, AbilityManager.TargetMask, QueryTriggerInteraction.Ignore);
            if (numHits == 0) return null;

            var characters = new CharacterContext[hits.Length];

            int j = 0;
            for (int i = 0; i < numHits; i++)
            {
                var hit = hits[i];
                if(hit.TryGetComponent<CharacterBinding>(out var characterBinding))
                {
                    if(characterBinding.Character.Faction != context.GetOwner().Faction)
                    {
                        characters[j] = CharacterContext.GetOrCreate(characterBinding.Character);
                        j++;
                    }
                }
            }
            colliderArrayPool.Return(hits);
            var result = new CharacterContext[j];
            for (int i = 0; i < j; i++)
            {
                result[i] = characters[i];
            }
            return result;
        }

        public static CharacterContext[] OverlapBox(CastContext context, Vector3Context center, Vector3Context halfExtents, QuaternionContext orientation, int layerMask)
        {
            var hits = colliderArrayPool.Get();
            var numHits = Physics.OverlapBoxNonAlloc(center.Vector, halfExtents.Vector, hits, orientation.Value, layerMask, QueryTriggerInteraction.Ignore);
            if (numHits == 0) return null;

            var characters = new CharacterContext[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (hit.TryGetComponent<CharacterBinding>(out var characterBinding))
                {
                    if (characterBinding.Character.Faction != context.GetOwner().Faction)
                    {
                        characters[i] = CharacterContext.GetOrCreate(characterBinding.Character);
                    }
                }
            }
            colliderArrayPool.Return(hits);
            return characters;
        }

        public static CharacterContext RayCast(CastContext context, Vector3Context origin, Vector3Context direction, float distance, int layerMask)
        {
            if(Physics.Raycast(origin.Vector, direction.Vector, out var hit, distance, layerMask, QueryTriggerInteraction.Ignore))
            {
                if(hit.collider.TryGetComponent<CharacterBinding>(out var characterBinding))
                {
                    if(characterBinding.Character.Faction != context.GetOwner().Faction)
                    {
                        return CharacterContext.GetOrCreate(characterBinding.Character);
                    }
                }
            }
            return null;
        }

    }
}