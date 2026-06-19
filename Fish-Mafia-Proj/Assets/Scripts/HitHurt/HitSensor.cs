using System;
using UnityEngine;

namespace Combat2D
{
    public class HitSensor : HitHurtBoxBase
    {
        [Header("Filtering")]
        public LayerMask targetLayers;

        protected override Color GetDisabledColor()
        {
            return Color.darkGreen;
        }

        protected override Color GetEnabledColor()
        {
            return Color.gray;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsInLayerMask(other.gameObject.layer, targetLayers))
                return;

            OnHitSuccess(other);
        }

        protected virtual void OnHitSuccess(Collider2D other)
        {
            
        }

        private bool IsInLayerMask(int layer, LayerMask mask)
        {
            return (mask.value & (1 << layer)) != 0;
        }
    }
}