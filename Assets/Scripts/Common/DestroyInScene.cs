using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Automic.Common {
    public class DestroyInScene : MonoBehaviour {
        [SerializeField] List<int> sceneIndexes;
        private void Awake() {
            SceneManager.sceneLoaded += destroyOnScene;
        }
    
        private void destroyOnScene(Scene scene, LoadSceneMode loadSceneMode) {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (sceneIndexes.Contains(currentSceneIndex)) {
                Destroy(gameObject);
            }
        }

        private void OnDestroy() {
            SceneManager.sceneLoaded -= destroyOnScene;
        }

    }
}
