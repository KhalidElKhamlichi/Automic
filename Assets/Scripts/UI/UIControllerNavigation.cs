using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Automic.UI {
    public class UIControllerNavigation : MonoBehaviour {
        private const string BUTTON_TAG = "Button";
        private const string RECORD_TIME = "Record time";
        private GameObject[] buttons;
        private int selectedBtnIndex = -1;
        private GameObject currentSelectedBtn;
        private TextMeshProUGUI recordTimeText;
        void Start() {
            buttons = GameObject.FindGameObjectsWithTag(BUTTON_TAG);
            recordTimeText = gameObject.transform.Find(RECORD_TIME).GetComponent<TextMeshProUGUI>();
            string recordTime = "N/A";
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(RECORD_TIME)))
                recordTime = PlayerPrefs.GetString(RECORD_TIME);
            recordTimeText.text += recordTime;
        }

        void Update() {
            if(Input.GetButton("Submit")) currentSelectedBtn.GetComponent<Button>().onClick.Invoke();
            if (Input.GetAxis("DPad vertical") == 1) {
                unselectButton();
                selectedBtnIndex++;
                selectButton();
            }
            if (Input.GetAxis("DPad vertical") == -1) {
                unselectButton();
                selectedBtnIndex--;
                selectButton();
            }
        }

        private void selectButton() {
            selectedBtnIndex = Mathf.Clamp(selectedBtnIndex, 0, buttons.Length - 1);
            currentSelectedBtn = buttons[selectedBtnIndex];
            currentSelectedBtn.GetComponent<Button>().OnPointerEnter(null);
            currentSelectedBtn.GetComponent<ButtonView>().OnPointerEnter(null);
        }
        private void unselectButton() {
            currentSelectedBtn?.GetComponent<Button>().OnPointerExit(null);
            currentSelectedBtn?.GetComponent<ButtonView>().OnPointerExit(null);
        }
    }
}
