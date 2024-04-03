using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// Scene auto loader.
/// </summary>
/// <description>
/// This class adds a File > Scene Autoload menu containing options to select
/// a "master scene" enable it to be auto-loaded when the user presses play
/// in the editor. When enabled, the selected scene will be loaded on play,
/// then the original scene will be reloaded on stop.
///
/// Based on an idea on this thread:
/// http://forum.unity3d.com/threads/157502-Executing-first-scene-in-build-settings-when-pressing-play-button-in-editor
/// </description>
[InitializeOnLoad]
public static class SceneAutoLoader
{
    // Static constructor binds a playmode-changed callback.
    // [InitializeOnLoad] above makes sure this gets execusted.
    static SceneAutoLoader()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    // Menu items to select the "master" scene and control whether or not to load it.
    [MenuItem("Scene Autoload/Select GameManager Scene...")]
    private static void SelectMasterScene()
    {
        string gameManagerScene = EditorUtility.OpenFilePanel("Select GameManager Scene", Application.dataPath, "unity");
        if (!string.IsNullOrEmpty(gameManagerScene))
        {
            GameManagerScene = gameManagerScene;
        }
    }

    // Play mode change callback handles the scene load/reload.
    private static void OnPlayModeChanged(PlayModeStateChange change)
    {
        if (change == PlayModeStateChange.ExitingEditMode)
        {
            // User pressed play -- autoload master scene.
            PreviousScene = SceneManager.GetActiveScene().path;
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(GameManagerScene, OpenSceneMode.Single);
            }
            else
            {
                // User cancelled the save operation -- cancel play as well.
                EditorApplication.isPlaying = false;
            }
        }
        if(change == PlayModeStateChange.EnteredPlayMode)
        {
            var prevScene = EditorSceneManager.LoadSceneAsyncInPlayMode(PreviousScene, new LoadSceneParameters(LoadSceneMode.Additive));
            prevScene.completed += (op) =>
            {
                EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneByPath(PreviousScene));
            };
        }
        if (change == PlayModeStateChange.EnteredEditMode)
        {
            // User pressed stop -- reload previous scene.
            if (SceneAutoLoader.PreviousScene != SceneAutoLoader.GameManagerScene)
            {
                EditorApplication.update += ReloadLastScene;
            }
        }
    }

    public static void ReloadLastScene()
    {
        Debug.Log("Reloading..");
        EditorSceneManager.OpenScene(PreviousScene, OpenSceneMode.Single);
        EditorApplication.update -= ReloadLastScene;
    }

    // Properties are remembered as editor preferences.
    private const string cEditorPrefMasterScene = "SceneAutoLoader.MasterScene";
    private const string cEditorPrefPreviousScene = "SceneAutoLoader.PreviousScene";


    private static string GameManagerScene
    {
        get { return EditorPrefs.GetString(cEditorPrefMasterScene, "Assets/_EcologyRPG/Levels/GameManager.unity"); }
        set { EditorPrefs.SetString(cEditorPrefMasterScene, value); }
    }

    private static string _previousScene;

    public static string PreviousScene
    {
        get { return EditorPrefs.GetString(cEditorPrefPreviousScene, _previousScene); }
        set
        {
            _previousScene = value;
            EditorPrefs.SetString(cEditorPrefPreviousScene, value);
        }
    }
}