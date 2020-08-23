
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnScene : MonoBehaviour {
    public List<int> scenes;
    private void Awake() {
        SceneManager.sceneLoaded += destroyOnScene;
    }
    
    private void destroyOnScene(Scene scene, LoadSceneMode loadSceneMode) {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (scenes.Contains(currentSceneIndex)) {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= destroyOnScene;
    }

}
