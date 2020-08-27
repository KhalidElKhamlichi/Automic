using Automic.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Automic.UI {
    public class ButtonView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        
        [SerializeField] Color onHoverTextColor;
        [SerializeField] Color defaultTextColor;
        
        public void OnPointerEnter(PointerEventData eventData) {    
            GetComponentInChildren<TextMeshProUGUI>().color  = onHoverTextColor;
        }
        
        public void OnPointerExit(PointerEventData eventData) {
            GetComponentInChildren<TextMeshProUGUI>().color  = defaultTextColor;
        }
    
    }
}
