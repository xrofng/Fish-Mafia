using System;
using System.Collections.Generic;
using UnityEngine;
using Xrofng;

public class FloorManager : Singleton<FloorManager>
{
    [SerializeField] private bool autoDetectFloors = true;
    [SerializeField] private List<FloorController> floors = new();

    private FloorController _currentFloor;
    private List<FloorController> _nearbyFloors;

    protected override void Awake()
    {
        base.Awake();
        _nearbyFloors = new List<FloorController>();
        if (autoDetectFloors)
        {
            DetectFloors();
        }
    }

    protected override void Start()
    {
        base.Start();
        /// should be on delayed Start after EnemyStateMachine set initial state
        //SetCurrentFloor(floors[0]);
    }

    private void DetectFloors()
    {
        floors.Clear();
        int i = 0;
        foreach (Transform child in transform)
        {
            FloorController floor = child.GetComponent<FloorController>();

            if (floor != null)
            {
                floor.ListId = i;
                floors.Add(floor);
                i += 1;
            }
        }
    }

    public void SetCurrentFloor(FloorController floor)
    {
        if (_currentFloor == floor)
            return;

        if (_currentFloor != null)
        {
            SetFloorActive(false);
        }
            
        EventBus.TriggerEvent(new EvsFloorChanged(floor, _currentFloor));
        _currentFloor = floor;
        EvaluateNearbyFloor(_currentFloor);
        SetFloorActive(true);
    }

    private void EvaluateNearbyFloor(FloorController currentFloor)
    {
        _nearbyFloors.Clear();
        FloorController upper = GetUpperFloor(currentFloor);
        FloorController lower = GetLowerFloor(currentFloor);

        if (upper)
        {
            _nearbyFloors.Add(upper);
        }
        if (lower)
        {
            _nearbyFloors.Add(lower);
        }
    }

    private FloorController GetUpperFloor(FloorController currentFloor)
    {
        if (currentFloor.ListId + 1  < floors.Count)
        {
            return floors[currentFloor.ListId + 1];
        }
        return null;
    }

    private FloorController GetLowerFloor(FloorController currentFloor)
    {
        if (currentFloor.ListId - 1 >= 0)
        {
            return floors[currentFloor.ListId - 1];
        }
        return null;
    }

    private void SetFloorActive(bool v)
    {
        _currentFloor.SetFloorActive(v);
        foreach (FloorController floor in _nearbyFloors)
        {
            Debug.Log(floor.name);
            floor.SetFloorActive(v);
        }
    }

    public struct EvsFloorChanged
    {
        public FloorController NextFloor;
        public FloorController PrevFloor;

        public EvsFloorChanged(FloorController nextFloor, FloorController prevFloor)
        {
            NextFloor = nextFloor;
            PrevFloor = prevFloor;
        }
    }
}

