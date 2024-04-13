using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcologyRPG.Utility
{
    [CreateAssetMenu(fileName = "SceneReference", menuName = "SceneReference")]
    public class SceneReference : ScriptableObject
    {
#if UNITY_EDITOR
        public int BuildIndex { get { return SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(sceneAsset)); } }
#else
        public int BuildIndex { get { return buildIndex; } }
#endif
        [SerializeField] int buildIndex = -1;

#if UNITY_EDITOR
        public SceneAsset sceneAsset;
#endif

        private void OnValidate()
        {
            buildIndex = BuildIndex;
        }

        public AsyncOperation LoadSceneAsync(LoadSceneMode mode = LoadSceneMode.Single)
        {
            Debug.Log("Loading scene: " + BuildIndex);
            if (BuildIndex >= 0)
            {
                return SceneManager.LoadSceneAsync(BuildIndex, mode);
            }
            return null;
        }

        public void LoadScene(LoadSceneMode mode = LoadSceneMode.Single)
        {
            Debug.Log("Loading scene: " + BuildIndex);
            if (BuildIndex >= 0)
            {
                SceneManager.LoadScene(BuildIndex, mode);
            }
        }

        public void UnloadScene()
        {
            if (BuildIndex >= 0)
            {
                SceneManager.UnloadSceneAsync(BuildIndex);
            }
        }
    }
}
