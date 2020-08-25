using UnityEngine;

namespace Automic.Common {
    public class ProjectileDamage : MonoBehaviour, Damager {
        [SerializeField] int damage;

        public int getDamage() {
            return damage;
        }
    }
}
