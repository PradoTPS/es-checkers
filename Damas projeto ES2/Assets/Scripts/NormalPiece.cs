using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPiece : PieceAbstract, INormalPiece
{

    public override List<PieceMoveV2> Move()
    {
        List<PieceMoveV2> moveOptionsList = new List<PieceMoveV2>();

        moveOptionsList.Add(new PieceMoveV2(-1, -1));
        moveOptionsList.Add(new PieceMoveV2(-1, 1));
        moveOptionsList.Add(new PieceMoveV2(1, -1));
        moveOptionsList.Add(new PieceMoveV2(1, 1));

        return moveOptionsList;
    }
    public override List<PieceEatV2> Eat()
    {
        List<PieceEatV2> eatOptionsList = new List<PieceEatV2>();

        eatOptionsList.Add(new PieceEatV2(-2, -2, -1, -1));
        eatOptionsList.Add(new PieceEatV2(-2, 2, -1, 1));
        eatOptionsList.Add(new PieceEatV2(2, -2, 1, -1));
        eatOptionsList.Add(new PieceEatV2(2, 2, 1, 1));

        return eatOptionsList;
    }
}
