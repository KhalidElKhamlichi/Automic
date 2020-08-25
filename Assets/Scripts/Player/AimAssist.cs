using Automic.Common;
using UnityEngine;

namespace Automic.Player {
    [RequireComponent(typeof(GenericController))]
    public class AimAssist : MonoBehaviour {
        [SerializeField, Range(0f, 10f)]
        float slowRotationSpeed;
        public LayerMask collisionMask;
    
        private GenericController controller;

        void Start() {
            controller = GetComponent<GenericController>();
        }

        void Update() {
            if (castRay(Vector3.zero, transform.forward)
                || castRay(transform.right / 1.5f - transform.forward / 1.5f, transform.forward)
                || castRay(-transform.right / 1.5f - transform.forward / 1.5f, transform.forward)) {
                controller.slowRotation(slowRotationSpeed);
            }
            else {
                controller.resetRotation();
            }
        }
    
        private bool castRay(Vector3 offset, Vector3 direction) {
            Ray ray = new Ray(transform.position + offset, direction);

            bool hit = Physics.Raycast(ray, out var hitInfo, 100, collisionMask);
            if (hit) {
                Debug.DrawLine(ray.origin, hitInfo.point, Color.green);
            }
            else {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red);
            }
            return hit;
        }
    }
}
