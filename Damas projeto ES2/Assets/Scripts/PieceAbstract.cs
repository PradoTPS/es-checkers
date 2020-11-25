using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PieceAbstract : MonoBehaviour, IPieceAbstract
{

    public PieceFiliation Filiation { get => filiation; set => filiation = value; }
    
    [SerializeField] private PieceAbstract promoteTo = null;
    private PieceFiliation filiation;

    public abstract List<PieceMoveV2> Move();
    public abstract List<PieceEatV2> Eat();

    public PieceAbstract Promote()
    {
        return SpawnPromotedAndCopyInfos();
    }

    public bool HasSameFiliation(PieceAbstract otherPiece)
    {
        return Filiation == otherPiece.Filiation;
    }

    private PieceAbstract SpawnPromotedAndCopyInfos()
    {

        if (promoteTo == null)
            return this;

        var promoted = Instantiate(promoteTo).GetComponent<PieceAbstract>();

        promoted.transform.position = this.transform.position;
        promoted.Filiation = this.filiation;

        DestroyAfterFrame();

        return promoted;
    }

    private IEnumerator DestroyAfterFrame()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

}
