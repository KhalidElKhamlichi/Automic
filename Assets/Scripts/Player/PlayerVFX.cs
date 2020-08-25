using Automic.Common;
using UnityEngine;

namespace Automic.Player {
    public class PlayerVFX : MonoBehaviour {

        public GameObject hitVFX;
    
        private Animator animator;
        private static readonly int HURT = Animator.StringToHash("Hurt");


        void Start() {
            GetComponent<CollisionManager>().onHit(playOnHitVFX);
            animator = GetComponent<Animator>();
        }

        private void playOnHitVFX(Collision other) {
            Instantiate(hitVFX, transform.position, hitVFX.transform.rotation, transform);
            animator.SetTrigger(HURT);
        }
    }
}
