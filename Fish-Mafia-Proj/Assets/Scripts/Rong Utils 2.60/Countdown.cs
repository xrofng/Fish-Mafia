using System;
using UnityEngine;

namespace Xrofng
{
    /// <summary>
    /// Simple integer-based countdown timer.
    /// Decreases Current by a given amount each tick and fires events.
    /// </summary>
    public class Countdown
    {
        /// Current remaining value of the countdown.
        public int Current { get; private set; }

        /// Initial value used when resetting the countdown.
        public int Start { get; private set; }

        /// Returns true when the countdown has reached zero or below.
        public bool IsFinished => Current <= 0;

        /// Invoked every time the countdown changes.
        /// Passes the current remaining value.
        public event Action<int> OnTick;

        /// Invoked once when the countdown reaches zero.
        public event Action OnFinished;

        /// <summary>
        /// Creates a new countdown starting at the given value.
        /// </summary>
        public Countdown(int startValue)
        {
            Start = startValue;
            Current = startValue;
        }

        /// <summary>
        /// Advances the countdown.
        /// Call this manually (for example from Update or turn logic).
        /// progress: how much to subtract (default = 1).
        /// </summary>
        public void ProgressCountdown(int progress = 1)
        {
            // Prevent ticking once already finished
            if (IsFinished)
                return;

            // Reduce current value
            Current -= progress;

            // If countdown reaches zero or below
            if (Current <= 0)
            {
                // Notify listeners of the final tick
                OnTick?.Invoke(Current);

                // Notify listeners that countdown finished
                OnFinished?.Invoke();
            }
            else
            {
                // Normal tick update
                OnTick?.Invoke(Current);
            }
        }

        /// <summary>
        /// Resets countdown back to its starting value.
        /// </summary>
        public void Reset()
        {
            Current = Start;
        }

        public void CleatOnFinished()
        {
            OnFinished = null;
        }

        public void CleatOnTick()
        {
            OnTick = null;
        }
    }
}
