using System.Collections;
using System.Collections.Generic;
using PathCreation.Examples;
using UnityEngine;

public class MasterEnhancer : MonoBehaviour
{
    public List<Lifecycle> shieldLifeSources;
    public List<Lifecycle> seals;
    public float firerateMultiplierOnSealBreak;
    public GameObject deathVFX;
    public bool followPath;
    public Audio shieldBreakAudio;
    public GameObject audioPlayer;

    void Start() {
        foreach (Lifecycle lifecycle in shieldLifeSources) {
            lifecycle.onDeath(checkShieldLife);
        }
        foreach (Lifecycle lifecycle in seals) {
            lifecycle.onDeath(checkSeal);
        }
    }

    private void checkSeal() {
        seals.RemoveAt(0);
        if (seals.Count == 0) {
            applyEnhancement();
        }
    }

    private void applyEnhancement() {
        if(firerateMultiplierOnSealBreak > 0) GetComponentInChildren<Weapon>().firerate *= firerateMultiplierOnSealBreak;
        if (followPath) GetComponent<PathFollower>().enabled = true;
    }

    private void checkShieldLife() {
        shieldLifeSources.RemoveAt(0);
        if (shieldLifeSources.Count == 0) {
            breakShield();
        }
    }

    private void breakShield() {
        Instantiate(deathVFX, transform.position, deathVFX.transform.rotation, transform.parent);
        Instantiate(audioPlayer).GetComponent<AudioPlayer>().play(shieldBreakAudio);
        Destroy(transform.GetChild(1).gameObject);
    }
}
