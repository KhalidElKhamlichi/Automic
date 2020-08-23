using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    public Audio lvlClearAudio;
    public Audio transitionAudio;
    public GameObject audioPlayer;

    private static LevelLoader instance;
    private Animator levelTransitionCanvasAnimator;
    private Animator pauseCanvasAnimator;
    private bool isPaused;
    private GameManager gameManager;
    private void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("LevelLoader");

        if (objs.Length > 1) {
            Destroy(gameObject);
        }
        else {
            gameManager = FindObjectOfType<GameManager>();
            subscribeToGameEvents();
            // SceneManager.sceneLoaded += onSceneLoaded;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        levelTransitionCanvasAnimator = transform.GetChild(0).GetComponent<Animator>();
        pauseCanvasAnimator = transform.GetChild(1).GetComponent<Animator>();
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode) {
        // subscribeToGameEvents();
    }

    private void subscribeToGameEvents() {
        gameManager.onLevelEnd(loadNextLevel);
        gameManager.onLevelEnd(playLvlClearAudio);
    }

    private void OnDestroy() {
        gameManager?.removeFromLevelEndEvent(loadNextLevel);
        gameManager?.removeFromLevelEndEvent(playLvlClearAudio);    
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
        pauseCanvasAnimator.SetTrigger("Fadeout");
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
        Invoke("fadeInLevelTransition", .8f);
    }

    private void fadeInLevelTransition() {
        levelTransitionCanvasAnimator.SetTrigger("FadeIn");
        Instantiate(audioPlayer).GetComponent<AudioPlayer>().play(transitionAudio);
        Invoke("nextScene", 1.2f );
    }


    private void nextScene() {
        if(SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings) SceneManager.LoadScene(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void pause() {
        if(isPaused) return;
        isPaused = true;
        Cursor.visible = true;
        pauseCanvasAnimator.SetTrigger("FadeIn");
        Time.timeScale = 0;
        
    }
}
