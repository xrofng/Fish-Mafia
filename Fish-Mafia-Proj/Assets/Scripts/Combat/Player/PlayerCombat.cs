using Combat2D;
using MoreMountains.Tools;
using System;
using System.Collections;
using UnityEngine;

public class PlayerCombat : BetterMonoBehaviour, IEventSubcriber<Health.EvsEnemyDied>
{
    [Header("Obj Ref")]
    public InputController input;
    public Animator animator;
    public SpriteRenderer SpriteRenderer;
    public Transform VisualGroup;
    public Transform EvocationSpawnPoint;
    public BulletMana BulletMana;
    public HurtBox HurtBox;
    public HitBox[] hitBoxes;

    [Header("Moves")]
    public MoveData[] neutralMoves;
    public float bufferDuration = 0.25f; // tweakable

    public Vector3 EncloseOffset;

    bool bufferedAttack;
    float bufferTimer;
    public string bufferedInputName;

    //PlayerStateMain.InputHoldDirection bufferedDirection;
    AttackResolver resolver = new AttackResolver();

    Transform _currTarget;
    MoveData currentMove;
    private float moveElapsedTime;

    public IEnumerator CurrAttackMulator { get; private set; }

    public string InputOnEnter { get; set; }
    public bool BufferedAttack => bufferedAttack;

    protected override void Awake()
    {
        base.Awake();
        animator.Play("Stand_Aclip");
    }

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

    protected override void Update()
    {
        UpdateMoveTimer();
        UpdateBuffer();
    }

    // -------------------------
    // Attack Flow
    // -------------------------

    public MoveData InvokeAttack(PlayerStateMain.InputHoldDirection currFaceDirection, Transform target, string inputName)
    {
        _currTarget = target;

        // no active move → start immediately
        if (currentMove == null)
            return StartAttack(inputName, currFaceDirection);

        if (currentMove.CanCancelByAttack == false)
        {
            return null;
        }

        // too early → BUFFER instead of ignore
        if (moveElapsedTime < currentMove.comboWindows.x)
        {
            BufferAttack(currFaceDirection, inputName);
            return null;
        }

        // valid window → combo immediately
        if (moveElapsedTime <= currentMove.comboWindows.y)
        {
            return TryCombo(currFaceDirection, inputName);
        }

        // too late → restart
        Debug.Log("Too late → restart");
        return StartAttack(inputName, currFaceDirection);
    }


    public void ClearBuffer()
    {
        bufferedAttack = false;
        bufferTimer = 0;
    }

    void BufferAttack(PlayerStateMain.InputHoldDirection currFaceDirection, string inputName)
    {
        bufferedAttack = true;
        bufferTimer = currentMove.comboWindows.x - moveElapsedTime;
        bufferedInputName = inputName;
    }

    MoveData StartAttack(string inputActionName, PlayerStateMain.InputHoldDirection currFaceDirection = PlayerStateMain.InputHoldDirection.None)
    {
        var move = resolver.ResolveFromNeutral(inputActionName, currFaceDirection, neutralMoves);

        if (move != null)
        {
            ExecuteMove(move);
            return move;
        }

        return null;
    }

    MoveData TryCombo(PlayerStateMain.InputHoldDirection direction, string inputName)
    {
        var move = resolver.ResolveCombo(currentMove, inputName);

        if (move != null)
        {
            ExecuteMove(move);
            return move;
        }

        return null;
    }

    void ExecuteMove(MoveData move)
    {
        currentMove = move;
        moveElapsedTime = 0f;

        Vector3 dir = (_currTarget.position - transform.position).normalized;

        SetHitBoxesMultiplier(move, dir);

        CurrAttackMulator = move.AttackMoveRoutine(animator, this, _currTarget.position + EncloseOffset, dir);
        StartCoroutine(CurrAttackMulator);
    }

    private void SetHitBoxesMultiplier(MoveData move, Vector3 dir)
    {
        foreach (HitBox hitBox in hitBoxes)
        {
            hitBox.Stat = move.Stat;
            hitBox.DirFromAttacker = dir;
        }
    }

    public void SetHitBoxesEnabled(bool ena)
    {
        foreach (HitBox hitBox in hitBoxes)
        {
            hitBox.enabled = ena;
        }
    }

    void UpdateMoveTimer()
    {
        if (currentMove == null) return;

        moveElapsedTime += Time.deltaTime;

        if (moveElapsedTime > currentMove.comboWindows.y)
        {
            EndCombo();
        }
    }

    void UpdateBuffer()
    {
        if (!bufferedAttack) return;

        bufferTimer -= Time.deltaTime;

        if (bufferTimer <= 0)
        {
            bufferedAttack = false;

            // 🔥 KEY PART: check if window is NOW valid
            if (currentMove != null &&
                moveElapsedTime >= currentMove.comboWindows.x &&
                moveElapsedTime <= currentMove.comboWindows.y)
            {
                bufferedAttack = false;
                //Debug.Log("Try Buffered TryCombo with dir " + bufferedDirection);
                TryCombo(PlayerStateMain.InputHoldDirection.None, bufferedInputName);

                //bufferedDirection = PlayerStateMain.InputHoldDirection.None;
            }
            return;
        }
    }

    void EndCombo()
    {
        currentMove = null;
    }

    public void SetFacing(Vector3 dir)
    {
        Vector3 g = Vector3.one;
        g.x = (dir.x >= 0 ? 1 : -1);
        VisualGroup.localScale = g;
        //SpriteRenderer.flipX = dir.x >= 0 ? true : false;
    }

    public void OnEventBusTrigger(Health.EvsEnemyDied eventType)
    {
        if (eventType.KokonutStateMachine == _currTarget)
        {
            _currTarget = null;
        }
    }
}