using EcologyRPG.Core.Abilities;
using System.IO;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UIElements;

public class SingleConditionEditor : EditorWindow
{
    private int index;
    public static void Open(int conditionIndex)
    {
        var window = GetWindow<SingleConditionEditor>();
        window.index = conditionIndex;
        window.Show();
    }

    GlobalVariableType selectedGlobalVariableType;
    SerializedConditionArray conditions;
    bool[] foldouts;

    private void OnEnable()
    {
        if(ConditionEditor.conditionData == null)
        {
            ConditionEditor.Load();
        }
        conditions = ConditionEditor.conditionData;
    }

    private void OnGUI()
    {
        var condition = conditions.data[index];
        if (condition == null)
        {
            Close();
            return;
        }

        if (GUILayout.Button("Edit Script Behaviour"))
        {
            if (condition.ScriptPath == null || condition.ScriptPath == "")
            {
                ConditionEditor.CreateScript(index);
                EditorUtility.OpenWithDefaultApp(condition.ScriptPath);
            }
            else
            {
                if (!File.Exists(condition.ScriptPath))
                {
                    ConditionEditor.CreateScript(index);
                }
                EditorUtility.OpenWithDefaultApp(condition.ScriptPath);
            }
        }

        GUILayout.Label("ID: " + condition.ID);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        condition.ConditionName = GUILayout.TextField(condition.ConditionName);
        GUILayout.EndHorizontal();

        if (condition._DefaultGlobalVariables == null)
        {
            condition._DefaultGlobalVariables = new GlobalVariable[0];
        }
        if (foldouts == null || foldouts.Length != condition._DefaultGlobalVariables.Length)
        {
            foldouts = new bool[condition._DefaultGlobalVariables.Length];
        }

        for (int j = 0; j < condition._DefaultGlobalVariables.Length; j++)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            foldouts[j] = EditorGUILayout.Foldout(foldouts[j], "Variable: " + condition._DefaultGlobalVariables[j].Name);
            if (!foldouts[j])
            {
                continue;
            }
            var globalVariable = condition._DefaultGlobalVariables[j];
            GUILayout.BeginHorizontal();
            GUILayout.Label("Global Variable Name");
            globalVariable.Name = GUILayout.TextField(condition._DefaultGlobalVariables[j].Name);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Global Variable Value");
            globalVariable.DrawEditableValue();
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Delete Global Variable"))
            {
                var newVar = new GlobalVariable[condition._DefaultGlobalVariables.Length - 1];
                for (int k = 0; k < condition._DefaultGlobalVariables.Length; k++)
                {
                    if (k == j) continue;
                    newVar[k] = condition._DefaultGlobalVariables[k];
                }
                condition._DefaultGlobalVariables = newVar;
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        GUILayout.BeginHorizontal();
        selectedGlobalVariableType = (GlobalVariableType)EditorGUILayout.EnumPopup("Add Global Variable", selectedGlobalVariableType);
        if (GUILayout.Button("Add Globalvar"))
        {
            var newVar = new GlobalVariable[condition._DefaultGlobalVariables.Length + 1];
            for (int j = 0; j < condition._DefaultGlobalVariables.Length; j++)
            {
                newVar[j] = condition._DefaultGlobalVariables[j];
            }
            if (selectedGlobalVariableType == GlobalVariableType.String)
                newVar[condition._DefaultGlobalVariables.Length] = new StringGlobalVariable(name, "");
            else if (selectedGlobalVariableType == GlobalVariableType.Int)
                newVar[condition._DefaultGlobalVariables.Length] = new IntGlobalVariable(name, 0);
            else if (selectedGlobalVariableType == GlobalVariableType.Float)
                newVar[condition._DefaultGlobalVariables.Length] = new FloatGlobalVariable(name, 0f);
            else if (selectedGlobalVariableType == GlobalVariableType.Bool)
                newVar[condition._DefaultGlobalVariables.Length] = new BoolGlobalVariable(name, false);
            else if (selectedGlobalVariableType == GlobalVariableType.DamageType)
                newVar[condition._DefaultGlobalVariables.Length] = new DamageTypeGlobalVariable(name, DamageType.Physical);
            else if (selectedGlobalVariableType == GlobalVariableType.AbilityID)
                newVar[condition._DefaultGlobalVariables.Length] = new AbilityIDGlobalVariable(name, 0);
            condition._DefaultGlobalVariables = newVar;
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Save"))
        {
            ConditionEditor.Save();
        }
    }
}