using Automic.Player;
using UnityEngine;

namespace Automic.Camera {
    public class CameraTracking : MonoBehaviour {
        [SerializeField] float speed;
        [SerializeField] float horizontalOffset;

        private Transform target;
        private CameraShake cameraShake;

        private void Start() {
            target = FindObjectOfType<PlayerLifecycle>().transform;
            cameraShake = GetComponent<CameraShake>();
        }

        void FixedUpdate() {
            if (cameraShake.isShaking) return;
            Vector3 currentPosition = transform.position;
            Vector3 newPosition = new Vector3(target.position.x-horizontalOffset, currentPosition.y, target.position.z); 
            transform.position = Vector3.Lerp(currentPosition, newPosition, speed * Time.deltaTime);
        }
    }
}
