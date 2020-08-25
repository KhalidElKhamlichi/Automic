using System.Collections.Generic;
using Automic.Common;
using UnityEngine;

namespace Automic {
    public class MasterShield : MonoBehaviour {
        public List<Lifecycle> slaves;
        public GameObject deathVFX;

        void Start() {
            foreach (Lifecycle lifecycle in slaves) {
                lifecycle.onDeath(checkLife);
            }
        }

        private void checkLife() {
            slaves.RemoveAt(0);
            if (slaves.Count == 0) {
                Instantiate(deathVFX, transform.position, deathVFX.transform.rotation, transform.parent);
                Destroy(gameObject);
            }
        }
    }
}
