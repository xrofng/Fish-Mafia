using UnityEngine;
using Sirenix.OdinInspector;

namespace Xrofng
{
    public class WorldLayoutGroup : MonoBehaviour
    {
        public enum Direction
        {
            Horizontal,
            Vertical
        }

        public Direction LayoutDirection = Direction.Horizontal;
        [OnValueChanged(nameof(OnSpacingChanged))]
        public float Spacing = 1f;
        public bool AutoApplyWhenChanceSpacing = false;
        public bool Center = true;

        public void OnSpacingChanged()
        {
            if (AutoApplyWhenChanceSpacing)
            {
                ApplyLayout();
            }
        }

        [Button]
        public void ApplyLayout()
        {
            int count = transform.childCount;
            if (count == 0) return;

            float totalSize = 0f;

            // Calculate total length
            for (int i = 0; i < count; i++)
            {
                totalSize += Spacing;
            }

            totalSize -= Spacing;

            float startOffset = Center ? -totalSize * 0.5f : 0f;

            for (int i = 0; i < count; i++)
            {
                var child = transform.GetChild(i);

                Vector3 pos = child.localPosition;

                float offset = startOffset + i * Spacing;

                if (LayoutDirection == Direction.Horizontal)
                    pos.x = offset;
                else
                    pos.y = offset;

                child.localPosition = pos;
            }
        }
    }
}
