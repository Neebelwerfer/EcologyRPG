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

    public List<VisualAbilityEffect> OnCastEffects = new();

    BaseCharacter[] targets;

    public override void Cast(CastInfo caster)
    {
        targets = TargetUtility.GetTargetsInRadius(caster.castPos, Radius, targetMask);
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
}

#if UNITY_EDITOR
[CustomEditor(typeof(CenteredExplosion))]
public class CenteredExplosionEditor : BaseAbilityEditor
{
    int index = 0;
    bool foldOut;

    public override void OnInspectorGUI()
    {
        CenteredExplosion ability = (CenteredExplosion)target;
        ability.targetMask = EditorGUILayout.LayerField("Target Mask", ability.targetMask);
        ability.Radius = EditorGUILayout.FloatField("Radius", ability.Radius);

        AbilityEffectEditor.Display(ref foldOut, ref index, ability.OnHitEffects, ability);
    }
}
#endif