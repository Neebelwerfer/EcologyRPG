using Character.Abilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BasicProjectile : ProjectileAbility
{
    [Tooltip("The travel speed of the projectile")]
    public float Speed;

    [Header("Projectile Settings")]
    [Tooltip("The prefab of the projectile")]
    public GameObject ProjectilePrefab;


    public override void Cast(CastInfo castInfo)
    {
        var dir = GetDir(castInfo);
        ProjectileUtility.CreateProjectile(ProjectilePrefab, castInfo.owner.CastPos + (dir * Range), Speed, destroyOnHit, targetMask, castInfo.owner, (target) =>
        {
            var newCastInfo = new CastInfo { owner = castInfo.owner, castPos = target.transform.position, dir = dir, mousePoint = Vector3.zero };
            foreach (var effect in OnHitEffects)
            {
                effect.ApplyEffect(newCastInfo, target);
            }
        });
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BasicProjectile))]
public class BasicProjectileEditor : ProjectileAbilityEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BasicProjectile ability = (BasicProjectile)target;
        ability.Speed = EditorGUILayout.FloatField("Speed", ability.Speed);
        ability.ProjectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", ability.ProjectilePrefab, typeof(GameObject), false);
    }
}
#endif