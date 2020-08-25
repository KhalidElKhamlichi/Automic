using System;
using UnityEngine;

namespace Automic.Common {
    public class Lifecycle : MonoBehaviour {

        [SerializeField] int initialHP;
    
        private event Action deathEvent;
        private int currentHP;

        protected virtual void Start() {
            currentHP = initialHP;
            GetComponent<CollisionManager>().onHit(takeDamage);
        }

        public void onDeath(Action onDeath) {
            deathEvent += onDeath;
        }

        protected virtual void takeDamage(Collision other) {
            Damager damager = other.gameObject.GetComponent<Damager>();
            if (damager == null) return;
        
            currentHP -= damager.getDamage();
            checkLife();
        }

        private void checkLife() {
            if (currentHP <= 0) {
                deathEvent?.Invoke();
                onDeath();
            }
        }

        protected virtual void onDeath() {
            if (transform.parent != null) Destroy(transform.parent.gameObject);
            else Destroy(gameObject);
        }
    }
}
