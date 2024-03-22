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
    public Action<GameObject> OnUpdate;

    int counter = 0;
    Vector3 localScale;
    Quaternion rotation;

    public void Init(Vector3[] path, float speed, bool DestroyOnHit, LayerMask layerMask, BaseCharacter owner, Action<BaseCharacter> OnHit, Action<GameObject> OnUpdate)
    {
        this.path = path;
        this.speed = speed;
        this.DestroyOnCollision = DestroyOnHit;
        this.layerMask = layerMask;
        this.owner = owner;
        this.OnHit = OnHit;
        this.OnUpdate = OnUpdate;

        localScale = transform.localScale;
        rotation = transform.rotation;


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
        OnUpdate?.Invoke(gameObject);

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
        transform.localScale = localScale;
        transform.rotation = rotation;
        ProjectilePoolHandler.Instance.ReturnProjectile(gameObject);
        Destroy(this);
    }
}
