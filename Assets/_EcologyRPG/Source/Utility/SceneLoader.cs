using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcologyRPG.Utility
{
    public class SceneLoader : MonoBehaviour
    {

        public void Setup(SceneReference targetScene, string sceneToUnload)
        {
            SceneManager.UnloadSceneAsync(sceneToUnload);
            var op = targetScene.LoadSceneAsync(LoadSceneMode.Additive);
            op.completed += (p) =>
            {
                var thisScene = SceneManager.GetActiveScene();
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(targetScene.BuildIndex));
                SceneManager.UnloadSceneAsync(thisScene);
            };
        }
    }
}