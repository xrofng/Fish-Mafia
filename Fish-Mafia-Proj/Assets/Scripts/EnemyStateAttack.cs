using Combat2D;
using System;
using UnityEngine;

public class EnemyStateAttack : BaseEnemyState
{
    public AttackSetting Setting;

    [System.Serializable]
    public class AttackSetting
    {
        [Header("Anticipation")]
        public float BackStepDistance = 0.5f;
        public float BackStepDuration = 0.15f;
        public AnimationCurve BackStepCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Dash")]
        public float DashDistance = 5f;
        public float DashDuration = 0.25f;
        public AnimationCurve DashCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("EndLag")]
        public float EndLagDuration = 1f;
    }

    enum EPhase
    {
        Anticipation,
        Dash,
        End
    }

    private EPhase _phase;
    private float _timer;
    private HitBox _hitBox;

    private Vector3 _startPos;
    private Vector3 _targetPos;
    private Vector3 _backDir;
    private Vector3 _dashDir;
    private float _phaseDuration;

    public EnemyStateAttack(KokonutStateMachine.EEnemyState key, KokonutStateMachine kokonutStateMachine, PlayerChar pChar, AttackSetting set, HitBox hitBox) : base(key, kokonutStateMachine, pChar)
    {
        Setting = set;
        _hitBox = hitBox;
        _hitBox.SetHitBoxEnabled(false);
    }

    public override void EnterState()
    {
        base.EnterState();

        _phase = EPhase.Anticipation;

        _phaseDuration = Setting.BackStepDuration;
        _timer = _phaseDuration;

        _startPos = StateMachine.transform.position;

        _backDir = (StateMachine.transform.position - PlayerChar.transform.position).normalized;
        _dashDir = (PlayerChar.transform.position - StateMachine.transform.position).normalized;

        Kokonut.Animator.Play("Enemy_Bashing_Aclip");
        Kokonut.OnBashStartedFB?.PlayFeedbacks();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _timer -= Time.deltaTime;

        switch (_phase)
        {
            case EPhase.Anticipation:
                HandleAnticipation();
                break;

            case EPhase.Dash:
                HandleDash();
                break;

            case EPhase.End:
                HandleEndLag();
                break;
        }
    }

    void HandleAnticipation()
    {
        float t = 1f - (_timer / _phaseDuration); // normalized 0 → 1
        float eval = Setting.BackStepCurve.Evaluate(t);

        Vector3 targetBackPos = _startPos + _backDir * Setting.BackStepDistance;

        StateMachine.transform.position = Vector3.Lerp(_startPos, targetBackPos, eval);

        if (_timer <= 0f)
        {
            _targetPos = ComputTargetPos();

            _phase = EPhase.Dash;

            _phaseDuration = Setting.DashDuration;
            _timer = _phaseDuration;

            _startPos = StateMachine.transform.position; // IMPORTANT reset
        }
    }

    Vector3 ComputTargetPos()
    {
        // lock direction ONCE
        _dashDir = (PlayerChar.transform.position - StateMachine.transform.position).normalized;

        return StateMachine.transform.position + _dashDir * Setting.DashDistance; ;
    }

    void HandleDash()
    {
        float t = 1f - (_timer / _phaseDuration);
        float eval = Setting.DashCurve.Evaluate(t);

        StateMachine.transform.position = Vector3.Lerp(_startPos, _targetPos, eval);
        _hitBox.SetHitBoxEnabled(true);

        if (_timer <= 0f)
        {
            _phase = EPhase.End;
            _phaseDuration = Setting.EndLagDuration;
            _timer = _phaseDuration;
            _hitBox.SetHitBoxEnabled(false);
        }
    }

    private void HandleEndLag()
    {
        if (_timer <= 0f)
        {
            // stop movement → go back to surround
            StateMachine.TransitionToState(KokonutStateMachine.EEnemyState.FleeAround);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override KokonutStateMachine.EEnemyState GetNextState()
    {
        return KokonutStateMachine.EEnemyState.Attack;
    }
}