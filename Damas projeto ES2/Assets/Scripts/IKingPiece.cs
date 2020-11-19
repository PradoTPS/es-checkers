using System.Collections;
using System.Collections.Generic;

public interface IKingPiece
{
    List<PieceEatV2> Eat();
    List<PieceMoveV2> Move();
}