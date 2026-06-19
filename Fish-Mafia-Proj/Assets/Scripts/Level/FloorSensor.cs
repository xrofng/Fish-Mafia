using Combat2D;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(FloorController))]
public class FloorSensor : HitSensor
{
    FloorController floorController;

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out floorController);
    }

    protected override Color GetDisabledColor()
    {
        return Color.black;
    }

    protected override Color GetEnabledColor()
    {
        return Color.purple;
    }

    protected override void OnHitSuccess(Collider2D other)
    {
        base.OnHitSuccess(other);
        FloorManager.Instance.SetCurrentFloor(floorController);
    }
}
