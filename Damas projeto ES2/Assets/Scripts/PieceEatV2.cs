using UnityEngine;

public class PieceEatV2
{


    public int x;
    public int y;
    public int otherPieceX;
    public int otherPieceY;

    public bool recursive;


    public PieceEatV2(int x, int y, int otherX, int otherY)
    {
        this.x = x;
        this.y = y;

        this.otherPieceX = otherX;
        this.otherPieceY = otherY;
    }

    public PieceEatV2(int x, int y, int otherX, int otherY, bool recursive)
    {
        this.x = x;
        this.y = y;

        this.otherPieceX = otherX;
        this.otherPieceY = otherY;

        this.recursive = recursive;
    }


}
