using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BlockerVFX : MonoBehaviour
{
    public GameObject impactVFX;
    void Start() {
        GetComponent<CollisionManager>().onHit(instantiateImpact); 
    }

    private void instantiateImpact(Collision other) {
        GameObject impact = Instantiate(impactVFX, other.contacts[0].point, Quaternion.identity);
        impact.transform.LookAt(other.transform.position);
        impact.GetComponent<VisualEffect>().Play();
        Destroy(impact, 1);
    }
}
