using UnityEngine;

namespace Automic.Common {
    public class Rotator : MonoBehaviour
    {
        [SerializeField] float rotationSpeed;
    
        private Rigidbody rigidbody;

        void Start() {
            rigidbody = GetComponent<Rigidbody>();   
        }

        void FixedUpdate() {
            Vector3 currentRotation = rigidbody.rotation.eulerAngles;
            rigidbody.rotation = Quaternion.Euler(currentRotation + new Vector3(0f, rotationSpeed * Time.deltaTime, 0f));
        }
    }
}
