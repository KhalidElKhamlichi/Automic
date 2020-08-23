using UnityEngine;

[System.Serializable]
public class Audio {
    public AudioClip clip;
    [Range(0, 1)]
    public float volume = .5f;
    [Range(-3, 3)]
    public float pitch = 1;

}