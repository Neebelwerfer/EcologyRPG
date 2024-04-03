using EcologyRPG.Game;
using UnityEditor;

public class GameSettingsEditor : EditorWindow
{
    const string path = "Assets/_EcologyRPG/Resources/Config/GameSettings.asset";

    [MenuItem("Game/Game Settings")]
    public static void ShowWindow()
    {
        GetWindow<GameSettingsEditor>("Game Settings");
    }

    GameSettings GameSettings;

    private void OnEnable()
    {
        GameSettings = AssetDatabase.LoadAssetAtPath<GameSettings>(path);
    }

    private void OnGUI()
    {
        if(GameSettings == null)
        {
            GameSettings = CreateInstance<GameSettings>();
            AssetDatabase.CreateAsset(GameSettings, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        Editor.CreateEditor(GameSettings).OnInspectorGUI();
    }
}