using EcologyRPG.AbilityScripting;
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
    }

    AbilityEditor editor;
    int abilityIndex;
    GlobalVariableType selectedGlobalVariableType;
    bool[] foldouts;


    private void OnGUI()
    {
        if (editor == null)
        {
            Close();
            return;
        }

        var ability = AbilityEditor.abilityData.data[abilityIndex];
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
            var globalVariable = ability._DefaultGlobalVariables[j];
            GUILayout.BeginHorizontal();
            GUILayout.Label("Global Variable Name");
            globalVariable.Name = GUILayout.TextField(ability._DefaultGlobalVariables[j].Name);
            GUILayout.EndHorizontal();

            if (ability._DefaultGlobalVariables[j].Type == GlobalVariableType.String)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Global Variable Value");
                globalVariable.Value = GUILayout.TextField(globalVariable.GetString());
                GUILayout.EndHorizontal();
            }
            else if (ability._DefaultGlobalVariables[j].Type == GlobalVariableType.Int)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Global Variable Value");
                globalVariable.Value = EditorGUILayout.IntField(globalVariable.GetInt()).ToString();
                GUILayout.EndHorizontal();
            }
            else if (ability._DefaultGlobalVariables[j].Type == GlobalVariableType.Float)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Global Variable Value");
                globalVariable.Value = EditorGUILayout.FloatField(globalVariable.GetFloat()).ToString();
                GUILayout.EndHorizontal();
            }
            else if (ability._DefaultGlobalVariables[j].Type == GlobalVariableType.Bool)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Global Variable Value");
                globalVariable.Value = EditorGUILayout.Toggle(globalVariable.GetBool()).ToString();
                GUILayout.EndHorizontal();
            }

            if(GUILayout.Button("Delete Global Variable"))
            {
                var newVar = new GlobalVariable[ability._DefaultGlobalVariables.Length - 1];
                for (int k = 0; k < ability._DefaultGlobalVariables.Length; k++)
                {
                    if (k == j) continue;
                    newVar[k] = ability._DefaultGlobalVariables[k];
                }
                ability._DefaultGlobalVariables = newVar;
            }
            ability._DefaultGlobalVariables[j] = globalVariable;
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
            if (selectedGlobalVariableType == GlobalVariableType.String)
                newVar[ability._DefaultGlobalVariables.Length] = new GlobalVariable(name, selectedGlobalVariableType, "");
            else if (selectedGlobalVariableType == GlobalVariableType.Int)
                newVar[ability._DefaultGlobalVariables.Length] = new GlobalVariable(name, selectedGlobalVariableType, "0");
            else if (selectedGlobalVariableType == GlobalVariableType.Float)
                newVar[ability._DefaultGlobalVariables.Length] = new GlobalVariable(name, selectedGlobalVariableType, "0.0");
            ability._DefaultGlobalVariables = newVar;
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Save"))
        {
            editor.Save();
        }
    }
}