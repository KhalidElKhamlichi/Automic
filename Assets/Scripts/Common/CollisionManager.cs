using System;
using UnityEngine;

namespace Automic.Common {
    public class CollisionManager : MonoBehaviour {
        [SerializeField] string hitTriggerTag;
        private event Action<Collision> hitEvent;

        private void OnCollisionEnter(Collision other) {
            if (other.collider.tag.Equals(hitTriggerTag)) {
                hitEvent?.Invoke(other);
            }
        }

        public void onHit(Action<Collision> onHit) => hitEvent += onHit;
        
    }
}
