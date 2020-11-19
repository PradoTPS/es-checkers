using System.Collections.Generic;

public interface INormalPiece
{
    List<PieceEatV2> Eat();
    List<PieceMoveV2> Move();
}