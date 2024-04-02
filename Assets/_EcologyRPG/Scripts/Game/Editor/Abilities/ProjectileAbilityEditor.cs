using EcologyRPG._Core.Abilities;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(ProjectileAbility), false)]
public class ProjectileAbilityEditor : AttackAbilityEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ProjectileAbility ability = (ProjectileAbility)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("destroyOnHit"));
    }
}
#endif