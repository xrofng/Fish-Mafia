using UnityEngine;
using System.Text.RegularExpressions;

public class ObjectNameGiver : MonoBehaviour
{
    public string ObjectName = "ObjectName";

    void OnValidate()
    {
        RenameObject();
    }

    void RenameObject()
    {
        string currentName = gameObject.name;

        // Find last number in the name
        Match match = Regex.Match(currentName, @"(\d+)(?!.*\d)");

        string variantNumber = "0";

        if (match.Success)
        {
            variantNumber = match.Value;
        }

        gameObject.name = $"{ObjectName}_{variantNumber}_Envi";
    }
}