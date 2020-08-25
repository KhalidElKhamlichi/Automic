using Automic.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Automic.UI {
    public class ButtonEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        public Color defaultTextColor;
        public Color onHoverTextColor;
        public GameObject audioPlayer;
        public Audio onHoverAudio;
        public Audio onClickAudio;

        private void Start() {
            GetComponent<Button>().onClick.AddListener(playOnClickAudio);
        }

   

        private void playOnClickAudio() {
            Instantiate(audioPlayer).GetComponent<AudioPlayer>().play(onClickAudio);
        }

        public void OnPointerEnter(PointerEventData eventData) {    
            GetComponentInChildren<TextMeshProUGUI>().color  = onHoverTextColor;
            Instantiate(audioPlayer).GetComponent<AudioPlayer>().play(onHoverAudio);
        }


        public void OnPointerExit(PointerEventData eventData) {
            GetComponentInChildren<TextMeshProUGUI>().color  = defaultTextColor;
        }
    
    }
}
