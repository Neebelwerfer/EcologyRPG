#if UNITY_EDITOR
using EcologyRPG.Core.Abilities;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SingleAbilityEditor : EditorWindow
{
    public static void ShowWindow(int abilityIndex, AbilityEditor editor)
    {
        var singleAbilityEditor = GetWindow<SingleAbilityEditor>("Single Ability Editor");
        singleAbilityEditor.abilityIndex = abilityIndex;
        singleAbilityEditor.editor = editor;

        if(AbilityEditor.abilityData == null)
        {
            AbilityEditor.Load();
        }
        singleAbilityEditor.abilityData = AbilityEditor.abilityData.data;
        singleAbilityEditor.serializedObject = new SerializedObject(singleAbilityEditor);
        singleAbilityEditor.SerializedAbility = singleAbilityEditor.serializedObject.FindProperty("abilityData").GetArrayElementAtIndex(abilityIndex);
        singleAbilityEditor.globalArray = singleAbilityEditor.SerializedAbility.FindPropertyRelative("_DefaultGlobalVariables");

    }

    AbilityEditor editor;
    [SerializeField]
    AbilityData[] abilityData;
    [SerializeField]
    int abilityIndex;
    GlobalVariableType selectedGlobalVariableType;
    bool[] foldouts;

    SerializedObject serializedObject;
    SerializedProperty SerializedAbility;
    SerializedProperty globalArray;

    private void OnGUI()
    {

        if (editor == null)
        {
            Close();
            return;
        }

        serializedObject.Update();
        var ability = abilityData[abilityIndex];
        if (GUILayout.Button("Edit Script Behaviour"))
        {
            if (ability.ScriptPath == null || ability.ScriptPath == "")
            {
                editor.CreateScript(abilityIndex);
                EditorUtility.OpenWithDefaultApp(ability.ScriptPath);
            }
            else
            {
                if (!File.Exists(ability.ScriptPath))
                {
                    editor.CreateScript(abilityIndex);
                }
                EditorUtility.OpenWithDefaultApp(ability.ScriptPath);
            }
        }

        GUILayout.Label("ID: " + ability.ID);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        ability.abilityName = GUILayout.TextField(ability.abilityName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Category");
        ability.Category = (AbilityCategory)EditorGUILayout.EnumPopup(ability.Category);
        GUILayout.EndHorizontal();

        if (ability._DefaultGlobalVariables == null)
        {
            ability._DefaultGlobalVariables = new GlobalVariable[0];
        }
        if(foldouts == null || foldouts.Length != ability._DefaultGlobalVariables.Length)
        {
            foldouts = new bool[ability._DefaultGlobalVariables.Length];
        }

        for (int j = 0; j < ability._DefaultGlobalVariables.Length; j++)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            foldouts[j] = EditorGUILayout.Foldout(foldouts[j], "Variable: " + ability._DefaultGlobalVariables[j].Name);
            if (!foldouts[j])
            {
                continue;
            }
            EditorGUILayout.PropertyField(globalArray.GetArrayElementAtIndex(j));

            if (GUILayout.Button("Delete Global Variable"))
            {
                var newVar = new GlobalVariable[ability._DefaultGlobalVariables.Length - 1];
                for (int k = 0; k < ability._DefaultGlobalVariables.Length; k++)
                {
                    if (k == j) continue;
                    newVar[k] = ability._DefaultGlobalVariables[k];
                }
                ability._DefaultGlobalVariables = newVar;
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        GUILayout.BeginHorizontal();
        selectedGlobalVariableType = (GlobalVariableType)EditorGUILayout.EnumPopup("Add Global Variable", selectedGlobalVariableType);
        if (GUILayout.Button("Add Globalvar"))
        {
            var newVar = new GlobalVariable[ability._DefaultGlobalVariables.Length + 1];
            for (int j = 0; j < ability._DefaultGlobalVariables.Length; j++)
            {
                newVar[j] = ability._DefaultGlobalVariables[j];
            }
            newVar[ability._DefaultGlobalVariables.Length] = new GlobalVariable("", selectedGlobalVariableType);

            ability._DefaultGlobalVariables = newVar;
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Save"))
        {
            editor.Save();
        }
    }
}
#endif