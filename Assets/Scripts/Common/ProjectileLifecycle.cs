using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLifecycle : MonoBehaviour
{
   public List<string> tagsToIgnore;
   public GameObject onDeathVFX;

   private void OnTriggerEnter(Collider other) {
      if (!tagsToIgnore.Contains(other.tag)) {
         if (onDeathVFX != null) Instantiate(onDeathVFX, transform.position, onDeathVFX.transform.rotation);
         Destroy(gameObject);
      }
   }

   private void OnCollisionEnter(Collision other) {
      if (!tagsToIgnore.Contains(other.collider.tag)) {
         if (onDeathVFX != null) Instantiate(onDeathVFX, transform.position, onDeathVFX.transform.rotation);
         Destroy(gameObject);
      }
   }
}
