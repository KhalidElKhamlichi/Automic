using System.Collections.Generic;
using UnityEngine;

namespace Automic.Common {
   public class Projectile : MonoBehaviour
   {
      [SerializeField] List<string> tagsToIgnore;
      [SerializeField] GameObject impactVFX;

      private void OnTriggerEnter(Collider other) {
         if (!tagsToIgnore.Contains(other.tag)) {
            if (impactVFX != null) Instantiate(impactVFX, transform.position, impactVFX.transform.rotation);
            Destroy(gameObject);
         }
      }

      private void OnCollisionEnter(Collision other) {
         if (!tagsToIgnore.Contains(other.collider.tag)) {
            if (impactVFX != null) Instantiate(impactVFX, transform.position, impactVFX.transform.rotation);
            Destroy(gameObject);
         }
      }
   }
}
