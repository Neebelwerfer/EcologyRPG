using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    public abstract class ProjectileBehaviour
    {
        protected GameObject projectileObj;
        protected Projectile projectile;
        
        AudioClip clip;


        public ProjectileBehaviour(GameObject prefab, Vector3 origin, Quaternion rotation, AudioClip clip)
        {
            projectileObj = ProjectilePoolHandler.Instance.GetProjectile(prefab, origin, rotation);
            if (projectileObj.TryGetComponent<Projectile>(out var proj))
            {
                projectile = proj;
            }
            else
            {
                projectile = projectileObj.AddComponent<Projectile>();

            }
            projectile.Init(this);
            ProjectileSystem.Instance.AddBehaviour(this);
            this.clip = clip;

            if(clip != null)
            {
                projectile.AudioSource.clip = clip;
                projectile.AudioSource.Play();
            }
        }

        public abstract void OnUpdate();

        public virtual void OnTriggerEnter(Collider other)
        {

        }

        public virtual void OnCollisionEnter(Collision collision)
        {

        }

        protected virtual void Stop()
        {
            projectile.Stop();
            ProjectilePoolHandler.Instance.ReturnProjectile(projectileObj);
            ProjectileSystem.Instance.RemoveBehaviour(this);
        }
    }
}