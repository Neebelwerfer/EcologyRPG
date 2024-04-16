using UnityEditor;
using UnityEngine;
using static EcologyRPG.Core.Items.Item;

namespace EcologyRPG.Core.Items
{
    [CustomEditor(typeof(Armour), true)]
    public class ArmourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var item = target as Armour;

            if (item.generationRules == null)
            {
                if (GUILayout.Button("Allow Random Generation"))
                {
                    item.generationRules = CreateInstance<ArmourGenerationRules>();
                    AssetDatabase.AddObjectToAsset(item.generationRules, item);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    EditorUtility.SetDirty(item);
                }
            }
            else
            {
                EditorGUILayout.LabelField("Generation Rules", EditorStyles.boldLabel);
                var generationRules = item.generationRules as EquipmentGenerationRules;
                if (generationRules.Modifiers == null)
                {
                    generationRules.Modifiers = new System.Collections.Generic.List<Ranges>();
                }
                var editor = CreateEditor(generationRules);
                editor.OnInspectorGUI();
                if (GUILayout.Button("Disallow Random Generation"))
                {
                    DestroyImmediate(item.generationRules, true);
                    item.generationRules = null;
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    EditorUtility.SetDirty(item);
                }

                if (GUI.changed)
                    EditorUtility.SetDirty(item);
            }
        }
    }
}