using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Xrofng;
using static Unity.Cinemachine.RequiredTargetAttribute;

public class PlayerTargeting : BetterMonoBehaviour, IEventSubcriber<Health.EvsEnemyDied>
{
    public Vector2 detectionBoxSize;   
    public float clickRadius = 0.5f;   
    public LayerMask enemyLayer;
    public Transform currentTarget;
    public Transform VoidTarget { get; private set; }

    private Vector3 _lastInteractPoint;


    protected override void Awake()
    {
        base.Awake();
        VoidTarget = Instantiate(new GameObject(), transform).transform;
        VoidTarget.name = "void_target";
    }

    protected override void Update()
    {
        base.Update();
        Vector2 clickPoint = GetMouseWorldPos();
        _lastInteractPoint = (Vector3)clickPoint;

        Collider2D[] hits = Physics2D.OverlapCircleAll(clickPoint, clickRadius, enemyLayer);

        if (hits.Length == 0)
        {
            CursorController.Instance.SetState("Neutral");
        }
        else
        {
            CursorController.Instance.SetState("Detect Enemy");
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBusRegister.EventBusSubcribe(this);
    }

    protected override void OnDisable()
    {
        base.OnEnable();
        EventBusRegister.EventBusUnscribe(this);
    }

    Vector2 GetMouseWorldPos()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreen);

        Plane plane = new Plane(Vector3.forward, Vector3.zero); // Z = 0 plane

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector2.zero;
    }

    public bool FindTarget()
    {
        currentTarget = null;
        Vector2 clickPoint = GetMouseWorldPos();
        _lastInteractPoint = (Vector3)clickPoint;

        Collider2D[] hits = Physics2D.OverlapCircleAll(clickPoint, clickRadius, enemyLayer);

        if (hits.Length == 0)
        {
            return false;
        }

        // Find closest
        Collider2D closest = null;
        float closestDist = float.MaxValue;

        foreach (var h in hits)
        {
            float dist = (h.transform.position - (Vector3)clickPoint).sqrMagnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = h;
            }
        }

        currentTarget = closest.transform;
        return true;
    }

    public bool HasEnemyNearby()
    {
        Collider2D hit = Physics2D.OverlapBox(
            transform.position,
            detectionBoxSize,
            0, enemyLayer
        );
        
        return hit != null;
    }

    public void OnEventBusTrigger(Health.EvsEnemyDied eventType)
    {
        if (currentTarget != null)
        {
            if (eventType.KokonutStateMachine == currentTarget.GetComponentInParent<KokonutStateMachine>())
            {
                currentTarget = null;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, detectionBoxSize);

        if (!Application.isPlaying) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_lastInteractPoint, clickRadius);
    }

    public void DetermineVoidTargetPosition()
    {
        Vector3 dir = (_lastInteractPoint - transform.position).normalized;
        VoidTarget.transform.localPosition = new Vector3(dir.x, 1, 0);
    }

    public Transform GetTarget()
    {
        if (currentTarget)
        {
            return currentTarget;
        }
        DetermineVoidTargetPosition();
        return VoidTarget;
    }
}