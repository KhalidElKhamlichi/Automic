using Automic.Common;
using UnityEngine;

namespace Automic.Player {
    public class PlayerInput : MonoBehaviour {
        [SerializeField] GameObject crosshair;
        [SerializeField] float cursorDepth;
    
        private BasicController basicController;
        private GameObject crosshairInstance;
        private Vector3 lastMousePos = Vector3.zero;
        private InputType inputType = InputType.KB;
        private UnityEngine.Camera camera;

        public void Start() {
            camera = UnityEngine.Camera.main;
            basicController = GetComponent<BasicController>();
            crosshairInstance = Instantiate(crosshair);
        }

        void FixedUpdate() {
            basicController.move(new Vector2(Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal")));
            
            Vector3 controllerDir = new Vector3(Input.GetAxis("RightVertical"), 0, Input.GetAxis("RightHorizontal"));
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.nearClipPlane + cursorDepth);
            detectInputType(controllerDir, mousePosition);
            
            if (inputType == InputType.CONTROLLER) {
                basicController.lookAt(controllerDir);
                crosshairInstance.SetActive(false);
            }

            if (inputType == InputType.KB) {
                crosshairInstance.SetActive(true);
                updateCrosshairPosition(mousePosition);
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    Vector2 crosshairDir = new Vector2(hit.point.x - transform.position.x, hit.point.z - transform.position.z);
                    float rotationAngleToTarget = Vector3.SignedAngle(transform.forward, new Vector3(crosshairDir.x, 0, crosshairDir.y), Vector3.up);
                    basicController.rotate(rotationAngleToTarget);
                }
            }
        }

        private void detectInputType(Vector3 controllerDir, Vector3 mousePosition) {
            if (controllerDir.magnitude >= .8f) {
                inputType = InputType.CONTROLLER;
            }
            if (!lastMousePos.Equals(mousePosition)) {
                lastMousePos = mousePosition;
                inputType = InputType.KB;
            }
        }

        private void Update() {
            Vector3 controllerDir = new Vector3(Input.GetAxis("RightVertical"), 0, Input.GetAxis("RightHorizontal"));
            if(Input.GetButton("Fire") || controllerDir.magnitude >= .8f)
                basicController.shoot();
        
        }

        private void updateCrosshairPosition(Vector3 position) {
            Vector3 mousePosition = camera.ScreenToWorldPoint(position);
            crosshairInstance.transform.localPosition = mousePosition;
        }

        enum InputType {
            CONTROLLER,
            KB
        }
    }
}
