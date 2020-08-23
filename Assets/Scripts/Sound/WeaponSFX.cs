using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class WeaponSFX : MonoBehaviour
{
    private AudioSource audioSource;
    
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void play() {
        audioSource.Play();
    }
}
