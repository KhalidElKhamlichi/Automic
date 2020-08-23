using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public event Action levelEndEvent;
    public int[] nbrOfRemainingEnemiesSpawnTriggerByScene;
    
    private List<GameObject> enemiesToSpawn;
    private static GameManager instance;
    private int currentEnemyCount;
    private bool spawned;
    private float playTime;
    private TextMeshProUGUI playTimeText;

    private void Awake() {
        Cursor.visible = false;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");

        if (objs.Length > 1) {
            Destroy(gameObject);
        } 
        else {
            SceneManager.sceneLoaded += onSceneLoaded;
            playTimeText = transform.Find("Canvas").Find("Time").GetComponent<TextMeshProUGUI>();
        }
        DontDestroyOnLoad(gameObject);
    }

    private void onSceneLoaded(Scene arg0, LoadSceneMode arg1) {
        currentEnemyCount = 0;
        spawned = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesToSpawn = new List<GameObject>();
        Transform toSpawn = GameObject.FindGameObjectWithTag("ToSpawn")?.transform;
        if (toSpawn != null) {
            for (int i = 0; i < toSpawn.childCount; i++) {
                Transform enemy = toSpawn.GetChild(i);
                enemiesToSpawn.Add(enemy.gameObject);
            }
            toSpawn.DetachChildren();
        }
        subscribeToEnemyDeathEvent(enemies);
    }

    private void Update() {
        playTime += Time.deltaTime;
        playTimeText.text = playTime.ToString("0.00");
    }

    private void checkSpawnTrigger() {
        int nbrOfRemainingEnemiesSpawnTrigger = nbrOfRemainingEnemiesSpawnTriggerByScene[SceneManager.GetActiveScene().buildIndex];
        if (spawned || nbrOfRemainingEnemiesSpawnTrigger <= 0) {
            return;
        }
        if (currentEnemyCount <= nbrOfRemainingEnemiesSpawnTrigger) {
            foreach (GameObject ennemy in enemiesToSpawn) {
                ennemy.SetActive(true);
            }
            spawned = true;
            subscribeToEnemyDeathEvent(enemiesToSpawn.ToArray());
        }
    }

    private void subscribeToEnemyDeathEvent(GameObject[] enemies) {
        foreach (GameObject enemy in enemies) {
            Lifecycle lifecycle = enemy.GetComponent<Lifecycle>();
            lifecycle.onDeath(updateEnemyCount);
            lifecycle.onDeath(checkSpawnTrigger);
        }
        currentEnemyCount += enemies.Length;
    }

    private void updateEnemyCount() {
        currentEnemyCount--;
        if (currentEnemyCount == 0) {
            clearEnemyProjectiles();
            levelEndEvent?.Invoke();
            if (isLastScene()) {
                updateRecordTime();
            }
        }
    }

    private static bool isLastScene() {
        return SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings;
    }

    private void updateRecordTime() {
        string currentPlayTime = playTime.ToString("0.00");
        string currentRecordTime = PlayerPrefs.GetString("Record time");
        if (string.IsNullOrEmpty(currentRecordTime) || float.Parse(currentPlayTime) < float.Parse(currentRecordTime)) {
            PlayerPrefs.SetString("Record time", currentPlayTime);
        }
    }

    private void clearEnemyProjectiles() {
        foreach (GameObject projectile in GameObject.FindGameObjectsWithTag("ENEMY_PROJECTILE")) {
            Destroy(projectile);
        }
    }

    public void onLevelEnd(Action onLevelEnd) {
        levelEndEvent += onLevelEnd;
    }

    public void removeFromLevelEndEvent(Action onLevelEnd) {
        levelEndEvent -= onLevelEnd;
    }

    private void OnDestroy() {
        levelEndEvent = null;
    }
}
