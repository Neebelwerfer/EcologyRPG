using UnityEngine;

namespace EcologyRPG._Core.Abilities
{
    public abstract class ProjectileBehaviour
    {
        protected GameObject projectileObj;
        protected Projectile projectile;

        public ProjectileBehaviour(GameObject prefab, Vector3 origin, Quaternion rotation)
        {
            projectileObj = ProjectilePoolHandler.Instance.GetProjectile(prefab, origin, Quaternion.identity);
            if(projectileObj.TryGetComponent<Projectile>(out var proj))
            {
                projectile = proj;
            }
            else
            {
                projectile = projectileObj.AddComponent<Projectile>();

            }
            projectile.Init(this);
            ProjectileSystem.Instance.AddBehaviour(this);
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