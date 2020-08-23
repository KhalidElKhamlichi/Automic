using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathVFX : MonoBehaviour {

    public GameObject VFX;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void play() {
        GameObject VFXClone = Instantiate(VFX, transform.position, VFX.transform.rotation);
        // Destroy(VFXClone, .7f);
    }
}
