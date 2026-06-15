using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xrofng
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseAlphaView : UIPanelView
    {
        private CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                {
                    TryGetComponent(out _canvasGroup);
                }
                return _canvasGroup;
            }
        }

        [SerializeField] bool InstantHideOnInitial = false;


        protected override void Awake()
        {
            base.Awake();
            if (InstantHideOnInitial)
            {
                SetAlphaInstant(0);
            }
        }

        protected override void OnShowing()
        {
            base.OnShowing();
            if (CanvasGroup)
            {
                SetAlphaInstant(1);
            }
        }

        protected override void OnHiding()
        {
            base.OnHiding();
            if (CanvasGroup)
            {
                SetAlphaInstant(0);
            }
        }

        public void SetAlphaInstant(float alpha)
        {
            CanvasGroup.alpha = alpha;
            CanvasGroup.interactable = alpha > 0;
            CanvasGroup.blocksRaycasts = alpha > 0;
        }
    }
}