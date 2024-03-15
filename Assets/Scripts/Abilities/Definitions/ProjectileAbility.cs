using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ProjectileType
{
    Line,
    Cone,
    Circular
}

public abstract class ProjectileAbility : AttackAbilityEffect
{
    [Header("Projectile Ability")]
    [Tooltip("The projectile will be destroyed when it hits the first target")]
    public bool destroyOnHit = true;

    [Tooltip("The ability that will be cast when the projectile hits the ground")]
    public AttackAbility OnHitAbility;
}

#if UNITY_EDITOR
[CustomEditor(typeof(ProjectileAbility), false)]
public class ProjectileAbilityEditor : AttackAbilityEffectEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ProjectileAbility ability = (ProjectileAbility)target;
        ability.destroyOnHit = EditorGUILayout.Toggle("Destroy On Hit", ability.destroyOnHit);

        if (ability.OnHitAbility == null)
        {
            if (GUILayout.Button("Add On Hit Ability"))
            {
                ability.OnHitAbility = CreateInstance<AttackAbility>();
                ability.OnHitAbility.name = "On Hit Ability";
                AssetDatabase.AddObjectToAsset(ability.OnHitAbility, ability);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(ability);
            }
        }
        else
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnHitAbility"));
            if (GUILayout.Button("Remove On Hit Ability"))
            {
                DestroyImmediate(ability.OnHitAbility, true);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(ability);
                ability.OnHitAbility = null;
            }
        }
    }
}
#endif