using EcologyRPG.Utility;
using UnityEditor;
using UnityEngine;

public class TagEditor : EditorWindow
{
    [MenuItem("Game/Tag Editor")]
    static void OpenWindow()
    {
       GetWindow<TagEditor>();
    }

    Tags tags;

    TagEditor()
    {
    }

    private void OnEnable()
    {
        tags = AssetDatabase.LoadAssetAtPath<Tags>(Tags.path);
    }

    private void OnGUI()
    {
        if(tags == null) return;
        int tagToRemove = -1;
        for (int i = 0; i < tags.tags.Count; i++)
        {
            GUILayout.BeginHorizontal();
            tags.tags[i] = EditorGUILayout.TextField(tags.tags[i]);
            if (GUILayout.Button("X"))
            {
                tagToRemove = i;
            }
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Tag"))
        {
            tags.tags.Add("");
        }

        if(tagToRemove > -1)
        {
            tags.tags.RemoveAt(tagToRemove);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(tags);
        }
    }
}