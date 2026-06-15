using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;

public class BulletMPGauge : BetterMonoBehaviour, IEventSubcriber<BulletMana.EvsBulletManaChanged>
{
    public List<MMProgressBar> BulletWidgets;

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

    public void OnEventBusTrigger(BulletMana.EvsBulletManaChanged eventType)
    {
        int mana = eventType.NewMana;

        int fullChunks = mana / 10;
        float partial = (mana % 10) / 10f;

        for (int i = 0; i < BulletWidgets.Count; i++)
        {
            float targetFill;

            if (i < fullChunks)
            {
                targetFill = 1f; // full
            }
            else if (i == fullChunks)
            {
                targetFill = partial; // partial
            }
            else
            {
                targetFill = 0f; // empty
            }

            BulletWidgets[i].UpdateBar01(targetFill);
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }


}
