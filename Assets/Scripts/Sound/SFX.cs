using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CollisionManager))]
public class SFX : MonoBehaviour {
    public Audio deathAudio;
    public Audio hitAudio;
    public GameObject audioPlayer;
    
    void Start() {
        GetComponent<CollisionManager>().onHit(playHit);
        GetComponent<Lifecycle>()?.onDeath(playDeath);
    }

    private void playHit(Collision other) {
        GameObject audioPlayerInstance = Instantiate(audioPlayer);
        audioPlayerInstance.GetComponent<AudioPlayer>().play(hitAudio);
    }

    private void playDeath() {
        GameObject audioPlayerInstance = Instantiate(audioPlayer);
        audioPlayerInstance.GetComponent<AudioPlayer>().play(deathAudio);
    }
}
