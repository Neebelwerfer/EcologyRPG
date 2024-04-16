using UnityEditor;
using UnityEngine;
using static EcologyRPG.Core.Items.Item;

namespace EcologyRPG.Core.Items
{
    [CustomEditor(typeof(Item), false)]
    public class ItemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var item = target as Item;

            if (item.generationRules == null)
            {
                if (GUILayout.Button("Allow Random Generation"))
                {
                    item.generationRules = CreateInstance<BasicGenerationRules>();
                    AssetDatabase.AddObjectToAsset(item.generationRules, item);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    EditorUtility.SetDirty(item);
                }
            }
            else
            {
                EditorGUILayout.LabelField("Generation Rules", EditorStyles.boldLabel);
                var generationRules = item.generationRules as BasicGenerationRules;
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