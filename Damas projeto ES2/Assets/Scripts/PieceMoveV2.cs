public class PieceMoveV2
{

    public int x;
    public int y;
    public bool recursive;

    public PieceMoveV2(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.recursive = false;
    }

    public PieceMoveV2(int x, int y, bool recursive)
    {
        this.x = x;
        this.y = y;
        this.recursive = recursive;
    }

}
