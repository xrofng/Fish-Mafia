using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class MenuButtonBase<T> : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    ISelectHandler,
    IDeselectHandler
{
    protected T data;
    protected Button button;

    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);
    }

    public virtual void Setup(T data)
    {
        this.data = data;
        Refresh();
    }

    public void SetInteractable(bool v)
    {
        button.interactable = v;
    }

    protected virtual void Update()
    {
        // Detect disabled transition
        if (!button.interactable)
        {
            OnDisabled();
        }
    }

    // -------------------------
    // Unity UI Events
    // -------------------------

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
            OnHighlighted();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnUnhighlighted();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (button.interactable)
            OnSelected();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        OnDeselected();
    }

    // -------------------------
    // Virtual Hooks
    // -------------------------

    protected virtual void OnHighlighted() { }
    protected virtual void OnUnhighlighted() { }

    protected virtual void OnSelected() { }
    protected virtual void OnDeselected() { }

    protected virtual void OnDisabled() { }

    // -------------------------

    protected abstract void Refresh();
    protected abstract void HandleClick();
}
