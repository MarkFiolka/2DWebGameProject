using System;

public class PopupService
{
    private readonly PopupPanel _popupPanel;

    public PopupService(PopupPanel popupPanel)
    {
        if (popupPanel == null) throw new ArgumentNullException(nameof(popupPanel));
        _popupPanel = popupPanel;
    }

    public void ShowTimed(string message, float duration = 2f)
    {
        _popupPanel.ShowTimedPopup(message, duration);
    }

    public void ShowWithButtons(
        string message,
        Action onYes = null,
        Action onNo = null,
        float timeout = -1f)
    {
        _popupPanel.ShowPopupWithButtons(message, onYes, onNo, timeout);
    }

    public void Hide()
    {
        _popupPanel.HidePopup();
    }
}