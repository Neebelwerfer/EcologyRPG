using Character;
using Character.Abilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MultipleProjectiles : ProjectileAbility
{
    public ProjectileType type;
    [Min(2)]
    public int numberOfProjectiles = 2;
    [Header("Cone Settings")]
    public float ConeAngle;
    [Header("Line Settings")]
    public float LineWidth;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float Speed;

    Vector3 dir;
    float angleBetweenProjectiles;

    public override void Cast(CastInfo caster)
    {
        var dir = GetDir(caster);
        if (type == ProjectileType.Cone)
        {
            angleBetweenProjectiles = ConeAngle / (numberOfProjectiles - 1);
        }
        else if (type == ProjectileType.Line)
        {
            angleBetweenProjectiles = LineWidth / numberOfProjectiles;
        }
        else if (type == ProjectileType.Circular)
        {
            angleBetweenProjectiles = 360 / numberOfProjectiles;
        }

        if (type == ProjectileType.Line)
        {
            var center = caster.castPos;
            var left = Quaternion.Euler(0, -90, 0) * dir;
            var start = center + left * LineWidth / 2;
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                var pos = start + i * LineWidth * -left / (numberOfProjectiles - 1);
                ProjectileUtility.CreateProjectile(projectilePrefab, pos, pos + dir * Range, Speed, destroyOnHit, targetMask, caster.owner, (target) =>
                {
                    OnHit(caster, target, dir);
                });
            }
        }
        else if (type == ProjectileType.Cone)
        {
            var start = Quaternion.Euler(0, -ConeAngle / 2, 0) * dir;
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                var newDir = Quaternion.Euler(0, angleBetweenProjectiles * i, 0) * start;
                ProjectileUtility.CreateProjectile(projectilePrefab, caster.castPos, caster.castPos + newDir * Range, Speed, destroyOnHit, targetMask, caster.owner, (target) =>
                {
                    OnHit(caster, target, newDir);
                });
            }
        }
        else if (type == ProjectileType.Circular)
        {
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                var newDir = Quaternion.Euler(0, angleBetweenProjectiles * i, 0) * dir;
                ProjectileUtility.CreateProjectile(projectilePrefab, caster.castPos, caster.castPos + newDir * Range, Speed, destroyOnHit, targetMask, caster.owner, (target) =>
                {
                    OnHit(caster, target, newDir);
                });
            }
        }
    }

    void OnHit (CastInfo caster, BaseCharacter target, Vector3 direction)
    {
        var newCastInfo = new CastInfo { owner = caster.owner, castPos = target.transform.position, dir = direction, mousePoint = caster.mousePoint };
        foreach (var effect in OnHitEffects)
        {
            effect.ApplyEffect(newCastInfo, target);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MultipleProjectiles))]
public class MultipleProjectilesEditor : ProjectileAbilityEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MultipleProjectiles ability = (MultipleProjectiles)target;
        EditorGUILayout.LabelField("Multiple Projectiles Settings", EditorStyles.boldLabel);
        ability.type = (ProjectileType)EditorGUILayout.EnumPopup("Type", ability.type);
        ability.numberOfProjectiles = EditorGUILayout.IntField("Number of Projectiles", ability.numberOfProjectiles);
        if (ability.type == ProjectileType.Cone)
        {
            ability.ConeAngle = EditorGUILayout.FloatField("Cone Angle", ability.ConeAngle);
        }
        else if (ability.type == ProjectileType.Line)
        {
            ability.LineWidth = EditorGUILayout.FloatField("Line Width", ability.LineWidth);
        }
        ability.projectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", ability.projectilePrefab, typeof(GameObject), false);
        ability.Speed = EditorGUILayout.FloatField("Speed", ability.Speed);

    }
}
#endif