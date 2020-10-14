using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPiece : PieceAbstract
{
    public override List<PiecePositionV2> Move()
    {
        List<PiecePositionV2> moveOptionsList = new List<PiecePositionV2>();

        moveOptionsList.Add(new PiecePositionV2(-1, -1, true));
        moveOptionsList.Add(new PiecePositionV2(-1, 1, true));
        moveOptionsList.Add(new PiecePositionV2(1, -1, true));
        moveOptionsList.Add(new PiecePositionV2(1, 1, true));

        return moveOptionsList;
    }

    public override PiecePositionV2 Eat()
    {
        throw new System.NotImplementedException();
    }

}
