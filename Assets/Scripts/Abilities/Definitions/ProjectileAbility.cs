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

        foldOut = EditorGUILayout.BeginFoldoutHeaderGroup(foldOut, "On Hit Effects");
        if (foldOut)
        {
            foreach (var effect in ability.OnHitEffects)
            {
                EditorGUILayout.ObjectField(effect, typeof(AbilityEffect), false);
            }
            if (GUILayout.Button("Add Effect"))
            {
                var window = EditorWindow.GetWindow<AbilityEffectEditor>();
                window.editedEffects = new EditListEffect(ability, ability.OnHitEffects);
                window.Show();
            }

            if (ability.OnHitEffects.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Remove All"))
                {
                    foreach (var effect in ability.OnHitEffects)
                    {
                        DestroyImmediate(effect, true);
                    }
                    ability.OnHitEffects.Clear();
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }

                if (GUILayout.Button("Remove Last"))
                {
                    DestroyImmediate(ability.OnHitEffects[ability.OnHitEffects.Count - 1], true);
                    ability.OnHitEffects.RemoveAt(ability.OnHitEffects.Count - 1);
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                var ind = EditorGUILayout.IntField("Index", index);
                if (ind > ability.OnHitEffects.Count)
                {
                    ind = ability.OnHitEffects.Count;
                }
                index = ind;
                if (GUILayout.Button("Remove Index"))
                {
                    DestroyImmediate(ability.OnHitEffects[index], true);
                    ability.OnHitEffects.RemoveAt(index);
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}
#endif