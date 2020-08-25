using UnityEngine;
using UnityEngine.SceneManagement;

namespace Automic.UI {
    public class ButtonActions : MonoBehaviour
    {
    
        public void nextScene() {
            Invoke("loadScene", .4f);
        }

        private void loadScene() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
