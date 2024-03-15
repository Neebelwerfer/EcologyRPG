using Character.Abilities;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityHolder/AttackAbility", fileName = "New Attack Ability")]
public class AttackAbility : BaseAbility
{
    public AbilityEffect Ability;

    Vector3 MousePoint;

    public override void CastStarted(CastInfo caster)
    {
        base.CastStarted(caster);
        MousePoint = TargetUtility.GetMousePoint(Camera.main);
    }

    public override void CastEnded(CastInfo caster)
    {
        base.CastEnded(caster);
        caster.mousePoint = MousePoint;
        if(caster.castPos == Vector3.zero) caster.castPos = caster.owner.CastPos;
        Ability.Cast(caster);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AttackAbility))]
public class AttackAbilityEditor : BaseAbilityEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AttackAbility ability = (AttackAbility)target;

        if(ability.Ability == null)
        {
            var res = (AbilityEffect) EditorGUILayout.ObjectField(new GUIContent("Ability"), ability.Ability, typeof(AbilityEffect), false);
            if (res != null)
            {
                Debug.Log("Creating new ability");
                ability.Ability = Instantiate(res);
                AssetDatabase.AddObjectToAsset(ability.Ability, ability);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(ability);
            }
        }
        else
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Ability"));
        }


        if (ability.Ability != null)
        {
               
            if(GUILayout.Button("Remove Ability"))
            {
                DestroyImmediate(ability.Ability, true);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(ability);
                ability.Ability = null;
            }
        }
    }
}
#endif