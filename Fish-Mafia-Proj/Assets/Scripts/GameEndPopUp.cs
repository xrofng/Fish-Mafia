using TMPro;
using UnityEngine;
using Xrofng;

public class GameEndPopUp : BaseFadeView, IEventSubcriber<LevelController.EvsGameEnd>
{
    public TextMeshProUGUI BigText;
    public TextMeshProUGUI ButtonText;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBusRegister.EventBusSubcribe(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBusRegister.EventBusUnscribe(this);
    }

    public void OnEventBusTrigger(LevelController.EvsGameEnd eventType)
    {
        BigText.text = eventType.BigText;
        CanvasGroup.alpha = 1;
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.interactable = true;
        Invoke(nameof(Reload), 2);
    }

    public void Reload()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
