using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    private AudioSource audioSource;
    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void play(Audio audio) {
        audioSource.clip = audio.clip;
        audioSource.volume = audio.volume;
        audioSource.pitch = audio.pitch;
        audioSource.Play();
    }
}
