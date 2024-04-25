using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class Projectile : MonoBehaviour
    {
        public Collider Collider;
        public Rigidbody Rigidbody;
        public AudioSource AudioSource;
        ProjectileBehaviour behaviour;

        public virtual void Init(ProjectileBehaviour behaviour)
        {
            this.behaviour = behaviour;
            Collider = GetComponent<Collider>();
            Rigidbody = GetComponent<Rigidbody>();
            AudioSource = GetComponent<AudioSource>();
            Collider.isTrigger = true;
            Rigidbody.isKinematic = true;
            gameObject.layer = LayerMask.NameToLayer("Projectile");

            AudioSource.loop = true;
        }

        private void OnEnable()
        {
            if(AudioSource.clip != null)
            {
                AudioSource.Play();
            }
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
            AudioSource.Stop();
            AudioSource.clip = null;
        }
    }
}