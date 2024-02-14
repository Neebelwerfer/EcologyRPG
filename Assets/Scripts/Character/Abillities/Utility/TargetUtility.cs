using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class TargetUtility
{
    static readonly RaycastHit[] rayCastHits = new RaycastHit[5];
    static readonly Collider[] colliderHits = new Collider[5];

    public static BaseCharacter[] GetTargetsInLine(Vector3 origin, Vector3 direction, Vector3 halfExtents, LayerMask mask)
    {
        var numHits = Physics.OverlapBoxNonAlloc(origin + (direction * halfExtents.z), halfExtents, colliderHits, Quaternion.LookRotation(direction, Vector3.up), mask);
        BaseCharacter[] targets = new BaseCharacter[numHits];


        for (int i = 0; i < numHits; i++)
        {
            if (colliderHits[i].TryGetComponent<BaseCharacter>(out var character))
            {
                targets[i] = character;
            }
        }
        return targets;
    }

    public static Vector3 GetMouseDirection(Vector3 origin, Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, LayerMask.GetMask("Default")))
        {
            return (hit.point - origin).normalized;
        }
        return Vector3.zero;
    }

    public static Vector3 GetMousePoint(Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, LayerMask.GetMask("Default")))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
