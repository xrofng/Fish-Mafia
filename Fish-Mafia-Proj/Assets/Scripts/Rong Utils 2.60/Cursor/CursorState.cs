using UnityEngine;

[System.Serializable]
public class CursorState
{
    public string StateName;
    public Sprite[] Frames;
    public float FrameRate = 10f;

    public bool IsAnimated => Frames != null && Frames.Length > 1;

    public Sprite GetFrame(float time = 0)
    {
        if (Frames == null || Frames.Length == 0) return null;

        if (!IsAnimated) return Frames[0];

        int index = Mathf.FloorToInt(time * FrameRate) % Frames.Length;
        return Frames[index];
    }
}