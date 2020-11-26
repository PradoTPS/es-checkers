using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            Debug.LogWarning("No tiles to move");
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
        bool foundEatingTile = false;
        listToStoreMovements = new List<MovementInBoard>();
        var tileDic = new Dictionary<Tile, List<MovementInBoard>>();


        foreach(var tile in board.tilesMatrix)
        {
            if (tile.HasPiece && tile.MyPiece.Filiation == myFiliation)
            {
                var thisTileMove = board.CanEatAny(tile);
                if (thisTileMove.Count > 0)
                {
                    tileDic.Add(tile, thisTileMove);
                    foundEatingTile = true;
                    break;
                }
            }
        }


        if(foundEatingTile == false)
        {
            foreach(var tile in board.tilesMatrix)
            {
                if (tile.HasPiece && tile.MyPiece.Filiation == myFiliation)
                {
                    var moveList = board.GetPositionsToMove(tile);

                    if (moveList.Count > listToStoreMovements.Count)
                    {
                        tileDic.Add(tile, moveList);
                    }
                }
            }
        }

        if (tileDic.Count == 0)
        {
            return null;
        }

        int randInt = Random.Range(0, tileDic.Count);
        var randomTile = tileDic.Keys.ToList()[randInt];

        listToStoreMovements = tileDic[randomTile];
        return randomTile;

    }

}
