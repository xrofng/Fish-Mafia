using MoreMountains.Feedbacks;
using UnityEngine;

public class KokonutCamera : BetterMonoBehaviour, IEventSubcriber<BasePlayerState.EvsPlayerStateEntered>
{
    public MMF_Player EnterEngageFB;
    public MMF_Player ExitEngageFB;

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

    public void OnEventBusTrigger(BasePlayerState.EvsPlayerStateEntered eventType)
    {
        if (eventType.EnteredState == PlayerStateMachine.EPlayerState.Field &&
            eventType.PrevState != PlayerStateMachine.EPlayerState.Field)
        {
            ExitEngageFB?.PlayFeedbacks();
        }
        else if (eventType.EnteredState > PlayerStateMachine.EPlayerState.Field &&
            eventType.PrevState == PlayerStateMachine.EPlayerState.Field)
        {
            EnterEngageFB?.PlayFeedbacks();
        }
    }
}
