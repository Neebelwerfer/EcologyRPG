using Character;
using System;
using UnityEngine;
using Character.Abilities;

[RequireComponent(typeof(Collider))]
public class ProjectileBehaviour : MonoBehaviour
{
    public Vector3[] path;
    public float speed;
    public float damage;
    public DamageType damageType;
    public bool DestroyOnCollision;
    public LayerMask layerMask;
    public BaseCharacter owner;

    public Action<BaseCharacter> OnHit;

    int counter = 0;

    public void Start()
    {
        if (path.Length == 0)
        {
            Destroy(gameObject);
        }

        GetComponent<Collider>().isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Projectile");
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
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != owner.gameObject && other.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            if (other.gameObject.TryGetComponent<BaseCharacter>(out var character))
            {
                OnHit(character);
            }

            if (DestroyOnCollision)
            {
                Destroy(gameObject);
            }
        }
    }
}
