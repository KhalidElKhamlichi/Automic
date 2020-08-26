using Automic.Sound;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Automic.Managers {
    public class LevelLoader : MonoBehaviour {
        private const string LEVEL_LOADER_TAG = "LevelLoader";
        [SerializeField] Audio lvlClearAudio;
        [SerializeField] Audio transitionAudio;
        [SerializeField] GameObject audioPlayer;

        private static LevelLoader instance;
        private Animator levelTransitionCanvasAnimator;
        private Animator pauseCanvasAnimator;
        private bool isPaused;
        private GameManager gameManager;
        private static readonly int FADE_OUT = Animator.StringToHash("Fadeout");
        private static readonly int FADE_IN = Animator.StringToHash("FadeIn");

        private void Awake() {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(LEVEL_LOADER_TAG);

            if (objs.Length > 1) {
                Destroy(gameObject);
            }
            else {
                gameManager = FindObjectOfType<GameManager>();
                subscribeToGameEvents();
            }
            DontDestroyOnLoad(gameObject);
        }

        void Start() {
            levelTransitionCanvasAnimator = transform.GetChild(0).GetComponent<Animator>();
            pauseCanvasAnimator = transform.GetChild(1).GetComponent<Animator>();
        }

        private void subscribeToGameEvents() {
            gameManager.onLevelEnd(loadNextLevel);
            gameManager.onLevelEnd(playLvlClearAudio);
        }

        void Update() {
            if (Input.GetButtonDown("Pause")) {
                pause();
            }
        }

        public void resume() {
            isPaused = false;
            Cursor.visible = false;
            // Unselect resume button to keep regular on hover color
            EventSystem.current.SetSelectedGameObject(null);
            pauseCanvasAnimator.SetTrigger(FADE_OUT);
            Time.timeScale = 1;
        }

        // Used by onClick button event
        public void exit() {
            resume();
            Cursor.visible = true;
            SceneManager.LoadScene(0);
        }

        private void playLvlClearAudio() {
            Instantiate(audioPlayer).GetComponent<AudioPlayer>().play(lvlClearAudio);
        }

        private void loadNextLevel() {
            Invoke(nameof(fadeInLevelTransition), .8f);
        }

        private void fadeInLevelTransition() {
            levelTransitionCanvasAnimator.SetTrigger(FADE_IN);
            Instantiate(audioPlayer).GetComponent<AudioPlayer>().play(transitionAudio);
            Invoke(nameof(nextScene), 1.2f );
        }


        private void nextScene() {
            if(SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings) SceneManager.LoadScene(0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void pause() {
            if(isPaused) return;
            isPaused = true;
            Cursor.visible = true;
            pauseCanvasAnimator.SetTrigger(FADE_IN);
            Time.timeScale = 0;
        }

        private void OnDestroy() {
            gameManager?.removeFromLevelEndEvent(loadNextLevel);
            gameManager?.removeFromLevelEndEvent(playLvlClearAudio);    
        }
    }
}
