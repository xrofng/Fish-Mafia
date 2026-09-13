using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FloorManager))]
public class FloorFinishingController : BetterMonoBehaviour, IEventSubcriber<FloorController.EvsAllEnemiesDied>
{
    [Title("Move during Transition Setting")]
    public float MoveDuration = 0.5f;
    public AnimationCurve MoveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private FloorManager floorManager;
    private PlayerStateMachine player;
    private bool isTransitioning;

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out floorManager);
        player = FindAnyObjectByType<PlayerStateMachine>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBusRegister.EventBusSubcribe<FloorController.EvsAllEnemiesDied>(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBusRegister.EventBusUnscribe<FloorController.EvsAllEnemiesDied>(this);
    }

    private void GoToUpperFloor()
    {
        Vector3 targetPos = floorManager.GetUpperFloor().GetLowestMidCenterPos(player.transform.position);
        StartCoroutine(TransitionToLevel(targetPos));
    }

    IEnumerator TransitionToLevel(Vector3 targetPos)
    {
        isTransitioning = true;

        player.TransitionToState(PlayerStateMachine.EPlayerState.CantControlled);

        yield return MovePlayer(targetPos, player.transform);

        isTransitioning = false;
    }

    IEnumerator MovePlayer(Vector3 targetPos, Transform mover)
    {
        Vector3 start = mover.position;
        float time = 0f;

        while (time < MoveDuration)
        {
            time += Time.deltaTime;
            float t = time / MoveDuration;
            float eval = MoveCurve.Evaluate(t);

            mover.position = Vector3.Lerp(start, targetPos, eval);
            yield return null;
        }

        mover.position = targetPos;
    }

    public void OnEventBusTrigger(FloorController.EvsAllEnemiesDied eventType)
    {
        GoToUpperFloor();
    }
}
