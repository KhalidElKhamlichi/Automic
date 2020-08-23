using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public GameObject crosshair;
    
    private GenericController genericController;
    private GameObject crosshairInstance;
    public float cursorDepth;
    private Vector3 lastMousePos = Vector3.zero;
    private InputType inputType = InputType.KB;

    public void Start() {
        genericController = GetComponent<GenericController>();
        crosshairInstance = Instantiate(crosshair);
    }

    void FixedUpdate() {
        genericController.move(new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")));
        // genericController.rotate(Input.GetAxis("Mouse X") );
        // controller input
        Vector3 direction = new Vector3(Input.GetAxis("RightVertical"), 0, Input.GetAxis("RightHorizontal"));
        if (direction.magnitude >= .8f) {
            genericController.lookAt(direction);
            crosshairInstance.SetActive(false);
            inputType = InputType.CONTROLLER;
        }
        // mouse input
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.nearClipPlane + cursorDepth);
        if (!lastMousePos.Equals(mousePosition)) {
            lastMousePos = mousePosition;
            inputType = InputType.KB;
        }

        if (inputType == InputType.KB) {
            crosshairInstance.SetActive(true);
            updateCrosshairPosition(mousePosition);
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                Vector2 direction2 =
                    new Vector2(hit.point.x - transform.position.x, hit.point.z - transform.position.z);
                float rotationAngleToTarget = Vector3.SignedAngle(transform.forward,
                    new Vector3(direction2.x, 0, direction2.y), Vector3.up);
                genericController.rotate(rotationAngleToTarget);
            }
        }

    }

    private void Update() {
        Vector3 direction = new Vector3(Input.GetAxis("RightVertical"), 0, Input.GetAxis("RightHorizontal"));
        if(Input.GetButton("Fire") || direction.magnitude >= .8f)
            genericController.shoot();
        
    }

    private void updateCrosshairPosition(Vector3 position) {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(position);
        crosshairInstance.transform.localPosition = mousePosition;
    }

    enum InputType {
        CONTROLLER,
        KB
    }
}
