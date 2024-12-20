using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupPanel : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TMP_Text popupText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private bool _actionTaken;

    // Show a popup without buttons, auto-hides after a set duration
    public void ShowTimedPopup(string message, float duration = 2f)
    {
        if (popupPanel == null || popupText == null)
        {
            Debug.LogError("PopupPanel or PopupText is not assigned.");
            return;
        }

        popupText.text = message;
        popupPanel.SetActive(true);
        _actionTaken = true; // No user action required for timed popups

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        // Automatically hide the popup after the specified duration
        Invoke(nameof(HidePopup), duration);
    }

    // Show a popup with optional buttons and a timeout
    public void ShowPopupWithButtons(
        string message,
        System.Action onYes = null,
        System.Action onNo = null,
        float timeout = -1f)
    {
        if (popupPanel == null || popupText == null || yesButton == null || noButton == null)
        {
            Debug.LogError("PopupPanel or required components are not assigned.");
            return;
        }

        popupText.text = message;
        popupPanel.SetActive(true);
        _actionTaken = false;

        // Show or hide the "Yes" button
        if (onYes != null)
        {
            yesButton.gameObject.SetActive(true);
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(() =>
            {
                _actionTaken = true;
                onYes.Invoke();
                HidePopup();
            });
        }
        else
        {
            yesButton.gameObject.SetActive(false);
        }

        if (onNo != null)
        {
            noButton.gameObject.SetActive(true);
            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(() =>
            {
                _actionTaken = true;
                onNo.Invoke();
                HidePopup();
            });
        }
        else
        {
            noButton.gameObject.SetActive(false);
        }

        if (timeout > 0)
        {
            Invoke(nameof(HandleTimeout), timeout);
        }
    }

    private void HandleTimeout()
    {
        if (!_actionTaken)
        {
            _actionTaken = true;
            HidePopup();
        }
    }

    public void HidePopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        CancelInvoke(nameof(HandleTimeout)); // Prevent timeout action if popup is closed
    }
}
