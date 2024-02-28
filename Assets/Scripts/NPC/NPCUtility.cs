
using UnityEngine;

public static class NPCUtility
{
    public static Vector3 GetRandomPointInRadius(Vector3 center, float radius)
    {
        Vector3 point = Vector3.zero;
        Vector3 randomPoint = center + Random.insideUnitSphere * radius;
        randomPoint.y = 1000;
        while (point == Vector3.zero)
        {
            if (Physics.BoxCast(randomPoint, Vector3.one * 0.5f, Vector3.down, out var hit, Quaternion.identity, 1001, LayerMask.GetMask("Ground")))
            {
                point = hit.point;
            }
            else
            {
                randomPoint = center + Random.insideUnitSphere * radius;
                randomPoint.y = 1000;
            }
        }
        return randomPoint;
    }
}