using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class SceneWaiter : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        [SerializeField] private bool setActiveScene = true;
        [SerializeField] private UnityEvent onSceneLoaded, onSceneUnloaded;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != sceneName)
            {
                return;
            }

            if (setActiveScene)
            {
                SceneManager.SetActiveScene(scene);
            }

            onSceneLoaded?.Invoke();
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (scene.name != sceneName)
            {
                return;
            }

            onSceneUnloaded?.Invoke();
        }
    }
}