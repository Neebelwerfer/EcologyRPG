using EcologyRPG.Core.Character;
using System;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    [RequireComponent(typeof(Collider))]
    public class CurvedProjectileBehaviour : MonoBehaviour
    {
        public Vector3 target;
        public float time;
        public float angle;
        public LayerMask IgnoreMask;
        public BaseCharacter owner;

        public Action<GameObject> OnGroundHit;

        Rigidbody rb;
        Collider col;

        public void Init(Vector3 target, float time, float angle, LayerMask ignoreMask, BaseCharacter owner, Action<GameObject> OnGroundHit)
        {
            this.target = target;
            this.time = time;
            this.angle = angle;
            this.IgnoreMask = ignoreMask;
            this.owner = owner;
            this.OnGroundHit = OnGroundHit;

            if (TryGetComponent<Rigidbody>(out var body))
            {
                rb = body;
            }
            else
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.mass = 0;
            rb.drag = 0f;
            col = GetComponent<Collider>();
            gameObject.layer = LayerMask.NameToLayer("Projectile");
            col.isTrigger = false;
            col.excludeLayers = IgnoreMask;
            transform.rotation = Quaternion.LookRotation(target - transform.position);
            transform.Rotate(angle, 0, 0);
            rb.velocity = CalculateInitialVelocity(Vector3.Distance(transform.position, target), time, Mathf.Abs(angle)) * transform.forward;
        }

        float CalculateInitialVelocity(float distance, float time, float angle)
        {
            angle *= Mathf.Deg2Rad;
            return distance / (time * Mathf.Cos(angle));
        }


        private void OnCollisionEnter(Collision collision)
        {
            OnGroundHit?.Invoke(gameObject);
            ProjectilePoolHandler.Instance.ReturnProjectile(gameObject);
            Destroy(this);
        }
    }
}
