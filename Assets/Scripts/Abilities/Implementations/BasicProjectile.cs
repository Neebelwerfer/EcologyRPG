using Character.Abilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BasicProjectile")]
public class BasicProjectile : ProjectileAbility
{
    [Tooltip("The base damage of the projectile")]
    public float BaseDamage;
    [Tooltip("The type of damage the projectile will deal")]
    public DamageType damageType;
    [Tooltip("The travel speed of the projectile")]
    public float Speed;

    [Header("Projectile Settings")]
    [Tooltip("The prefab of the projectile")]
    public GameObject ProjectilePrefab;
    [Tooltip("Debuffs that will be applied to the target when the projectile hits")]    
    public List<DebuffEffect> Effects;

    public override void Cast(CastInfo castInfo)
    {
        var dir = GetDir(castInfo);
        ProjectileUtility.CreateProjectile(ProjectilePrefab, castInfo.owner.CastPos + (dir * Range), Speed, destroyOnHit, targetMask, castInfo.owner, (target) =>
        {
            target.ApplyDamage(CalculateDamage(castInfo.owner, damageType, BaseDamage));
            foreach (var effect in Effects)
            {
                ApplyEffect(castInfo, target, effect);
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
        ability.BaseDamage = EditorGUILayout.FloatField("Base Damage", ability.BaseDamage);
        ability.damageType = (DamageType)EditorGUILayout.EnumPopup("Damage Type", ability.damageType);
        ability.Speed = EditorGUILayout.FloatField("Speed", ability.Speed);
        ability.ProjectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", ability.ProjectilePrefab, typeof(GameObject), false);
        if (EditorGUILayout.PropertyField(serializedObject.FindProperty("Effects")))
        {
            EditorUtility.SetDirty(ability);
        }
    }
}
#endif