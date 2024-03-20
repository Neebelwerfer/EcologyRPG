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
}

#if UNITY_EDITOR
[CustomEditor(typeof(ProjectileAbility), false)]
public class ProjectileAbilityEditor : AttackAbilityEditor
{
    bool test = true;
    int index = 0;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ProjectileAbility ability = (ProjectileAbility)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("destroyOnHit"));
    }
}
#endif