using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MoreMountains.Tools.MMSingleton<CursorController>
{
    public List<CursorState> States;
    public SpriteRenderer CursorRenderer;
    public Camera MainCamera;

    protected CursorState currentState;
    protected float stateTime;
    private BaseCursorBehaviour behavior;
    protected InputController inputController;
    private InputSystem_Actions input => inputController.Input;

    protected override void Awake()
    {
        base.Awake();
        inputController = FindAnyObjectByType<InputController>();
        Cursor.visible = false;

        if (MainCamera == null)
            MainCamera = Camera.main;

        if (States.Count > 0)
            SetState(States[0].StateName);

        behavior = GetComponent<BaseCursorBehaviour>();
    }

    private void OnEnable()
    {
        input.Player.LeftAttack.performed += OnClick;
    }

    private void OnDisable()
    {
        input.Player.LeftAttack.performed -= OnClick;
    }

    private void Update()
    {
        UpdatePosition();
    }

    public Vector2 GetMouseWorldPos(Camera cam, float zPlane = 0f)
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mouseScreen);

        Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, zPlane));

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector2.zero;
    }

    protected virtual void UpdatePosition()
    {
        Vector2 worldPos = GetMouseWorldPos(MainCamera);
        transform.position = worldPos;
    }

    public virtual void SetState(string stateName)
    {
        var newState = States.Find(s => s.StateName == stateName);
        if (newState == null)
        {
            Debug.LogWarning($"Cursor state not found: {stateName}");
            return;
        }

        currentState = newState;
        CursorRenderer.sprite = currentState.GetFrame();
        stateTime = 0f;
    }

    protected virtual void OnClick(InputAction.CallbackContext context)
    {
        behavior.OnClick();
    }
}