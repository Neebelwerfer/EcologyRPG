using Character.Abilities.AbilityEffects;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ProjectileType
{
    Line,
    Cone,
    Circular
}

public abstract class ProjectileAbility : AttackAbility
{
    [Header("Projectile Ability")]
    [Tooltip("The projectile will be destroyed when it hits the first target")]
    public bool destroyOnHit = true;

    public List<AbilityEffect> OnHitEffects = new();
}

#if UNITY_EDITOR
[CustomEditor(typeof(ProjectileAbility), false)]
public class ProjectileAbilityEditor : AttackAbilityEditor
{
    bool foldOut = false;
    int index = 0;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ProjectileAbility ability = (ProjectileAbility)target;
        ability.destroyOnHit = EditorGUILayout.Toggle("Destroy On Hit", ability.destroyOnHit);

        AbilityEffectEditor.Display(ref foldOut, ref index, ability.OnHitEffects, ability);

    }
}
#endif