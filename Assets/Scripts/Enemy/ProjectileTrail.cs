using System.Collections;
using UnityEngine;

namespace Automic {
    public class ProjectileTrail : MonoBehaviour {

        [SerializeField] GameObject trailObject;
        [SerializeField] float spawnRatePerSecond;

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
}
