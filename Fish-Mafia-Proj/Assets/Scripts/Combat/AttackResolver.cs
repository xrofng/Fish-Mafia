using Combat2D;
using UnityEngine;

public class AttackResolver
{
    public MoveData ResolveFromNeutral(string inpurActionName, PlayerStateMain.InputHoldDirection direction, MoveData[] moves)
    {
        foreach (var move in moves)
        {
            if (move.requiredInput.ToString() == inpurActionName && move.requiredDirection == direction)
                return move;
        }

        return null;
    }

    //public MoveData ResolveCombo(PlayerStateMain.InputHoldDirection direction, MoveData currentMove)
    //{
    //    foreach (var move in currentMove.nextMoves)
    //    {
    //        if (move.directionRequirement == direction) { 
    //        }
    //            return move;
    //    }

    //    return null;
    //}

    public MoveData ResolveCombo(MoveData currentMove, string inputName, PlayerStateMain.InputHoldDirection inputDir = PlayerStateMain.InputHoldDirection.None )
    {
        if (currentMove == null) return null;

        foreach (var branch in currentMove.comboBranches)
        {
            if (branch.RequiredInput.ToString() == inputName)
            {
                return branch.nextMove;
            }
        }

        return null;
    }
}
