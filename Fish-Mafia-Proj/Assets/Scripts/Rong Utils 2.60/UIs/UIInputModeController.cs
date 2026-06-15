using UnityEngine;
using UnityEngine.InputSystem;

namespace Xrofng
{
    public class UIInputModeController : BetterMonoBehaviour
    {
        public enum UIInputMode
        {
            Pointer,
            Navigation
        }

        [SerializeField]
        UIInputMode currentMode = UIInputMode.Navigation;

        protected override void Update()
        {
            base.Update();
            if (Mouse.current.delta.ReadValue().sqrMagnitude > 0.1f)
            {
                SetMode(UIInputMode.Pointer);
            }

            if (Keyboard.current.anyKey.wasPressedThisFrame ||
                Gamepad.current?.leftStick.ReadValue().sqrMagnitude > 0.2f)
            {
                SetMode(UIInputMode.Navigation);
            }
        }

        void SetMode(UIInputMode mode)
        {
            if (currentMode == mode)
                return;

            currentMode = mode;

            OnInputModeChanged(mode);
        }

        void OnInputModeChanged(UIInputMode mode)
        {
            switch (mode)
            {
                case UIInputMode.Pointer:
                    //EnablePointerHover();
                    break;

                case UIInputMode.Navigation:
                    //EnableKeyboardSelection();
                    break;
            }
        }

    }
}