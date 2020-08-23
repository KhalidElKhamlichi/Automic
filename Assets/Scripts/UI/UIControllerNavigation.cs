using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerNavigation : MonoBehaviour {
    
    
    private GameObject[] buttons;
    private int selectedBtnIndex = -1;
    private GameObject currentSelectedBtn;
    private TextMeshProUGUI recordTimeText;
    void Start() {
        buttons = GameObject.FindGameObjectsWithTag("Button");
        recordTimeText = gameObject.transform.Find("Record time").GetComponent<TextMeshProUGUI>();
        string recordTime = "N/A";
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Record time")))
            recordTime = PlayerPrefs.GetString("Record time");
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
        currentSelectedBtn.GetComponent<ButtonEvents>().OnPointerEnter(null);
    }
    private void unselectButton() {
        currentSelectedBtn?.GetComponent<Button>().OnPointerExit(null);
        currentSelectedBtn?.GetComponent<ButtonEvents>().OnPointerExit(null);
    }
}
