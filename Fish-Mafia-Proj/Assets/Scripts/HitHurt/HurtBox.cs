using MoreMountains.Feedbacks;
using System;
using System.Collections;
using UnityEngine;

namespace Combat2D
{
    public class HurtBox : HitHurtBoxBase
    {
        [Header("Obj Ref")]
        public Transform EntityRoot;
        public MMF_Player OnHitFB;

        private IDamageable damageable;
        private IKnockable knockable;

        protected override Color GetEnabledColor() => Color.green;
        protected override Color GetDisabledColor() => Color.black;

        protected override void Awake()
        {
            damageable = GetComponentInParent<IDamageable>();
            knockable = GetComponentInParent<IKnockable>();
        }

        public void TakeHit(int damage)
        {
            if (damageable != null)
            {
                OnHitFB?.PlayFeedbacks();
                damageable.TakeDamage(damage);
            }
        }

        public void TakeKnockback(Vector2 knockback, float duration)
        {
            if (knockable != null)
            {
                knockable.TakeKnockback(knockback, duration);
            }
        }
    }
}
