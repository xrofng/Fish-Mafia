using TMPro;
using UnityEngine;
using Xrofng;

public class BaseTextUI : BaseFadeView
{
    [SerializeField] protected TextMeshProUGUI TextMesh;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateWhileVisible()
    {
        base.UpdateWhileVisible();
        TextMesh.text = GetText();
    }

    protected virtual string GetText()
    {
        return TextMesh.text;
    }
}