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
            projectile.Init(this);
            projectileObj = projectile.gameObject;
            projectileObj.transform.SetPositionAndRotation(origin, rotation);
        }

        public void Fire()
        {
            ProjectileSystem.Instance.AddBehaviour(this);
            projectileObj.SetActive(true);
        }


        public abstract void OnUpdate();

        public virtual void OnTriggerEnter(Collider other)
        {

        }

        public virtual void OnCollisionEnter(Collision collision)
        {

        }

        public Vector3 GetPosition()
        {
            return projectileObj.transform.position;
        }

        protected virtual void Stop()
        {
            projectile.Stop();
            ProjectileDatabase.Instance.ReturnProjectile(prefabID, projectile);
            ProjectileSystem.Instance.RemoveBehaviour(this);
        }
    }
}