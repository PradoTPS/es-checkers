public class PiecePositionV2
{

    public int x;
    public int y;
    public bool recursive;

    public PiecePositionV2(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.recursive = false;
    }

    public PiecePositionV2(int x, int y, bool recursive)
    {
        this.x = x;
        this.y = y;
        this.recursive = recursive;
    }

}
