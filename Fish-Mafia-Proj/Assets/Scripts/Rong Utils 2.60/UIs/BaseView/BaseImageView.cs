using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Xrofng
{
    public class BaseImageView : BaseAlphaView
    {
        [ChildGameObjectsOnly]
        [FoldoutGroup("Child Ref")]
        [SerializeField] protected Image Image;

        protected override void Awake()
        {
            base.Awake();
            if (Image == null)
            {
                TryGetComponent(out Image);
            }
        }

        public void SetColor(Color color)
        {
            Image.color = color;
        }

        public void SetImageSprite(Sprite sprite)
        {
            Image.sprite = sprite;
        }

        public Sprite GetSprite()
        {
            return Image.sprite;
        }
    }
}