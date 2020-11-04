using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PieceAbstract : MonoBehaviour
{

    public PieceFiliation filiation;
    public abstract List<PieceMoveV2> Move();
    public abstract List<PieceEatV2> Eat();

    public bool HasSameFiliation(PieceAbstract otherPiece)
    {
        return filiation == otherPiece.filiation;
    }

}
