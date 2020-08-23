using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour, Damager {
    public int damage;

    public int getDamage() {
        return damage;
    }
}
