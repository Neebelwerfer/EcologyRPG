using UnityEngine;

namespace EcologyRPG._Core.Abilities
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        public Collider Collider;
        public Rigidbody Rigidbody;
        ProjectileBehaviour behaviour;

        public virtual void Init(ProjectileBehaviour behaviour)
        {
            this.behaviour = behaviour;
            Collider = GetComponent<Collider>();
            Rigidbody = GetComponent<Rigidbody>();
            Collider.isTrigger = true;
            Rigidbody.isKinematic = true;
            gameObject.layer = LayerMask.NameToLayer("Projectile");
        }

        protected void Update()
        {
            behaviour?.OnUpdate();
        }

        private void OnTriggerEnter(Collider other)
        {
            behaviour?.OnTriggerEnter(other);
        }

        private void OnCollisionEnter(Collision collision)
        {
            behaviour?.OnCollisionEnter(collision);
        }

        public virtual void Stop()
        {
            behaviour = null;
            Collider.isTrigger = true;
            Rigidbody.isKinematic = true;
        }
    }
}