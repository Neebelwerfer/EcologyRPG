using EcologyRPG._Core.Character;
using log4net.Util;
using System;
using UnityEngine;

namespace EcologyRPG._Core.Abilities
{
    [RequireComponent(typeof(Collider))]
    public class CurvedProjectileBehaviour : ProjectileBehaviour
    {
        public Vector3 target;
        public float time;
        public float angle;
        public LayerMask IgnoreMask;
        public BaseCharacter owner;

        public Action<GameObject> OnGroundHit;
        Rigidbody rb;
        Collider col;

        public CurvedProjectileBehaviour(GameObject prefab, Vector3 origin, Vector3 target, float time, float angle, LayerMask ignoreMask, BaseCharacter owner, Action<GameObject> OnGroundHit) : base(prefab, origin, Quaternion.LookRotation((target - origin).normalized))
        {
            this.target = target;
            this.time = time;
            this.angle = angle;
            this.IgnoreMask = ignoreMask;
            this.owner = owner;
            this.OnGroundHit = OnGroundHit;

            rb = projectile.Rigidbody;
            rb.isKinematic = false;
            rb.mass = 0;
            rb.drag = 0;
            col = projectile.Collider;
            col.isTrigger = false;
            col.excludeLayers = IgnoreMask;
            projectile.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane((target - origin).normalized, Vector3.up));
            projectileObj.transform.Rotate(angle, 0, 0);
            Debug.DrawRay(projectileObj.transform.position, projectileObj.transform.forward, Color.red, 5);
            rb.velocity = CalculateInitialVelocity(Vector3.Distance(projectileObj.transform.position, target), time, Mathf.Abs(angle)) * projectileObj.transform.forward;
        }

        float CalculateInitialVelocity(float distance, float time, float angle)
        {
            angle *= Mathf.Deg2Rad;
            return distance / (time * Mathf.Cos(angle));
        }

        public override void OnUpdate()
        {

        }


        public override void OnCollisionEnter(Collision collision)
        {
            OnGroundHit?.Invoke(projectileObj);
            Stop();
            rb.isKinematic = true;
            col.isTrigger = true;
            col.excludeLayers ^= IgnoreMask;
        }
    }
}
