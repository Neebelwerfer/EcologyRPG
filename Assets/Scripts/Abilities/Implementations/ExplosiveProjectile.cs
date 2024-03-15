using Character.Abilities;
using Codice.Client.Commands;
using Codice.CM.Common;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ExplosiveProjectile", fileName = "New Explosive Projectile")]
public class ExplosiveProjectile : ProjectileAbility
{
    public float Speed;

    [Header("Explosive Projectile")]
    [Tooltip("The radius of the explosion")]
    public float ExplosionRadius;
    [Tooltip("The base damage of the explosion")]
    public float ExplosionDamage;
    [Tooltip("Extra damage that will be dealt to the target that was hit by the projectile")]
    public float TargetHitExtraDamage;
    [Tooltip("The type of damage the explosion will deal")]
    public DamageType damageType;
    [Tooltip("Debuffs that will be applied to the targets when the explosion hits")]
    public List<DebuffEffect> effects;
    [Tooltip("The prefab of the projectile")]
    public GameObject ProjectilePrefab;

    public override void Cast(CastInfo caster)
    {
        var dir = GetDir(caster);
        ProjectileUtility.CreateProjectile(ProjectilePrefab, caster.owner.CastPos + (dir * Range), Speed, destroyOnHit, targetMask, caster.owner, (target) =>
        {
            var targets = TargetUtility.GetTargetsInRadius(target.transform.position, ExplosionRadius, targetMask);

            foreach (var t in targets)
            {

                if (t.Faction != caster.owner.Faction)
                {
                    t.ApplyDamage(CalculateDamage(caster.owner, damageType, t == target ? ExplosionDamage + TargetHitExtraDamage : ExplosionDamage));
                }

                foreach (var effect in effects)
                {
                    ApplyEffect(caster, t, effect);
                }
            }
        });
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ExplosiveProjectile))]
public class ExplosiveProjectileEditor : ProjectileAbilityEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ExplosiveProjectile ability = (ExplosiveProjectile)target;
        ability.Speed = EditorGUILayout.FloatField("Speed", ability.Speed);
        ability.ExplosionRadius = EditorGUILayout.FloatField("Explosion Radius", ability.ExplosionRadius);
        ability.ExplosionDamage = EditorGUILayout.FloatField("Explosion Damage", ability.ExplosionDamage);
        ability.TargetHitExtraDamage = EditorGUILayout.FloatField("Target Hit Extra Damage", ability.TargetHitExtraDamage);
        ability.damageType = (DamageType)EditorGUILayout.EnumPopup("Damage Type", ability.damageType);
        ability.ProjectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", ability.ProjectilePrefab, typeof(GameObject), false);
        if (EditorGUILayout.PropertyField(serializedObject.FindProperty("effects")))
        {
            EditorUtility.SetDirty(ability);
        }
    }
}
#endif
