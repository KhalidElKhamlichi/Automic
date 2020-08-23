using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Lifecycle))]
[RequireComponent(typeof(CollisionManager))]
public class EnemyVFX : MonoBehaviour
{
    public GameObject impactVFX;
    public GameObject hitVFX;
    public GameObject deathVfx;
    public GameObject spawnVFX;
    public Material transparentMaterial;
    public float hitVFXFrequency;
    
    private float currentHitVFXFrequency;
    private Material originalMaterial;
    private Renderer renderer;

    private void OnEnable() {
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;
        if (spawnVFX != null) {
            Instantiate(spawnVFX, transform.position, spawnVFX.transform.rotation);
        }

        if (transparentMaterial != null) {
            renderer.material = transparentMaterial;
            StartCoroutine("fadeIn");
        }
    }

    void Start() {
        GetComponent<CollisionManager>().onHit(playOnHitVFX); 
        GetComponent<Lifecycle>().onDeath(playOnDeathVFX);
        currentHitVFXFrequency = hitVFXFrequency;
    }

    private void Update() {
        currentHitVFXFrequency -= Time.deltaTime;
    }

    private IEnumerator fadeIn() {
        float alpha = 0;
        Color color = renderer.material.GetColor("_BaseColor");
        while (alpha < 1) {
            alpha = Mathf.Lerp(alpha, 1.1f, Time.deltaTime * 1.2f);
            color.a = alpha;
            renderer.material.SetColor("_BaseColor", color);
            yield return null;
        }
        renderer.material = originalMaterial;
    }
    private void playOnHitVFX(Collision other) {
        if(hitVFX) instantiateHitVFX();
        if(impactVFX) instantiateImpact(other);
    }

    private void instantiateHitVFX() {
        Transform gameObjectTransform = transform;
        if (currentHitVFXFrequency <= 0) {
            Instantiate(hitVFX, gameObjectTransform.position, Quaternion.identity, gameObjectTransform);
            currentHitVFXFrequency = hitVFXFrequency;
        }
    }    

    private void instantiateImpact(Collision other) {
        GameObject impact = Instantiate(impactVFX, other.contacts[0].point, Quaternion.identity);
        impact.transform.LookAt(other.transform.position);
        impact.GetComponent<VisualEffect>().Play();
        Destroy(impact, 1);
    }

    private void playOnDeathVFX() {
        if(deathVfx) Instantiate(deathVfx, transform.position, deathVfx.transform.rotation);
        Camera.main.GetComponent<CameraShake>().shakeCamera();
    }
}
