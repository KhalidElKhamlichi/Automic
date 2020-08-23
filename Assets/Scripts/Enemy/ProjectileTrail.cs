using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrail : MonoBehaviour {

    public GameObject trailObject;
    public float spawnRatePerSecond;

    private float spawnTimer;
    private Transform spawnPoint;
    void Start() {
        spawnTimer = 1 / spawnRatePerSecond;
        spawnPoint = transform.GetChild(1);
        StartCoroutine(instantiateTrail());
    }

    private IEnumerator instantiateTrail()
    {
        while (true) {
            Instantiate(trailObject, spawnPoint.position, transform.rotation);
            yield return new WaitForSeconds(spawnTimer);
        }

    }
}
