using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xrofng
{
    public class HideByScale : BetterMonoBehaviour
    {
        public bool IsHideOnAwake = false;

        protected override void Awake()
        {
            base.Awake();

        }
        [Button]
        public void Hide()
        {
            transform.localScale = Vector3.zero;
        }

        [Button]
        public void Show()
        {
            transform.localScale = Vector3.one;
        }
    }
}