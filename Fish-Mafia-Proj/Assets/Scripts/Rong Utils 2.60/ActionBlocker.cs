using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Blocks character actions either permanently or for a duration.
/// Supports global block/unblock callbacks.
/// </summary>
public class ActionBlocker
{
    public class BlockState
    {
        public float Timer;
        public Action OnRemovedCallback;
    }

    private Dictionary<object, BlockState> _timedBlocks;
    private HashSet<object> _permanentBlocks;

    private Action _onBlocked;
    private Action _onUnblocked;

    private bool _wasBlockedLastFrame;

    public bool HasBlocks =>
        _permanentBlocks.Count > 0 ||
        _timedBlocks.Count > 0;

    public bool HasNoBlocks => !HasBlocks;

    public ActionBlocker()
    {
        _timedBlocks = new Dictionary<object, BlockState>();
        _permanentBlocks = new HashSet<object>();
    }

    // ---------------- Callbacks ----------------

    public void SetOnBlock(Action callback)
    {
        _onBlocked = callback;
    }

    public void SetOnUnblock(Action callback)
    {
        _onUnblocked = callback;
    }

    // ---------------- Update ----------------

    /// <summary>
    /// Must be called every Update to expire timed blocks.
    /// Also evaluates block/unblock transitions.
    /// </summary>
    public void UpdateBlocks()
    {
        bool wasBlocked = HasBlocks;

        if (_timedBlocks.Count > 0)
        {
            var keys = ListPool<object>.Get();

            foreach (var kvp in _timedBlocks)
                keys.Add(kvp.Key);

            foreach (object key in keys)
            {
                var state = _timedBlocks[key];
                state.Timer -= Time.deltaTime;

                if (state.Timer <= 0f)
                {
                    _timedBlocks.Remove(key);
                    state.OnRemovedCallback?.Invoke();
                }
            }

            ListPool<object>.Release(keys);
        }

        EvaluateBlockTransition(wasBlocked);
    }

    private void EvaluateBlockTransition(bool wasBlocked)
    {
        bool isBlocked = HasBlocks;

        if (!wasBlocked && isBlocked)
        {
            _onBlocked?.Invoke();
        }
        else if (wasBlocked && !isBlocked)
        {
            _onUnblocked?.Invoke();
        }
    }

    // ---------------- Permanent ----------------

    public void AddPermanentBlock(object key)
    {
        if (_permanentBlocks.Contains(key) == false)
        {
            bool wasBlocked = HasBlocks;

            _permanentBlocks.Add(key);

            EvaluateBlockTransition(wasBlocked);
        }
    }

    public void RemovePermanentBlock(object key)
    {
        if (_permanentBlocks.Contains(key) == true)
        {
            bool wasBlocked = HasBlocks;

            _permanentBlocks.Remove(key);

            EvaluateBlockTransition(wasBlocked);
        }
    }

    public void ClearPermanentBlocks()
    {
        bool wasBlocked = HasBlocks;

        _permanentBlocks.Clear();

        EvaluateBlockTransition(wasBlocked);
    }

    // ---------------- Timed ----------------

    public void AddTimedBlock(object key, float duration)
    {
        AddTimedBlock(key, duration, null);
    }

    public void AddTimedBlock(object key, float duration, Action callback)
    {
        bool wasBlocked = HasBlocks;

        if (_timedBlocks.TryGetValue(key, out var state))
        {
            state.Timer = duration;
            state.OnRemovedCallback = callback;
        }
        else
        {
            _timedBlocks.Add(key, new BlockState
            {
                Timer = duration,
                OnRemovedCallback = callback
            });
        }

        EvaluateBlockTransition(wasBlocked);
    }

    public void RemoveTimedBlock(object key)
    {
        bool wasBlocked = HasBlocks;

        _timedBlocks.Remove(key);

        EvaluateBlockTransition(wasBlocked);
    }

    public bool HasTimedBlock(object key)
    {
        return _timedBlocks.ContainsKey(key);
    }

    public float GetRemainingDuration(object key)
    {
        if (_timedBlocks.TryGetValue(key, out var state))
            return state.Timer;

        return 0f;
    }
}