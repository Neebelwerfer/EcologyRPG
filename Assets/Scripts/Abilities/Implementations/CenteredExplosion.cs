using Character;
using Character.Abilities;
using Character.Abilities.AbilityEffects;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class CenteredExplosion : BaseAbility
{
    [Header("Centered Explosion")]
    [Tooltip("The layer mask of the targets that will be hit by the explosion")]
    public LayerMask targetMask;
    [Tooltip("The radius of the explosion")]
    public float Radius;
    [Tooltip("Debuffs that will be applied to the targets when the explosion hits")]
    public List<AbilityEffect> OnHitEffects = new();

    public List<AbilityEffect> OnCastEffects = new();

    BaseCharacter[] targets;

    public override void Cast(CastInfo caster)
    {
        targets = TargetUtility.GetTargetsInRadius(caster.castPos, Radius, targetMask);
        Debug.Log("Casting explosion!");
        Debug.Log("Targets: " + targets.Length);
        Debug.Log("Target Mask: " + targetMask.value);
        Debug.Log("Radius: " + Radius);

        foreach (var effect in OnCastEffects)
        {
            effect.ApplyEffect(caster, null);
        }
        
        if (targets != null && targets.Length > 0)
        {
            foreach (var t in targets)
            {
                if (t == null) continue;
                if (t.Faction == caster.owner.Faction) continue;


                foreach (var effect in OnHitEffects)
                {
                    effect.ApplyEffect(caster, t);
                }
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var effect in OnHitEffects)
        {
            DestroyImmediate(effect, true);
        }
        foreach (var effect in OnCastEffects)
        {
            DestroyImmediate(effect, true);
        }
    
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CenteredExplosion))]
public class CenteredExplosionEditor : BaseAbilityDefinitionEditor
{
    public override void OnInspectorGUI()
    {
        CenteredExplosion ability = (CenteredExplosion)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("targetMask"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Radius"));

        AbilityEffectEditor.Display("On Cast Effects", ability.OnCastEffects, ability, DisplayEffectType.Visual);
        AbilityEffectEditor.Display("On Hit Effects", ability.OnHitEffects, ability, DisplayEffectType.All);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif