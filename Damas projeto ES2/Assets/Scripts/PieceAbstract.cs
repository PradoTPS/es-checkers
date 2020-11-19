using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PieceAbstract : MonoBehaviour, IPieceAbstract
{

    private PieceFiliation filiation;

    public PieceFiliation Filiation { get => filiation; set => filiation = value; }

    public abstract List<PieceMoveV2> Move();
    public abstract List<PieceEatV2> Eat();

    public bool HasSameFiliation(PieceAbstract otherPiece)
    {
        return Filiation == otherPiece.Filiation;
    }

}
