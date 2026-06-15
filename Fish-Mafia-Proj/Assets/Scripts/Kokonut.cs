using Combat2D;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using Xrofng;

public class Kokonut : BetterMonoBehaviour, IKnockable
{
    [Title("Setting")]
    public EnemyStateSurround.SurroundSetting SurroundSetting;
    public EnemyStateSurround.SurroundSetting FleeAroundSetting;
    public EnemyStateAttack.AttackSetting AttackSetting;
    public LayerMask PlayerLayer;

    [Header("Obj Ref")]
    public MMF_Player OnHitFB;
    public MMF_Player OnBashStartedFB;
    public SpriteRenderer SpriteRenderer;
    public HitBox HitBox;
    public Animator Animator;

    private ActionBlocker _moveBlocker;

    public ActionBlocker MoveBlocker => _moveBlocker;


    protected override void Awake()
    {
        base.Awake();
        _moveBlocker = new ActionBlocker();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, SurroundSetting.AttackRadius);
    }

    public void TakeKnockback(Vector2 knockback, float duration)
    {
        OnHitFB?.PlayFeedbacks();
        StartCoroutine(KnockbackRoutine(knockback, duration));
    }

    IEnumerator KnockbackRoutine(Vector2 force, float duration)
    {
        _moveBlocker.AddPermanentBlock(this);
        float timer = 0f;

        while (timer < duration)
        {
            transform.position += (Vector3)(force * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1);
        _moveBlocker.RemovePermanentBlock(this);
    }

    public bool HasPlayerNearby(float radius)
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radius, PlayerLayer);
        return hit != null;
    }

    public bool HasPlayerInAttackRange()
    {
        return HasPlayerNearby(SurroundSetting.AttackRadius);
    }
}
