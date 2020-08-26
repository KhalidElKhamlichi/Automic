using Automic.Common;
using UnityEngine;

namespace Automic {
    [RequireComponent(typeof(BasicController))]
    [RequireComponent(typeof(CollisionManager))]
    public class Seeker : MonoBehaviour {
        [SerializeField] Transform target;
        [SerializeField] LayerMask collisionMask;
        [SerializeField] int rayCastMaxDistance = 4;

        private const string TARGET_TAG = "Player";
        private const float RAY_CAST_OFFSET = .85f;
        private BasicController basicController;
        private Rigidbody rigidbody;
        private RigidbodyConstraints originalRigidbodyConstraints;

        void Start() {
            basicController = GetComponent<BasicController>();
            CollisionManager collisionManager = GetComponent<CollisionManager>() ?? GetComponentInChildren<CollisionManager>();
            collisionManager.onHit(temporaryFreeze);
            rigidbody = GetComponent<Rigidbody>();
            originalRigidbodyConstraints = rigidbody.constraints;
            if (target == null) target = GameObject.FindGameObjectWithTag(TARGET_TAG).transform;
        }

        private void temporaryFreeze(Collision obj) {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            Invoke("unfreeze", .1f);
        }

        private void unfreeze() {
            rigidbody.constraints = originalRigidbodyConstraints;
        }

        void FixedUpdate() {
            Vector3 targetPosition = target.position;
            Vector3 transformPosition = transform.position;
            Vector3 transformForward = transform.forward;
            Vector3 transformRight = transform.right;

            Vector2 direction = new Vector2(targetPosition.x - transformPosition.x, targetPosition.z - transformPosition.z);
            float rotationAngle = calculateRotationAngle(transformForward, direction, transformRight);

            basicController.rotate(rotationAngle);
            basicController.move(new Vector2(transformForward.x, transformForward.z));
        }

        private float calculateRotationAngle(Vector3 transformForward, Vector2 direction, Vector3 transformRight) {
            float rotationAngleToTarget = Vector3.SignedAngle(transformForward, new Vector3(direction.x, 0, direction.y), Vector3.up);

            Vector3 offset = transformRight * RAY_CAST_OFFSET - transformForward * RAY_CAST_OFFSET;
            bool rightRay = castRay(offset, transformForward);
            bool leftRay = castRay(-offset, transformForward);
            if (rightRay && leftRay) {
                temporaryFreeze(null);
                return rotationAngleToTarget;
            }
            if (rightRay) rotationAngleToTarget = -10;
            if (leftRay) rotationAngleToTarget = 10;
            return rotationAngleToTarget;
        }

        private bool castRay(Vector3 offset, Vector3 direction) {
            Ray ray = new Ray(transform.position + offset, direction);

            bool hit = Physics.Raycast(ray, out var hitInfo, rayCastMaxDistance, collisionMask);
            if (hit) {
                Debug.DrawLine(ray.origin, hitInfo.point, Color.green);
            }
            else {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * rayCastMaxDistance, Color.red);
            }
            return hit;
        }
    }
}
