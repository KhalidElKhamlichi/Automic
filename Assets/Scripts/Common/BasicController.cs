using Automic.Player;
using UnityEngine;

namespace Automic.Common {
    [RequireComponent(typeof(Rigidbody))]
    public class BasicController : MonoBehaviour {
        [SerializeField, Range(0f, 10f)]
        float maxRotationSpeed;
        [SerializeField, Range(0f, 100f)]
        float maxSpeed = 10f;
        [SerializeField, Range(0f, 100f)]
        float acceleration = 10f;

        private float rotationSpeed;
        private Vector3 velocity;
        private Rigidbody rigidbody;
        private Weapon weapon;
        void Start() {
            rigidbody = GetComponent<Rigidbody>();
            weapon = GetComponentInChildren<Weapon>();
            rotationSpeed = maxRotationSpeed;
        }

        public void move(Vector2 direction) {
            direction = Vector2.ClampMagnitude(direction, 1f);
            Vector3 desiredVelocity = new Vector3(direction.x, 0f, direction.y) * maxSpeed;
		      
            float maxSpeedChange = acceleration * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        
            Vector3 displacement = velocity * Time.deltaTime;
            Vector3 newPosition = transform.localPosition + displacement;
            rigidbody.MovePosition(newPosition);
        }

        public void rotate(float rotationAngle) {
            if(rotationAngle == 0) return;
            rotationAngle = Mathf.Clamp(rotationAngle, -rotationSpeed, rotationSpeed);
            Vector3 currentRotation = rigidbody.rotation.eulerAngles;
            rigidbody.rotation = Quaternion.Euler(currentRotation + new Vector3(0f, rotationAngle, 0f));
        }

        public void shoot() => weapon?.fire();

        public void slowRotation(float rotationSpeed) => this.rotationSpeed = rotationSpeed;

        public void resetRotation() => rotationSpeed = maxRotationSpeed;

        public void lookAt(Vector3 direction) => transform.rotation = Quaternion.LookRotation(direction, transform.up);
        
    }
}
