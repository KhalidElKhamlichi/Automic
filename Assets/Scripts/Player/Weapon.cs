
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(WeaponSFX))]
public class Weapon : MonoBehaviour {
    public List<ProjectilePool> projectilePools;
    public float projectileSpeed;
    public float firerate;
    public int nbrOfSimultaneousProjectiles;
    public float spreadAngle;
    public float randomSpreadMax;

    private float timer;
    private WeaponSFX weaponSfx;
    private GameObject currentProjectile;
    public List<Stack<GameObject>> projectileStacks = new List<Stack<GameObject>>();

    private void OnValidate() {
        // if (nbrOfSimultaneousProjectiles > projectilePools.Count) {
        //     Debug.LogWarning("nbrOfSimultaneousProjectiles should not exceed the number of projectile pools");
        //     nbrOfSimultaneousProjectiles = projectilePools.Count;
        // }
    }

    void Start() {
        //timer = 1 / firerate;
        weaponSfx = GetComponent<WeaponSFX>();
        foreach (ProjectilePool projectilePool in projectilePools) {
            projectileStacks.Add(new Stack<GameObject>(projectilePool.projectiles));
        }
    }

    public bool fire(Transform target = null) {
        timer -= Time.deltaTime;
        if(timer <= 0) {
            instantiateProjectiles(target);
            weaponSfx.play();
            resetTimer();
            return true;
        }

        return false;
    }

    private void instantiateProjectiles(Transform target) {
        float spread = 0;
        
        for (int i = 0; i < nbrOfSimultaneousProjectiles; i++) {
            if(projectileStacks[i].Count == 0) projectileStacks[i] = new Stack<GameObject>(projectilePools[i].projectiles);
            currentProjectile = projectileStacks[i].Pop();
            // Increment for every other extra projectile
            if (i % 2 != 0) {
                spread += Mathf.Sign(spread) * spreadAngle;
            }

            // permute between each side
            spread *= -1;
            spread += Random.Range(-randomSpreadMax, randomSpreadMax);
            instantiateProjectile(target, transform.rotation, spread);
        }
    }

    private void instantiateProjectile(Transform target, Quaternion rotation, float spread) {
        GameObject projectileClone = Instantiate(currentProjectile, transform.position, rotation);
        if (target) {
            projectileClone.transform.LookAt(target);
        }
        // rotate weapon
        transform.Rotate(0, spread, 0);
        Rigidbody rbody = projectileClone.GetComponentInChildren<Rigidbody>() ?? projectileClone.GetComponent<Rigidbody>();
        // float random = Random.Range(1.1f, 1.4f);
        rbody.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        // reset rotation
        transform.Rotate(0, -spread, 0);
    }

    private void resetTimer() {
        timer = 1/firerate;
    }
    [Serializable]
    public class ProjectilePool {
        public List<GameObject> projectiles;
    }
}
