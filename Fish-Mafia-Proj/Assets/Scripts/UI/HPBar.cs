using MoreMountains.Tools;
using UnityEngine;

public class HPBar : BetterMonoBehaviour, IEventSubcriber<Health.EvsPlayerHPChanged>
{
    public MMProgressBar ProgressBar;

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

    protected override void Start()
    {
        base.Start();
    }

    public void OnEventBusTrigger(Health.EvsPlayerHPChanged eventType)
    {
        ProgressBar.UpdateBar01((float)eventType.CurrentHP / (float)eventType.MaxHP);
    }
}
