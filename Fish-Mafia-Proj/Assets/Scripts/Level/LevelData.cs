using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string LevelName;
    public Transform StartPoint;
    public KokonutHivemind Hivemind;

    public bool IsCleared()
    {
        if (Hivemind == null) return true;

        foreach (var agent in Hivemind.AllAgents)
        {
            if (agent != null && agent.gameObject.activeInHierarchy)
                return false;
        }

        return true;
    }
}