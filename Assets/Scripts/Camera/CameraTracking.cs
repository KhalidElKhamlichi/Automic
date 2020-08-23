using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour {
    public float speed;
    public float horizontalOffset;

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
