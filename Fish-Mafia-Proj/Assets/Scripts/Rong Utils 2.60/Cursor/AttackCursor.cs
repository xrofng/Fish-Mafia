using MoreMountains.Feedbacks;
using UnityEngine;

public class AttackCursor : BaseCursorBehaviour
{
    public MMF_Player player;
    public override void OnClick()
    {
        player.PlayFeedbacks();
        // Example: Raycast or spawn attack
    }
}