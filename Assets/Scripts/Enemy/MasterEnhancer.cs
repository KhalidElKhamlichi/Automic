using System.Collections.Generic;
using Automic.Common;
using Automic.Player;
using Automic.Sound;
using PathCreation.Examples;
using UnityEngine;

namespace Automic {
    public class MasterEnhancer : MonoBehaviour
    {
        [SerializeField] List<Lifecycle> shieldLifeSources;
        [SerializeField] List<Lifecycle> seals;
        [SerializeField] float firerateMultiplierOnSealBreak;
        [SerializeField] GameObject deathVFX;
        [SerializeField] bool followPath;
        [SerializeField] Audio shieldBreakAudio;
        [SerializeField] GameObject audioPlayer;

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
}
