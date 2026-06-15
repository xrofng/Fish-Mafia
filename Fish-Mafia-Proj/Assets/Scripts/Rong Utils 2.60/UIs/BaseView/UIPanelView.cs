using UnityEngine;

namespace Xrofng
{
    public class UIPanelView : BetterMonoBehaviour
    {
        public bool IsVisible { get; protected set; }
        public RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>();
        private RectTransform _rectTransform;

        #region Unity Lifecycle

        protected override void Update()
        {
            base.Update();
            OnFrameInitialization();
            if (IsVisible) UpdateWhileVisible();
        }

        private void OnValidate() => OnInspectorChanged();
        #endregion

        #region Initialization
        #endregion

        #region Panel Visibility
        public void Show()
        {
            IsVisible = true;
            OnShowing();
            OnVisibleChanged(true);
        }

        public void Hide()
        {
            IsVisible = false;
            OnHiding();
            OnVisibleChanged(false);
        }

        public void ShowPanelSilent() => IsVisible = true;
        public void HidePanelSilent() => IsVisible = false;
        public void SetShowPanel(bool isShow)
        {
            if (isShow)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        protected virtual void OnShowing() { }
        protected virtual void OnHiding() { }
        protected virtual void OnVisibleChanged(bool currentVisibility) { }
        #endregion

        #region Extension Hooks
        protected virtual void OnFrameInitialization() { }
        protected virtual void UpdateWhileVisible() { }
        protected virtual void OnInspectorChanged() { }
        #endregion
    }
}