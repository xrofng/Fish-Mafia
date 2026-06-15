using UnityEngine;

namespace Combat2D
{
    public class PlayerHitBox : HitBox
    {
        private PlayerCombat owner;

        protected override void Awake()
        {
            base.Awake();
            owner = GetComponentInParent<PlayerCombat>();
        }

        protected override void OnHitSuccess(HurtBox hurtbox)
        {
            base.OnHitSuccess(hurtbox);

            if (owner == null || Stat == null)
                return;

            owner.BulletMana.Add(Stat.BulletGainOnHit);
        }
    }
}