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
        Debug.Log($"Popup message triggered: {message}");

        if (popupPanel == null || popupText == null)
        {
            Debug.LogError("PopupPanel or PopupText is not assigned.");
            return;
        }

        popupPanel.SetActive(false);

        popupText.text = message;
        popupPanel.SetActive(true);

        Debug.Log("PopupPanel is now active with updated content.");
    }



    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}