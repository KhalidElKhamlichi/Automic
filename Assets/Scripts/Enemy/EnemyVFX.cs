using System.Collections;
using Automic.Camera;
using Automic.Common;
using UnityEngine;
using UnityEngine.VFX;

namespace Automic {
    [RequireComponent(typeof(Lifecycle))]
    [RequireComponent(typeof(CollisionManager))]
    public class EnemyVFX : MonoBehaviour
    {
        [SerializeField] GameObject impactVFX;
        [SerializeField] GameObject hitVFX;
        [SerializeField] GameObject deathVfx;
        [SerializeField] GameObject spawnVFX;
        [SerializeField] Material transparentMaterial;
        [SerializeField] float hitVFXFrequency;
    
        private float hitVFXTimer;
        private Material originalMaterial;
        private Renderer renderer;

        private void OnEnable() {
            renderer = GetComponent<Renderer>();
            originalMaterial = renderer.material;
            if (transparentMaterial != null && spawnVFX != null) {
                startSpawnVFX();
            }
        }

        private void startSpawnVFX() {
            Instantiate(spawnVFX, transform.position, spawnVFX.transform.rotation);
            renderer.material = transparentMaterial;
            StartCoroutine("fadeIn");
        }

        void Start() {
            GetComponent<CollisionManager>().onHit(playOnHitVFX); 
            GetComponent<Lifecycle>().onDeath(playOnDeathVFX);
            hitVFXTimer = 1/hitVFXFrequency;
        }

        private void Update() {
            hitVFXTimer -= Time.deltaTime;
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
            if (hitVFXTimer <= 0) {
                Instantiate(hitVFX, gameObjectTransform.position, hitVFX.transform.rotation, gameObjectTransform);
                hitVFXTimer = 1/hitVFXFrequency;
            }
        }    

        private void instantiateImpact(Collision other) {
            GameObject impact = Instantiate(impactVFX, other.contacts[0].point, Quaternion.identity);
            impact.transform.LookAt(other.transform.position);
            Destroy(impact, 1);
        }

        private void playOnDeathVFX() {
            if(deathVfx) Instantiate(deathVfx, transform.position, deathVfx.transform.rotation);
            UnityEngine.Camera.main.GetComponent<CameraShake>().shakeCamera();
        }
    }
}
