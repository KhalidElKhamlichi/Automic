using Automic.Common;
using UnityEngine;

namespace Automic.Player {
    [RequireComponent(typeof(BasicController))]
    public class AimAssist : MonoBehaviour {
        private const float RAY_CAST_OFFSET = 6.5f;

        [SerializeField, Range(0f, 10f)]
        float slowRotationSpeed;
        [SerializeField] LayerMask collisionMask;
    
        private BasicController controller;

        void Start() {
            controller = GetComponent<BasicController>();
        }

        void Update() {
            Vector3 offset = transform.right * RAY_CAST_OFFSET - transform.forward * RAY_CAST_OFFSET;
            if (castRay(Vector3.zero, transform.forward)
                || castRay(offset, transform.forward)
                || castRay(-offset, transform.forward)) {
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
