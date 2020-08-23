using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifecycle : Lifecycle
{
    private GameObject leftWing;
    private GameObject rightWing;
    protected override void Start() {
        base.Start();
        leftWing = transform.GetChild(0).GetChild(2).gameObject;
        rightWing = transform.GetChild(0).GetChild(3).gameObject;
    }

    protected override void takeDamage(Collision other) {
        base.takeDamage(other);
        destroyWing();
    }

    protected override void onDeath() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void destroyWing() {
        GameObject wingToDestroy = leftWing == null ? rightWing : leftWing;
        Destroy(wingToDestroy);
    }
}
