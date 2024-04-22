using EcologyRPG.AbilityScripting;
using EcologyRPG.Core.Character;
using System;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    [RequireComponent(typeof(Collider))]
    public class CurvedProjectileBehaviour : ProjectileBehaviour
    {
        public Vector3 target;
        public float time;
        public float angle;
        public BaseCharacter owner;

        public Action OnGroundHit;
        readonly Rigidbody rb;
        readonly Collider col;
        readonly float initialVelocity;

        float timeElapsed = 0;
        public CurvedProjectileBehaviour(int prefabID, Vector3 origin, Vector3 target, BaseCharacter owner, float time, float angle) : base(prefabID, origin, Quaternion.LookRotation((target - origin).normalized))
        {
            this.target = target;
            this.time = time;
            this.angle = angle;
            this.owner = owner;

            rb = projectile.Rigidbody;
            rb.isKinematic = false;
            rb.mass = 0;
            rb.drag = 0;
            rb.useGravity = false;
            rb.angularDrag = 0;
            col = projectile.Collider;
            col.isTrigger = false;
            col.excludeLayers = AbilityManager.CurvedProjectileIgnoreMask;

            projectileObj.transform.Rotate(angle, 0, 0);
            Debug.DrawRay(origin, projectileObj.transform.forward * 10, Color.red, 10);
            initialVelocity = CalculateInitialVelocity(Vector3.Distance(origin, target), time, Mathf.Abs(angle));
            Debug.Log($"Initial Velocity: {initialVelocity}");
            rb.velocity = initialVelocity * projectileObj.transform.forward;
        }

        float CalculateInitialVelocity(float distance, float time, float angle)
        {
            angle *= Mathf.Deg2Rad;
            return distance / (time * Mathf.Cos(angle));
        }

        public override void OnUpdate()
        {
            var AngleRad = Mathf.Abs(angle) * Mathf.Deg2Rad;
            var x = initialVelocity * Mathf.Cos(AngleRad);
            var y = initialVelocity * Mathf.Sin(AngleRad) + Physics.gravity.y * timeElapsed;
            rb.velocity += (x * Vector3.ProjectOnPlane(projectileObj.transform.forward, Vector3.up).normalized + y * Vector3.up) * Time.deltaTime;
            timeElapsed += Time.fixedDeltaTime;
        }

        public override void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Hit {collision.collider.name}");
            OnGroundHit?.Invoke();
            Stop();
            rb.isKinematic = true;
            col.isTrigger = true;
            col.excludeLayers ^= AbilityManager.CurvedProjectileIgnoreMask;
        }
    }
}
