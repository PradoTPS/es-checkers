using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour
{

    
    [SerializeField] PieceFiliation myFiliation = PieceFiliation.player2;
    TurnosManager turnosManager = null;

    //Members
    [SerializeField] private Board board = null;

    private bool doingAction = false;


    private void Awake()
    {
        turnosManager = TurnosManager.Instance;
    }

    private void Update()
    {
        if (!doingAction && turnosManager.CanDoAction(myFiliation))
        {
            doingAction = true;
            StartCoroutine(DoTileMove());
        }
    }


    private IEnumerator DoTileMove()
    {

        Debug.Log("Começou movimento IA");

        List<MovementInBoard> movementList;
        Tile tileToMove = GetTileToMove(out movementList);

        if (tileToMove == null)
        {
            Debug.LogError("No tiles to move");
            yield break;
        }

        int randomNumber = Random.Range(0, movementList.Count);
        var randomMovement = movementList[randomNumber];


        board.SelectTile(tileToMove);
        yield return new WaitForSeconds(1.5f);
        board.SelectTile(randomMovement.endMovementTile);

        doingAction = false;
    }

    private Tile GetTileToMove(out List<MovementInBoard> listToStoreMovements)
    {
        Tile tileToMove = null;
        listToStoreMovements = new List<MovementInBoard>();
        foreach(var tile in board.tilesMatrix)
        {
            if (tile.HasPiece && tile.MyPiece.Filiation == myFiliation)
            {
                var thisTileMove = board.CanEatAny(tile);
                if (thisTileMove.Count > 0)
                {
                    tileToMove = tile;
                    listToStoreMovements = thisTileMove;
                    break;
                }
            }
        }


        if(tileToMove == null)
        {
            foreach(var tile in board.tilesMatrix)
            {
                if (tile.HasPiece && tile.MyPiece.Filiation == myFiliation)
                {
                    var moveList = board.GetPositionsToMove(tile);

                    if (moveList.Count > listToStoreMovements.Count)
                    {
                        listToStoreMovements = moveList;
                        tileToMove = tile;
                    }
                }
            }
        }


        return tileToMove;

    }

}
