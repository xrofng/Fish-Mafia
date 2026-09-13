using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Combat2D
{
    public abstract class HitHurtBoxBase : BetterMonoBehaviour
    {
        public enum ShapeType
        {
            Box,
            Circle,
            Oval
        }

        [Header("Shape")]
        public ShapeType shape = ShapeType.Box;

        [Header("Offset")]
        public Vector2 offset;

        protected Collider2D col;
        private bool _prevActive;

        // -------------------------
        // Setup Collider
        // -------------------------

        [GUIColor(0.0f, 0.8f, 0.5f, 1f)]
        [Button]
        void SetupCollider()
        {
            if (col != null)
            {
                Destroy(col);
            }

            switch (shape)
            {
                case ShapeType.Box:
                    col = gameObject.AddComponent<BoxCollider2D>();
                    break;

                case ShapeType.Circle:
                    col = gameObject.AddComponent<CircleCollider2D>();
                    break;

                case ShapeType.Oval:
                    var capsule = gameObject.AddComponent<CapsuleCollider2D>();
                    capsule.direction = CapsuleDirection2D.Horizontal;
                    col = capsule;
                    break;
            }

            col.isTrigger = true;
            col.offset = offset;
        }

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent(out col);
        }

        // -------------------------
        // Gizmos
        // -------------------------

        protected abstract Color GetEnabledColor();
        protected abstract Color GetDisabledColor();

        protected virtual Color GetGizmoColor()
        {
            return col.enabled ? GetEnabledColor() : GetDisabledColor();
        }

        public Vector2 GetColliderSize()
        {
            Collider2D collider;
            if (TryGetComponent(out collider)) return Vector2.zero;
            Debug.Log("GetColliderSize " + collider);
            // Pattern match against each possible 2D collider type
            switch (collider)
            {
                case BoxCollider2D box:
                    return box.size;

                case CircleCollider2D circle:
                    return Vector2.one * circle.radius * 2;
                    break;

                case CapsuleCollider2D capsule:
                    return capsule.size;
                    break;

                default:
                    return Vector2.zero;
                    break;
            }
        }

        protected override void Update()
        {
            base.Update();
        }

        void OnDrawGizmosSelected()
        {
            TryGetComponent(out col);
            Color c = GetGizmoColor();
            c.a = 0.5f;
            Gizmos.color = c;

            Vector3 pos = transform.position + (Vector3)offset;

            Vector3 scale = transform.lossyScale.sqrMagnitude > col.bounds.size.sqrMagnitude ? transform.lossyScale : col.bounds.size;

            switch (shape)
            {
                case ShapeType.Box:
                    Gizmos.DrawCube(pos, new Vector3(scale.x, scale.y, 0));
                    break;

                case ShapeType.Circle:
                    Gizmos.DrawSphere(pos, scale.x * 0.5f);
                    break;

                case ShapeType.Oval:
                    Gizmos.DrawCube(pos, new Vector3(scale.x, scale.y, 0));
                    break;
            }
        }
    }
}