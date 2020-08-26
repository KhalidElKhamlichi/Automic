using System;
using System.Collections.Generic;
using Automic.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Automic.Managers {
    public class GameManager : MonoBehaviour {
        [SerializeField] int[] nbrOfRemainingEnemiesSpawnTriggerByScene;
        
        private const string GAME_MANAGER_TAG = "GameManager";
        private const string CANVAS_OBJECT_NAME = "Canvas";
        private const string TIME_UI_OBJECT_NAME = "Time";
        private const string ENEMY_TAG = "Enemy";
        private const string TOSPAWN_OBJECT_TAG = "ToSpawn";
        private const string RECORD_TIME_KEY = "Record time";
        private const string PLAY_TIME_FORMAT = "0.00";
        private const string ENEMY_PROJECTILE_TAG = "ENEMY_PROJECTILE";
        private event Action levelEndEvent;
        private List<GameObject> enemiesToSpawn;
        private static GameManager instance;
        private int currentEnemyCount;
        private bool spawned;
        private float playTime;
        private TextMeshProUGUI playTimeText => transform.Find(CANVAS_OBJECT_NAME).Find(TIME_UI_OBJECT_NAME).GetComponent<TextMeshProUGUI>();

        private void Awake() {
            Cursor.visible = false;
            GameObject[] objs = GameObject.FindGameObjectsWithTag(GAME_MANAGER_TAG);

            if (objs.Length > 1) {
                Destroy(gameObject);
            } 
            else {
                SceneManager.sceneLoaded += onSceneLoaded;
            }
            DontDestroyOnLoad(gameObject);
        }

        private void onSceneLoaded(Scene arg0, LoadSceneMode arg1) {
            currentEnemyCount = 0;
            spawned = false;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(ENEMY_TAG);
            getEnemiesToSpawn();
            subscribeToEnemyDeathEvent(enemies);
        }

        private void getEnemiesToSpawn() {
            enemiesToSpawn = new List<GameObject>();
            Transform toSpawn = GameObject.FindGameObjectWithTag(TOSPAWN_OBJECT_TAG)?.transform;
            if (toSpawn != null) {
                for (int i = 0; i < toSpawn.childCount; i++) {
                    Transform enemy = toSpawn.GetChild(i);
                    enemiesToSpawn.Add(enemy.gameObject);
                }

                toSpawn.DetachChildren();
            }
        }

        private void Update() {
            playTime += Time.deltaTime;
            playTimeText.text = playTime.ToString(PLAY_TIME_FORMAT);
        }

        private void checkSpawnTrigger() {
            int nbrOfRemainingEnemiesSpawnTrigger = nbrOfRemainingEnemiesSpawnTriggerByScene[SceneManager.GetActiveScene().buildIndex];
            if (spawned || nbrOfRemainingEnemiesSpawnTrigger <= 0) return;
            
            if (currentEnemyCount <= nbrOfRemainingEnemiesSpawnTrigger) {
                spawnEnemies();
            }
        }

        private void spawnEnemies() {
            foreach (GameObject ennemy in enemiesToSpawn) {
                ennemy.SetActive(true);
            }

            spawned = true;
            subscribeToEnemyDeathEvent(enemiesToSpawn.ToArray());
        }

        private void subscribeToEnemyDeathEvent(GameObject[] enemies) {
            foreach (GameObject enemy in enemies) {
                Lifecycle lifecycle = enemy.GetComponent<Lifecycle>();
                lifecycle.onDeath(reduceEnemyCount);
                lifecycle.onDeath(checkLvlEnd);
                lifecycle.onDeath(checkSpawnTrigger);
            }
            currentEnemyCount += enemies.Length;
        }

        private void reduceEnemyCount() {
            currentEnemyCount--;
        }

        private void checkLvlEnd() {
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
            string currentPlayTime = playTime.ToString(PLAY_TIME_FORMAT);
            string currentRecordTime = PlayerPrefs.GetString(RECORD_TIME_KEY);
            if (string.IsNullOrEmpty(currentRecordTime) || float.Parse(currentPlayTime) < float.Parse(currentRecordTime)) {
                PlayerPrefs.SetString(RECORD_TIME_KEY, currentPlayTime);
            }
        }

        private void clearEnemyProjectiles() {
            foreach (GameObject projectile in GameObject.FindGameObjectsWithTag(ENEMY_PROJECTILE_TAG)) {
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
}
