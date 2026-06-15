using Combat2D;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using Xrofng;

public class InputKeyDetectorPopUp : BaseFadeView
{
    [Header("Obj Ref")]
    public TextMeshProUGUI text;

    Coroutine currentRoutine;
    InputSystem_Actions input => inputController.Input;

    private Action<string, InputAction.CallbackContext> _onAnyAction;
    private InputController inputController;
    private List<InputAction> _actions;

    protected override void Awake()
    {
        base.Awake();
        text.text = "";
        inputController = FindAnyObjectByType<InputController>();
        _actions = new List<InputAction>();
        _actions.Add(input.Player.LeftAttack);
        _actions.Add(input.Player.Shoot);
        _actions.Add(input.Player.Move);
        _actions.Add(input.Player.Dodge);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        // Subscribe to actions in Player map
        foreach (var action in _actions)
        {
            action.performed += OnAnyActionPerformed;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        // Unsubscribe to actions in Player map
        foreach (var action in _actions)
        {
            action.performed -= OnAnyActionPerformed;
        }
    }

    // -------------------------
    // Internal
    // -------------------------
    private void OnAnyActionPerformed(InputAction.CallbackContext ctx)
    {
        Show(ctx.control.displayName + " | " + ctx.action.name);
    }

    // -------------------------
    // Core Logic
    // -------------------------
    void Show(string value)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        text.text = value;
        FadeToAlpha(1);
    }
}
