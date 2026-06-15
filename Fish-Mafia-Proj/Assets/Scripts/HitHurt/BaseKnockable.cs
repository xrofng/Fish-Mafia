using System;
using System.Collections;
using UnityEngine;

public class BaseKnockable : BetterMonoBehaviour, IKnockable
{
    public bool IsKnocking { get; private set; }
    public Action OnKnockbackStarted;
    public Action OnKnockbackEnded;
    public Transform TargetTansform;

    protected override void Awake()
    {
        base.Awake();
        if (TargetTansform == null)
        {
            TargetTansform = transform;
        }
    }

    public void TakeKnockback(Vector2 knockback, float duration)
    {
        StartCoroutine(KnockbackRoutine(knockback, duration));
    }

    IEnumerator KnockbackRoutine(Vector2 dir, float duration)
    {
        OnKnockbackStarted?.Invoke();
        float timer = 0f;
        IsKnocking = false;

        while (timer < duration)
        {
            IsKnocking = true;
            TargetTansform.position += (Vector3)(dir * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        IsKnocking = false;
        yield return new WaitForSeconds(1);
        OnKnockbackEnded?.Invoke();
    }
}
