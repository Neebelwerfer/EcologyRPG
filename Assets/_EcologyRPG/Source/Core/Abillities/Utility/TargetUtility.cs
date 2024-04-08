using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
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
            if (colliderHits[i].TryGetComponent<CharacterBinding>(out var binding))
            {
                var character = binding.Character;
                targets[i] = character;
            }
        }
        return targets;
    }

    public static BaseCharacter[] GetTargetsInRadius(Vector3 origin, float radius, LayerMask mask)
    {
        var numHits = Physics.OverlapSphereNonAlloc(origin, radius, colliderHits, mask);
        BaseCharacter[] targets = new BaseCharacter[numHits];

        for (int i = 0; i < numHits; i++)
        {
            if (colliderHits[i].TryGetComponent<CharacterBinding>(out var binding))
            {
                var character = binding.Character;
                targets[i] = character;
            }
        }
        return targets;
    }

    public static BaseCharacter[] GetTargetsInCone(Vector3 origin, Vector3 forward, float angle, float radius, LayerMask mask)
    {
        var numhits = Physics.OverlapSphereNonAlloc(origin, radius, colliderHits, mask);
        BaseCharacter[] targets = new BaseCharacter[numhits];

        for (int i = 0; i < numhits; i++)
        {
            if (Vector3.Angle(forward, colliderHits[i].transform.position - origin) < angle)
            {
                if (colliderHits[i].TryGetComponent<CharacterBinding>(out var binding))
                {
                    var character = binding.Character;
                    targets[i] = character;
                }
            }
        }
        return targets;
    }

    public static Vector3 GetMouseDirection(Vector3 origin, Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, AbilityManager.GroundMask))
        {
            return (hit.point - origin).normalized;
        }
        return Vector3.zero;
    }

    public static Vector3 GetDirection(Vector3 origin, Vector3 target)
    {
        return (target - origin).normalized;
    }

    public static Vector3 GetMousePoint(Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, AbilityManager.GroundMask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
