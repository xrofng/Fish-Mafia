using Combat2D;
using Sirenix.OdinInspector;
using UnityEngine;
using Xrofng;

public class EnemyStateSurround : BaseEnemyState
{
    [System.Serializable]
    public class SurroundSetting
    {
        [MinMaxSlider(0, 10, showFields: true)]
        public Vector2 SpeedRange;
        [MinMaxSlider(0, 10, showFields: true)]
        public Vector2 DetermineNextDecisionRange;
        [Header("Player Detection")]
        [Range(1, 15)]
        public float PlayerSurroundRadius;
        [Range(1, 8)] public float AttackRadius = 4;

        [ReadOnly]
        public CountdownClock DecisionTimer;

        public Vector3 ComputeTargetPos(Transform target)
        {
            if (target == null) return Vector3.zero;

            // Random point inside circle (uniform distribution)
            Vector2 offset = Random.insideUnitCircle * PlayerSurroundRadius;
            offset.y = Mathf.Abs(offset.y);

            Vector3 _targetPos = target.position + (Vector3)offset;
            _targetPos.x = Mathf.Clamp(_targetPos.x, -15, 15);
            return _targetPos;
        }

        public float ComputeMoveSpeed()
        {
            return Random.Range(SpeedRange.x, SpeedRange.y);
        }

        public void DetermineNextDecisionTime()
        {
            DecisionTimer = new CountdownClock(Random.Range(DetermineNextDecisionRange.x, DetermineNextDecisionRange.y));
            DecisionTimer.StartTimer();
        }
    }

    protected SurroundSetting Setting;

    float _currSpeed;
    Vector3 _targetPos;

    public virtual bool Attackable => true;

    public EnemyStateSurround(KokonutStateMachine.EEnemyState key, KokonutStateMachine kokonutStateMachine, PlayerChar pChar, SurroundSetting set) : base(key, kokonutStateMachine, pChar)
    {
        Setting = set;
    }

    public override void EnterState()
    {
        base.EnterState();
        DecideSurroundTarget();
    }

    private void DecideSurroundTarget()
    {
        _targetPos = Setting.ComputeTargetPos(PlayerChar.transform);
        _currSpeed = Setting.ComputeMoveSpeed();
        Setting.DetermineNextDecisionTime();
    }

    public override KokonutStateMachine.EEnemyState GetNextState()
    {
        return KokonutStateMachine.EEnemyState.Surround;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Setting.DecisionTimer.UpdateTimer(Time.deltaTime);

        if (Kokonut.MoveBlocker.HasBlocks)
            return;

        if (Attackable)
        {
            TryAttack();
        }

        Vector3 prevPos = Kokonut.transform.position;

        // Move
        Kokonut.transform.position = Vector3.MoveTowards(
            Kokonut.transform.position,
            _targetPos,
            _currSpeed * Time.deltaTime
        );

        // Direction check
        float deltaX = Kokonut.transform.position.x - prevPos.x;

        if (Mathf.Abs(deltaX) > 0.001f)
        {
            Kokonut.SpriteRenderer.flipX = deltaX < 0; // left = true, right = false
        }

        // Re-decide
        if (Setting.DecisionTimer.CompletionRatio >= 1)
        {
            DecideSurroundTarget();
            OnSurroundEnd();
        }
    }

    protected virtual void OnSurroundEnd()
    {
        
    }

    private void TryAttack()
    {
        if (StateMachine.Kokonut.HasPlayerInAttackRange())
        {
            if (StateMachine.HiveMind.RequestAttack(StateMachine))
            {
                StateMachine.TransitionToState(KokonutStateMachine.EEnemyState.Attack);
            }
        }
    }
}
