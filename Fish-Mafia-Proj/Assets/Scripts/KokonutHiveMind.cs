using System;
using System.Collections.Generic;
using UnityEngine;

public class KokonutHivemind : BetterMonoBehaviour, IEventSubcriber<Health.EvsEnemyDied>
{
    [Header("Limits")]
    public int MaxAttackers = 2;

    [Header("Obj Ref")]
    public List<KokonutStateMachine> AllAgents;
    private List<KokonutStateMachine> attackers = new();

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

    protected override void Awake()
    {
        base.Awake();
        foreach (var agent in AllAgents)
        {
            agent.RegisterHiveMind(this);
        }
    }

    public bool RequestAttack(KokonutStateMachine agent)
    {
        if (attackers.Contains(agent))
        {
            return true;
        }

        if (attackers.Count >= MaxAttackers)
        {
            return false;
        }
        attackers.Add(agent);
        return true;
    }

    public void EndAttack(KokonutStateMachine agent)
    {
        Remove(agent);
    }

    private void Remove(KokonutStateMachine agent)
    {
        if (attackers.Contains(agent))
        {
            attackers.Remove(agent);
        }
    }

    public bool IsAttacker(KokonutStateMachine agent)
    {
        return attackers.Contains(agent);
    }

    public void OnEventBusTrigger(Health.EvsEnemyDied eventType)
    {
        Remove(eventType.KokonutStateMachine);
    }

    public void SetEnemyState(KokonutStateMachine.EEnemyState targetState)
    {
        foreach (var agent in AllAgents)
        {
            agent.TransitionToState(targetState);
            Debug.Log("SetEnemyState");
        }
    }
}
