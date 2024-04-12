using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcologyRPG.Utility
{
    public class MoveToScene : MonoBehaviour
    {
        public static SceneReference TransitionSceneReference;

        public SceneReference sceneReference;

        public void Load()
        {
            Load(sceneReference);
        }

        public static void Load(SceneReference target)
        {
            var activeScene = SceneManager.GetActiveScene();
            var op = TransitionSceneReference.LoadSceneAsync(LoadSceneMode.Additive);
            op.completed += (p) =>
            {
                var SceneLoader = FindObjectOfType<SceneLoader>();
                SceneLoader.Setup(target, activeScene.name);
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(TransitionSceneReference.BuildIndex));
            };
        }
    }
}