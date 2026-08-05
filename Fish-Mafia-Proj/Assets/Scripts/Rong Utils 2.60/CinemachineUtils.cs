using Unity.Cinemachine;
using UnityEngine;

public static class CinemachineUtils
{
    /// <summary>
    /// เปลี่ยนเป้าหมายการมองและติดตามพร้อมกัน
    /// </summary>
#if CINEMACHINE_v3
    public static void ChangeTarget(this CinemachineCamera vcam, Transform newTarget)
    {
        if (vcam == null) return;
        var target = vcam.Target;
        target.TrackingTarget = newTarget;
        target.LookAtTarget = newTarget;
        vcam.Target = target;
        vcam.UpdateTargetCache();
    }
#else
    public static void ChangeTarget(this CinemachineCamera vcam, Transform newTarget)
    {
        if (vcam == null) return;
        vcam.Follow = newTarget;
        vcam.LookAt = newTarget;
    }
#endif

    /// <summary>
    /// เปลี่ยนเฉพาะเป้าหมายการเคลื่อนที่ตาม (Follow)
    /// </summary>
#if CINEMACHINE_v3
    public static void SetFollowTarget(this CinemachineCamera vcam, Transform newTarget)
    {
        if (vcam == null) return;
        var target = vcam.Target;
        target.TrackingTarget = newTarget;
        vcam.Target = target;
        vcam.UpdateTargetCache();
    }
#else
    public static void SetFollowTarget(this CinemachineCamera vcam, Transform newTarget)
    {
        if (vcam == null) return;
        vcam.Follow = newTarget;
    }
#endif

    /// <summary>
    /// เปลี่ยนเฉพาะเป้าหมายการหันมอง (LookAt)
    /// </summary>
#if CINEMACHINE_v3
    public static void SetLookAtTarget(this CinemachineCamera vcam, Transform newTarget)
    {
        if (vcam == null) return;
        var target = vcam.Target;
        target.LookAtTarget = newTarget;
        vcam.Target = target;
        vcam.UpdateTargetCache();
    }
#else
    public static void SetLookAtTarget(this CinemachineCamera vcam, Transform newTarget)
    {
        if (vcam == null) return;
        vcam.LookAt = newTarget;
    }
#endif

    /// <summary>
    /// ล้างเป้าหมายทั้งหมดออกจากกล้อง
    /// </summary>
#if CINEMACHINE_v3
    public static void ClearTargets(this CinemachineCamera vcam)
    {
        if (vcam == null) return;
        var target = vcam.Target;
        target.TrackingTarget = null;
        target.LookAtTarget = null;
        vcam.Target = target;
        vcam.UpdateTargetCache();
    }
#else
    public static void ClearTargets(this CinemachineCamera vcam)
    {
        if (vcam == null) return;
        vcam.Follow = null;
        vcam.LookAt = null;
    }
#endif
}