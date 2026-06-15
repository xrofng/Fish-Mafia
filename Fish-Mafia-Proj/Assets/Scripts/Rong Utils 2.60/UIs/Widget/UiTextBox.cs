using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Xrofng
{
    public class UITextBox : BaseImageView
    {
        [ChildGameObjectsOnly]
        [FoldoutGroup("Child Ref")]
        [SerializeField] TextMeshProUGUI Text;

        public void SetText(string message)
        {
            Text.text = message;
        }

        public void SetTextMaterial(Material material)
        {
            Text.fontMaterial = material;
        }

        public void SetBoxColor(Color color)
        {
            SetColor(color);
        }

        public void SetTextColor(Color color)
        {
            Text.color = color;
        }

        public void ReplaceBoxImage(Sprite sprite)
        {
            SetImageSprite(sprite);
        }

        public Sprite GetBoxSprite()
        {
            return GetSprite();
        }
    }
}