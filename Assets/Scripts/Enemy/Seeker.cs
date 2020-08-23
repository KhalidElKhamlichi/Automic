using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericController))]
[RequireComponent(typeof(CollisionManager))]
public class Seeker : MonoBehaviour {
    public Transform target;
    public LayerMask collisionMask;
    public int rayCastMaxDistance = 4;

    private GenericController genericController;
    private Rigidbody rigidbody;
    private RigidbodyConstraints rigidbodyConstraints;

    void Start() {
        genericController = GetComponent<GenericController>();
        CollisionManager collisionManager = GetComponent<CollisionManager>() ?? GetComponentInChildren<CollisionManager>();
        collisionManager.onHit(temporaryFreeze);
        rigidbody = GetComponent<Rigidbody>();
        rigidbodyConstraints = rigidbody.constraints;
        if (target == null) target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void temporaryFreeze(Collision obj) {
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        Invoke("unfreeze", .1f);
    }

    private void unfreeze() {
        rigidbody.constraints = rigidbodyConstraints;
    }

    void FixedUpdate() {
        Vector3 targetPosition = target.position;
        Vector3 transformPosition = transform.position;
        Vector3 transformForward = transform.forward;
        Vector3 transformRight = transform.right;

        Vector2 direction = new Vector2(targetPosition.x - transformPosition.x, targetPosition.z - transformPosition.z);
        float rotationAngle = calculateRotationAngle(transformForward, direction, transformRight);

        genericController.rotate(rotationAngle);
        genericController.move(new Vector2(transformForward.x, -transformForward.z));
    }

    private float calculateRotationAngle(Vector3 transformForward, Vector2 direction, Vector3 transformRight) {
        float rotationAngleToTarget = Vector3.SignedAngle(transformForward, new Vector3(direction.x, 0, direction.y), Vector3.up);
        
        bool rightRay = castRay(transformRight / 1.2f - transformForward / 1.2f, transformForward);
        bool leftRay = castRay(-transformRight / 1.2f - transformForward / 1.2f, transformForward);
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
