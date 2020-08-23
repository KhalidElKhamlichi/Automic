using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed;
    
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void FixedUpdate() {
        Vector3 currentRotation = rigidbody.rotation.eulerAngles;
        rigidbody.rotation = Quaternion.Euler(currentRotation + new Vector3(0f, rotationSpeed * Time.deltaTime, 0f));
    }
}
