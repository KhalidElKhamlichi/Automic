﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public bool isShaking;
    
    public void shakeCamera(float duration = .1f, float magnitude = .2f) {
        if (!isShaking) {
            StartCoroutine(shake(duration, magnitude));
        }
    }
    
    private IEnumerator shake(float duration, float magnitude) {
        isShaking = true;
        Vector3 orignalPosition = transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;

            transform.position += new Vector3(x, y, z);
            elapsed += Time.deltaTime;
            yield return 0;
        }

        isShaking = false;
        transform.position = orignalPosition;
    }
}