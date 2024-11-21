using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupPanel : MonoBehaviour
{
    public GameObject popupPanel;
    public TMP_Text popupText;
    public Button closeButton;

    private void Start()
    {
        popupPanel.SetActive(false);

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HidePopup);
        }
    }

    public void ShowPopup(string message)
    {
        popupText.text = message;
        popupPanel.SetActive(true);
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}