using System.Collections.Generic;
using UnityEngine;

public class MoveExecutor : MonoBehaviour
{
    public void Execute(MoveData move)
    {
        // play animation
        // enable hitbox
        // apply movement

        Debug.Log("Playing move: " + move.moveName);
    }
}