using EcologyRPG.AbilityScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace EcologyRPG.Core.Abilities
{
    public abstract class ProjectileBehaviour
    {
        protected GameObject projectileObj;
        protected Projectile projectile;

        protected int prefabID;
        protected Vector3 origin;
        protected Quaternion rotation;

        public ProjectileBehaviour(int prefabID, Vector3 origin, Quaternion rotation)
        {
            this.prefabID = prefabID;
            this.origin = origin;
            this.rotation = rotation;
            projectile = ProjectileDatabase.Instance.GetProjectile(prefabID);
            projectileObj = projectile.gameObject;
        }

        public void Fire()
        {
            projectileObj.transform.SetPositionAndRotation(origin, rotation);
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
            ProjectileDatabase.Instance.ReturnProjectile(prefabID, projectile);
            ProjectileSystem.Instance.RemoveBehaviour(this);
        }
    }
}