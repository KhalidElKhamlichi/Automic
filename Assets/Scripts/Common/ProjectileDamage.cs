using UnityEngine;

namespace Automic.Common {
    public class ProjectileDamage : MonoBehaviour, Damager {
        public int damage;

        public int getDamage() {
            return damage;
        }
    }
}
