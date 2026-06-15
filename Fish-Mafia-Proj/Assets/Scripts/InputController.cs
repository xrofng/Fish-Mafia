using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    InputSystem_Actions input;
    public InputSystem_Actions Input => input;

    private Action<InputAction.CallbackContext> _currLeftClick;
    private Action<InputAction.CallbackContext> _currRightClick;
    private Action<InputAction.CallbackContext> _currDodge;

    private Action<Vector2> _currMoveUpdate;

    private bool _movePressed;
    [ReadOnly]
    public Vector2 _moveDir;

    void Awake()
    {
        input = new InputSystem_Actions();
        InitHoldableMove();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        HandleMovePressing();
    }

    private void HandleMovePressing()
    {
        if (_movePressed) 
        {
            _currMoveUpdate?.Invoke(_moveDir);
        }
    }

    public void SetAction(InputAction action, Action<InputAction.CallbackContext> onActionClick, Action<InputAction.CallbackContext> cache)
    {
        if (cache != null)
        {
            action.performed -= cache;
        }
        cache = onActionClick;
        action.performed += cache;
    }

    public void SetLeftClick(Action<InputAction.CallbackContext> onLeftClick)
    {
        if (_currLeftClick != null)
        {
            input.Player.LeftAttack.performed -= onLeftClick;
        }
        _currLeftClick = onLeftClick;
        input.Player.LeftAttack.performed += _currLeftClick;
    }

    public void SetRightClick(Action<InputAction.CallbackContext> onRightClick)
    {
        if (_currRightClick != null)
        {
            input.Player.Shoot.performed -= _currRightClick;
        }
        _currRightClick = onRightClick;
        input.Player.Shoot.performed += _currRightClick;
    }

    public void SetDodgeClick(Action<InputAction.CallbackContext> onDodgeClick)
    {
        if (_currDodge != null)
        {
            input.Player.Dodge.performed -= _currDodge;
        }
        _currDodge = onDodgeClick;
        input.Player.Dodge.performed += _currDodge;
    }

    public void InitHoldableMove()
    {
        input.Player.Move.performed += MovePerformed;
        input.Player.Move.canceled += MoveCanceled;
    }

    private void MoveCanceled(InputAction.CallbackContext context)
    {
        _moveDir = context.ReadValue<Vector2>();
        _movePressed = _moveDir.magnitude >= 0.01f;
    }

    private void MovePerformed(InputAction.CallbackContext context)
    {
        _moveDir = context.ReadValue<Vector2>();
        _movePressed = _moveDir.magnitude >= 0.01f;
    }

    public void SetMoveUpdate(Action<Vector2> onMove)
    {
        _currMoveUpdate = onMove;
    }
}
