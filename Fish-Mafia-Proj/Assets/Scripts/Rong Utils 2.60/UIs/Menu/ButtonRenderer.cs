using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonRenderer : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    ISelectHandler,
    IDeselectHandler

{
    [SerializeField] Image TargeGraphic;
    [SerializeField] Button TargetButton;
    [SerializeField] Color NormalColor;
    [SerializeField] Color HighlightedColor;
    [SerializeField] Color PressedColor;
    [SerializeField] Color SelectedColor;
    [SerializeField] Color DisabledColor;

    private void Reset()
    {
        TargetButton = GetComponent<Button>();
    }

    protected virtual void Awake()
    {
        TargetButton = GetComponent<Button>();
    }

    protected virtual void Update()
    {
        // Detect disabled transition
        if (!TargetButton.interactable)
        {
            OnDisabled();
        }
    }

    private void OnDisabled()
    {
        TargeGraphic.color = DisabledColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        TargeGraphic.color = NormalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TargeGraphic.color = HighlightedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TargeGraphic.color = NormalColor;
    }

    public void OnSelect(BaseEventData eventData)
    {
        TargeGraphic.color = SelectedColor;
    }
}
