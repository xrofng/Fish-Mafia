using UnityEngine;

[CreateAssetMenu(
    fileName = "MechanicSetting",
    menuName = "Game/Settings/Mechanic Setting"
)]
public class MechanicSettingSO : ScriptableObject
{
    //[Header("General")]
    //public bool EnableDebugMode;

    //[Header("Gameplay")]
    //public bool GodMode;
    //public bool SkipTutorial;
    //public bool UnlockAll;

    //[Header("Debug Visuals")]
    //public bool ShowDebugUI;
    //public bool ShowGrid;
    //public bool ShowFPS;

    //[Header("Logging")]
    //public bool EnableDebugLog;

    [Header("Gameplay Debug")]
    public bool OneHitKill;

    [Header("Mechanic")]
    public bool ActivateEnemiesOfNearbyFloor = true;
}