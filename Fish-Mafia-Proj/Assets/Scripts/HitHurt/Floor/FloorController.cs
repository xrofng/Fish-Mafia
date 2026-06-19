using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private bool autoDetectEnemies = true;

    [FoldoutGroup("Ref")]
    [SerializeField] BoxCollider2D AirStandCol;

    private List<KokonutStateMachine> _enemies = new();

    public IReadOnlyList<KokonutStateMachine> Enemies => _enemies;

    public int ListId { get; internal set; }

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _enemies.Clear();

        if (autoDetectEnemies)
        {
            ScanChildren();
        }
    }

    private void ScanChildren()
    {
        var found = GetComponentsInChildren<KokonutStateMachine>(true);

        foreach (var component in found)
        {
            _enemies.Add(component);
        }
    }

    public void SetFloorActive(bool active, bool setAirStand = false)
    {
        foreach (KokonutStateMachine enemy in _enemies)
        {
            enemy.TransitionToState(active ? KokonutStateMachine.EEnemyState.Surround : KokonutStateMachine.EEnemyState.Pause);
        }
        if (setAirStand)
        {
            AirStandCol.enabled = active;
        }
    }
}