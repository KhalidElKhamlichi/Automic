using Automic.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Automic.UI {
    public class ButtonAudio : MonoBehaviour, IPointerEnterHandler {
        
        [SerializeField] GameObject audioPlayer;
        [SerializeField] Audio onHoverAudio;
        [SerializeField] Audio onClickAudio;
        
        private void Start() {
            GetComponent<Button>().onClick.AddListener(playOnClickAudio);
        }
        
        private void playOnClickAudio() {
            Instantiate(audioPlayer).GetComponent<AudioPlayer>().play(onClickAudio);
        }
        
        public void OnPointerEnter(PointerEventData eventData) {    
            Instantiate(audioPlayer).GetComponent<AudioPlayer>().play(onHoverAudio);
        }
   
    }
}