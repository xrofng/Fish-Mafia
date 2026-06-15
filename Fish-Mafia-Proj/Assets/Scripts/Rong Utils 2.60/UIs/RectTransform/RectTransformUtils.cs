using UnityEngine;

public static class RectTransformUtils
{
    // -----------------------------
    // Size
    // -----------------------------

    public static void SetWidth(RectTransform rt, float width)
    {
        if (!rt) return;

        Vector2 size = rt.sizeDelta;
        size.x = width;
        rt.sizeDelta = size;
    }

    public static void SetHeight(RectTransform rt, float height)
    {
        if (!rt) return;

        Vector2 size = rt.sizeDelta;
        size.y = height;
        rt.sizeDelta = size;
    }

    public static void SetSize(RectTransform rt, Vector2 size)
    {
        if (!rt) return;
        rt.sizeDelta = size;
    }

    public static void SetSize(RectTransform rt, float width, float height)
    {
        if (!rt) return;
        rt.sizeDelta = new Vector2(width, height);
    }

    // -----------------------------
    // Anchored Position
    // -----------------------------

    public static void SetAnchoredX(RectTransform rt, float x)
    {
        if (!rt) return;

        Vector2 pos = rt.anchoredPosition;
        pos.x = x;
        rt.anchoredPosition = pos;
    }

    public static void SetAnchoredY(RectTransform rt, float y)
    {
        if (!rt) return;

        Vector2 pos = rt.anchoredPosition;
        pos.y = y;
        rt.anchoredPosition = pos;
    }

    // -----------------------------
    // Stretch-safe helpers
    // (works even when anchors stretch)
    // -----------------------------

    public static void SetLeft(RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}
