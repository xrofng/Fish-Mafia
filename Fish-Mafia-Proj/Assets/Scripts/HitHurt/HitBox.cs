using UnityEngine;

namespace Combat2D
{
    public class HitBox : HitHurtBoxBase
    {
        public int BaseDamage = 10;
        public MoveData.MoveStat Stat;
        public bool AutoDetermineKnockDirection = false;
        public Vector3 DirFromAttacker;

        [Header("Filtering")]
        public LayerMask targetLayers;

        protected override Color GetEnabledColor() => Color.red;
        protected override Color GetDisabledColor() => Color.yellow;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsInLayerMask(other.gameObject.layer, targetLayers))
                return;

            var hurtbox = other.GetComponent<HurtBox>();
            if (hurtbox != null)
            {
                hurtbox.TakeHit((int)(BaseDamage * Stat.PowerMultiplier));
                if (AutoDetermineKnockDirection)
                {
                    DirFromAttacker = (hurtbox.transform.position - transform.position).normalized;
                }
                hurtbox.TakeKnockback(Stat.ComputeKnockback(DirFromAttacker), Stat.KnockbackDuration);
            }

            OnHitSuccess(hurtbox);
        }

        private bool IsInLayerMask(int layer, LayerMask mask)
        {
            return (mask.value & (1 << layer)) != 0;
        }

        public void SetHitBoxEnabled(bool enabled)
        {
            col.enabled = enabled;
        }

        protected virtual void OnHitSuccess(HurtBox hurtbox)
        {
            // base does nothing
        }
    }
}