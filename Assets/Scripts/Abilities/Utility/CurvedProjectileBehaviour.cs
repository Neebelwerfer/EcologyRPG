using Character;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CurvedProjectileBehaviour : MonoBehaviour
{
    public Vector3 target;
    public float time;
    public float angle;
    public LayerMask IgnoreMask;
    public BaseCharacter owner;

    public Action<GameObject> OnGroundHit;

    Rigidbody rb;
    Collider col;
    private void Start()
    {
        if(TryGetComponent<Rigidbody>(out var body))
        {
            rb = body;
        }
        else
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.mass = 0;
        rb.drag = 0.5f;
        col = GetComponent<Collider>();
        gameObject.layer = LayerMask.NameToLayer("Projectile");
        col.isTrigger = false;
        col.excludeLayers = IgnoreMask;
        transform.rotation = Quaternion.LookRotation(target - transform.position);
        transform.Rotate(angle, 0, 0);
        rb.velocity = CalculateInitialVelocity(Vector3.Distance(owner.Position, target), time, angle) * transform.forward;
    }

    float CalculateInitialVelocity(float distance, float time, float angle)
    {
        return distance / (time * Mathf.Cos(angle));
    }


    private void OnCollisionEnter(Collision collision)
    {
        OnGroundHit?.Invoke(gameObject);
        Destroy(gameObject);
    }
}
