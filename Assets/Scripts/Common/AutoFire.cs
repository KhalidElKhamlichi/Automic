
using Automic.Player;
using UnityEngine;

namespace Automic.Common {
    [RequireComponent(typeof(Weapon))]
    public class AutoFire : MonoBehaviour {
        public Transform target;
        public int nbrOfProjectilesInBurst;
        public float delayBetweenBursts;
        public float maxInitialDelay = 1f;
    
        private Weapon weapon;
        private float burstTimer;
        private int projectilesFired;
        private float initialDelay;
        void Start() {
            weapon = GetComponent<Weapon>();
            initialDelay = Random.Range(.2f, maxInitialDelay);
        }

        void FixedUpdate() {
            if (initialDelay > 0) {
                initialDelay -= Time.deltaTime;
                return;
            }
            burstTimer -= Time.deltaTime;
            if (burstTimer <= 0) {
                bool fired = weapon.fire(target);
                if (fired) projectilesFired++;
                if (projectilesFired >= nbrOfProjectilesInBurst) {
                    projectilesFired = 0;
                    burstTimer = delayBetweenBursts;
                }
            }
        }
    }
}
