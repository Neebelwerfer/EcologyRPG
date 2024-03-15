using Character.Abilities;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityHolder/NPCAbility", fileName = "New NPC Ability")]
public class NPCAbility : BaseAbility
{
    public AttackAbilityEffect attackAbilityEffect;

    public override void CastEnded(CastInfo caster)
    {
        base.CastEnded(caster);
        caster.castPos = caster.owner.CastPos;
        caster.dir = caster.owner.transform.forward;
        attackAbilityEffect.Cast(caster);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(NPCAbility))]
public class NPCAbilityEditor : BaseAbilityEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        NPCAbility ability = (NPCAbility)target;

        if(ability.attackAbilityEffect == null)
        {
            var res = (AttackAbilityEffect) EditorGUILayout.ObjectField(new GUIContent("Ability"), ability.attackAbilityEffect, typeof(AttackAbilityEffect), false);
            if (res != null)
            {
                ability.attackAbilityEffect = Instantiate(res);
                AssetDatabase.AddObjectToAsset(ability.attackAbilityEffect, ability);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(ability);
            }
        }
        else
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("attackAbilityEffect"));
        }
    }
}
#endif