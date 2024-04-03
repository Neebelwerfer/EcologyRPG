using EcologyRPG._Core.Character;
using System;
using UnityEngine;

namespace EcologyRPG._Core.Abilities
{
    public class BasicProjectileBehaviour : ProjectileBehaviour
    {
        public Vector3 dir;
        public float speed;
        public bool DestroyOnCollision;
        public LayerMask layerMask;
        public BaseCharacter owner;

        public Action<BaseCharacter> OnHit;
        public Action<GameObject> OnUpdating;

        float range;
        Vector3 localScale;
        Quaternion rotation;

        public BasicProjectileBehaviour(GameObject Prefab, float range, Vector3 origin, Vector3 dir, float speed, bool DestroyOnHit, LayerMask layerMask, BaseCharacter owner, Action<BaseCharacter> OnHit, Action<GameObject> OnUpdate)
            : base(Prefab, origin, Quaternion.LookRotation(dir))
        {
            this.range = range;
            this.dir = dir;
            this.speed = speed;
            this.DestroyOnCollision = DestroyOnHit;
            this.layerMask = layerMask;
            this.owner = owner;
            this.OnHit = OnHit;
            this.OnUpdating = OnUpdate;

            localScale = projectileObj.transform.localScale;
            rotation = projectileObj.transform.rotation;
        }

        public override void OnUpdate()
        {
            OnUpdating?.Invoke(projectileObj);
            Vector3 normal = Vector3.up;
            if(Physics.Raycast(projectileObj.transform.position, Vector3.down, out var hit, LayerMask.NameToLayer("Ground")))
            {
                normal = hit.normal;
            }
            var travelDir = Vector3.ProjectOnPlane(dir, normal).normalized;
            projectileObj.transform.rotation = Quaternion.LookRotation(travelDir);
            var travel = speed * Time.deltaTime * travelDir;
            projectileObj.transform.position += travel;
            range -= travel.magnitude;
            if (range <= 0)
            {
                Stop();
            }
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != owner.GameObject && other.gameObject.layer == LayerMask.NameToLayer("Entity"))
            {
                if (other.gameObject.TryGetComponent<CharacterBinding>(out var binding))
                {
                    BaseCharacter character = binding.Character;
                    if (character.Faction == owner.Faction) return;
                    OnHit(character);
                }

                if (DestroyOnCollision)
                {
                    Stop();
                }
            }
        }

        protected override void Stop()
        {
            projectileObj.transform.localScale = localScale;
            projectileObj.transform.rotation = rotation;
            base.Stop();
        }
    }
}