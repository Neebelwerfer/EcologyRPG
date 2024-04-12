using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcologyRPG.Utility
{
    [CreateAssetMenu(fileName = "SceneReference", menuName = "SceneReference")]
    public class SceneReference : ScriptableObject
    {
        public int BuildIndex { get { return buildIndex; } }

#if UNITY_EDITOR
        public SceneAsset sceneAsset;
 #endif
        public int buildIndex = -1;

        public AsyncOperation LoadSceneAsync(LoadSceneMode mode = LoadSceneMode.Single)
        {
            Debug.Log("Loading scene: " + buildIndex);
            if (buildIndex >= 0)
            {
                return SceneManager.LoadSceneAsync(buildIndex, mode);
            }
            return null;
        }

        public void LoadScene(LoadSceneMode mode = LoadSceneMode.Single)
        {
            Debug.Log("Loading scene: " + buildIndex);
            if (buildIndex >= 0)
            {
                SceneManager.LoadScene(buildIndex, mode);
            }
        }

        public void UnloadScene()
        {
            if (buildIndex >= 0)
            {
                SceneManager.UnloadSceneAsync(buildIndex);
            }
        }
    }
}
