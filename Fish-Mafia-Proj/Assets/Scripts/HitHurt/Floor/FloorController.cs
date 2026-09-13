using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FloorSensor))]
public class FloorController : BetterMonoBehaviour, IEventSubcriber<Health.EvsEnemyDied>
{
    [FoldoutGroup("Obj Ref")]
    [SerializeField] private bool autoDetectEnemiesFromChildren = true;

    [FoldoutGroup("Obj Ref")]
    [HideIf(nameof(autoDetectEnemiesFromChildren))]
    [SerializeField] List<KokonutStateMachine> enemies = new();

    [FoldoutGroup("Obj Ref")]
    [SerializeField] BoxCollider2D AirStandCol;
    
    private int _enemiesLeft;
    private FloorSensor floorSensor;

    public IReadOnlyList<KokonutStateMachine> Enemies => enemies;

    public int ListId { get; internal set; }

    public struct EvsAllEnemiesDied
    {
        public FloorController ThisFloor;

        public EvsAllEnemiesDied(FloorController thisFloor)
        {
            ThisFloor = thisFloor;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out floorSensor);
        Initialize();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBusRegister.EventBusSubcribe<Health.EvsEnemyDied>(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBusRegister.EventBusUnscribe<Health.EvsEnemyDied>(this);
    }

    public void Initialize()
    {
        if (autoDetectEnemiesFromChildren)
        {
            enemies.Clear();
            ScanChildren();
        }
    }

    private void ScanChildren()
    {
        var found = GetComponentsInChildren<KokonutStateMachine>(true);

        foreach (var component in found)
        {
            enemies.Add(component);
        }
        _enemiesLeft = enemies.Count;
    }

    public void SetFloorActive(bool active, bool setAirStand = false)
    {
        foreach (KokonutStateMachine enemy in enemies)
        {
            enemy.TransitionToState(active ? KokonutStateMachine.EEnemyState.Surround : KokonutStateMachine.EEnemyState.Pause);
        }
        if (setAirStand)
        {
            AirStandCol.enabled = active;
        }
    }

    public Vector3 GetLowestPoint()
    {
        Vector3 lowest = transform.position;
        lowest.y -= floorSensor.GetColliderSize().y / 2;
        return lowest;
    }

    public Vector3 GetRightestPoint()
    {
        Vector3 rightest = transform.position;
        rightest.x += floorSensor.GetColliderSize().y / 2;
        return rightest;
    }

    public void OnEventBusTrigger(Health.EvsEnemyDied eventType)
    {
        if (Enemies.Contains(eventType.KokonutStateMachine))
        {
            _enemiesLeft -= 1;
            if (_enemiesLeft <= 0)
            {
                EventBus.TriggerEvent(new EvsAllEnemiesDied(this));
            }
        }
    }

    public Vector3 GetLowestMidCenterPos(Vector3 playerPosition)
    {
        Vector3 targetPos = GetLowestPoint();
        if (Mathf.Abs(playerPosition.x) - Mathf.Abs(transform.position.x) > GetRightestPoint().x)
        {
            targetPos.x = Mathf.Lerp(transform.position.x, GetRightestPoint().x, .75f);
        }
        else
        {
            targetPos.x = playerPosition.x;
        }
        return targetPos;
    }
}
