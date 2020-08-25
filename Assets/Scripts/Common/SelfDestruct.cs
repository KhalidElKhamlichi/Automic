using UnityEngine;

namespace Automic.Common {
    public class SelfDestruct : MonoBehaviour {

        public float lifetime;
        void Start() {
            Destroy(gameObject, lifetime);
        }

    }
}
