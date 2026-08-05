using MoreMountains.Feedbacks;
using Unity.Cinemachine;
using UnityEngine;

public class KokonutCamera : BetterMonoBehaviour, 
    IEventSubcriber<BasePlayerState.EvsPlayerStateEntered>,
    IEventSubcriber<FloorManager.EvsFloorChanged>
{
    public CinemachineCamera Cinemachine;
    public MMF_Player EnterEngageFB;
    public MMF_Player ExitEngageFB;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBusRegister.EventBusSubcribe<BasePlayerState.EvsPlayerStateEntered>(this);
        EventBusRegister.EventBusSubcribe<FloorManager.EvsFloorChanged>(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBusRegister.EventBusUnscribe<BasePlayerState.EvsPlayerStateEntered>(this);
        EventBusRegister.EventBusUnscribe<FloorManager.EvsFloorChanged>(this);
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

    public void OnEventBusTrigger(FloorManager.EvsFloorChanged eventType)
    {
        Cinemachine.ChangeTarget(eventType.NextFloor.transform);
    }
}
