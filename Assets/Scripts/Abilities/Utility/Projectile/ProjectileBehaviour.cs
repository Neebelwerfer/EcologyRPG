using Character;
using System;
using UnityEngine;
using Character.Abilities;

[RequireComponent(typeof(Collider))]
public class ProjectileBehaviour : MonoBehaviour
{
    public Vector3[] path;
    public float speed;
    public bool DestroyOnCollision;
    public LayerMask layerMask;
    public BaseCharacter owner;

    public Action<BaseCharacter> OnHit;

    int counter = 0;

    public void Init(Vector3[] path, float speed, bool DestroyOnHit, LayerMask layerMask, BaseCharacter owner, Action<BaseCharacter> OnHit)
    {
        this.path = path;
        this.speed = speed;
        this.DestroyOnCollision = DestroyOnHit;
        this.layerMask = layerMask;
        this.owner = owner;
        this.OnHit = OnHit;

        if (path.Length == 0)
        {
            Stop();
        }

        GetComponent<Collider>().isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Projectile");
        counter = 0;
    }

    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, path[counter], speed * Time.deltaTime);

        if(transform.position == path[counter])
        {
            counter++;
        }

        if(counter == path.Length)
        {
            Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != owner.gameObject && other.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            if (other.gameObject.TryGetComponent<BaseCharacter>(out var character))
            {
                if(character.Faction == owner.Faction) return;
                OnHit(character);
            }

            if (DestroyOnCollision)
            {
                Stop();
            }
        }
    }

    void Stop()
    {
        ProjectilePoolHandler.Instance.ReturnProjectile(gameObject);
        Destroy(this);
    }
}
