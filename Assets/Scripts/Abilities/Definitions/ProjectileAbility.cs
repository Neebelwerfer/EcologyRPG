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

    private void OnDestroy()
    {
        foreach (var effect in OnHitEffects)
        {
            DestroyImmediate(effect, true);
        }
    
    }
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
        ability.destroyOnHit = EditorGUILayout.Toggle("Destroy On Hit", ability.destroyOnHit);

        AbilityEffectEditor.Display("On Hit Effects", ability.OnHitEffects, ability, DisplayEffectType.All);
    }
}
#endif