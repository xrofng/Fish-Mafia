using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomationFollow : Automation
{
    public enum FollowMoveMode
    {
        SmoothLerp,
        Constant,
        ByTime
    }
    [FoldoutGroup("Property")]
    public FollowMoveMode Mode = FollowMoveMode.SmoothLerp;
    [FoldoutGroup("Target")]
    public Transform FollowingTarget;
    private Transform _prevFollowingTarget;
    [FoldoutGroup("Target")]
    public Vector3 FollowingOffset;

    [FoldoutGroup("Property")]
    [ShowIf("HasSpeedProperty")]
    public float Speed = 7;
    public bool HasSpeedProperty => Mode <= FollowMoveMode.Constant;

    [FoldoutGroup("Property")]
    [ShowIf("HasDurationProperty")]
    public float Duration = 7;
    public bool HasDurationProperty => Mode == FollowMoveMode.ByTime;

    [FoldoutGroup("Property")]
    [ShowIf("HasDirectionalMultiplierProperty")]
    public Vector3 DirectionalMultiplier = Vector3.one;
    public bool HasDirectionalMultiplierProperty => Mode == FollowMoveMode.SmoothLerp;

    private Vector3 _startPos;
    private Vector3 _nextPosition;
    private Vector3 _targetPos;
    private float _timer;

    protected override bool CheckAutomationProcessCondition()
    {
        return FollowingTarget;
    }

    protected override void ProcessAutomation()
    {
        base.ProcessAutomation();

        _targetPos = FollowingTarget.position + FollowingOffset;
        if (Mode == FollowMoveMode.SmoothLerp)
        {
            _nextPosition = transform.position;

            _nextPosition.x = Mathf.Lerp(_nextPosition.x, _targetPos.x, Time.deltaTime * Speed * DirectionalMultiplier.x);
            _nextPosition.y = Mathf.Lerp(_nextPosition.y, _targetPos.y, Time.deltaTime * Speed * DirectionalMultiplier.y);
            _nextPosition.z = Mathf.Lerp(_nextPosition.z, _targetPos.z, Time.deltaTime * Speed * DirectionalMultiplier.z);
            transform.position = _nextPosition;
        }
        else if (Mode == FollowMoveMode.Constant)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, Speed * Time.deltaTime);
        }
        else if (Mode == FollowMoveMode.ByTime)
        {
            _timer += Time.deltaTime;
            transform.position = Vector3.Lerp(_startPos, _targetPos, _timer / Duration);
        }
    }

    protected override void OnStarted()
    {
        base.OnStarted();

        // For FollowMoveMode.ByTime
        _timer = 0;
        _startPos = transform.position;
    }

    public void InstantMoveToTarget()
    {
        transform.position = FollowingTarget.position;
    }

    public void UnassingTarget()
    {
        SetTarget(null);
    }

    public void SetTarget(Transform t)
    {
        _prevFollowingTarget = FollowingTarget;
        FollowingTarget = t;
    }

    public Vector3 GetDirectionToTarget()
    {
        return FollowingTarget.position - transform.position;
    }

    public void AddOffset(Vector3 increment)
    {
        FollowingOffset += increment;
    }
}

