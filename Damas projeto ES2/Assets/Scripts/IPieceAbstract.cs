using System.Collections.Generic;

public interface IPieceAbstract
{
    PieceFiliation Filiation { get; set; }
    bool HasSameFiliation(PieceAbstract otherPiece);
    List<PieceEatV2> Eat();
    List<PieceMoveV2> Move();
}