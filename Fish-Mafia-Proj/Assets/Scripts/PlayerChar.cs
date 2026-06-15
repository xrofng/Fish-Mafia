using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Xrofng;

namespace Combat2D
{
    public class PlayerChar : BetterMonoBehaviour, IEventSubcriber<Health.EvsEnemyDied>
    {
        [SerializeField]
        [Header("Setting")]
        public FallingSetting Engaging;
        public FallingSetting OnField;
        public PlayerStateDodge.DodgeSetting Dodge;
        public float MidAirMoveSpeed = 2;

        [System.Serializable]
        public class FallingSetting
        {
            public CountdownClock Timer;
            public AnimationCurve Curve;
            public float GravityAtZero;
            public float GravityAtOne;

            public float ComputeGravity()
            {
                float t = Curve.Evaluate(Timer.CompletionRatio);
                return Mathf.Lerp(GravityAtZero, GravityAtOne, t);
            }
        }

        [Header("My Ref")]
        public PlayerStateMachine PlayerStateMachine;
        public PlayerCombat PlayerCombat;
        public InputController inputController;
        public Rigidbody2D Rigdibody;
        [Header("Obj Ref")]
        public CoroutineRunner CoroutineRunner;
        public PlayerTargeting PlayerTargeting;
        public BaseKnockable Knockable;

        public bool HasNearbyEnemy = true;
        public PlayerStateMain.InputHoldDirection CurrHoldDir;

        protected override void OnEnable()
        {
            base.OnEnable();
            EventBusRegister.EventBusSubcribe(this);
            Knockable.OnKnockbackStarted += OnKnockbackStarted;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventBusRegister.EventBusUnscribe(this);
            Knockable.OnKnockbackStarted -= OnKnockbackStarted;
        }

        private void OnKnockbackStarted()
        {
            PlayerStateMachine.TransitionToState(PlayerStateMachine.EPlayerState.Knocked);
        }

        // -------------------------
        // Attack
        // -------------------------
        public void OnAttack(InputAction.CallbackContext ctx)
        {
            
        }

        // -------------------------
        // Face
        // -------------------------
        public void OnMove(InputAction.CallbackContext ctx)
        {
            
        }

        // -------------------------
        // Dodge
        // -------------------------
        public void OnDodge(InputAction.CallbackContext ctx)
        {
            // start routine
        }

        public void OnEventBusTrigger(Health.EvsEnemyDied eventType)
        {
            //HasNearbyEnemy = PlayerTargeting.HasEnemyNearby();
        }

        protected override void Update()
        {
            base.Update();
            HasNearbyEnemy = PlayerTargeting.HasEnemyNearby();
        }
    }
}
