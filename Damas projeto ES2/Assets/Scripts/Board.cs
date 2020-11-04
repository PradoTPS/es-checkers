using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    [HideInInspector] public Tile[,] tilesMatrix = null;

    [Header("Matrix info")]
    [SerializeField] private int matrixWidth = 8;
    [SerializeField] private int matrixHeight = 8;


    private Tile selectedTile = null;
    private Tile inCheckTile = null;


    [Header("Scene References")]
    [SerializeField] private Transform initialSpawnPoint = null;

    [Header("Tile Prefab References")]
    [SerializeField] private GameObject darkTilePrefab = null;
    [SerializeField] private GameObject lightTilePrefab = null;

    [Header("Piece Prefab References")]
    [SerializeField] private GameObject darkPiece = null;
    [SerializeField] private GameObject lightPiece = null;

    private Tile currentSelectedTile = null;

    //Funcion called by unity when object is created
    private void Awake()
    {
        InitializeMatrix();
        SpawnTiles();
        SpawnPieces();
    }


    public void SelectTile(Tile tileToSelect)
    {

        if(currentSelectedTile == null)
        {
            currentSelectedTile = tileToSelect;
            tileToSelect.IsSelected = true;
        }
        else
        {
            if (currentSelectedTile.HasPiece)
            {
                if (!tileToSelect.HasPiece)
                {
                    TryMovePieceFromTileToOther(currentSelectedTile, tileToSelect);
                }
            }
             

            currentSelectedTile.IsSelected = false;

            currentSelectedTile = tileToSelect;
            tileToSelect.IsSelected = true;
        }
    }

    private void InitializeMatrix()
    {
        tilesMatrix = new Tile[matrixWidth, matrixHeight];
    }

    private void SpawnTiles()
    {
        Vector2 initialPosition = initialSpawnPoint.position;
        GameObject prefabToIntantiate = null;

        for(int i = 0; i < matrixWidth; i++)
        {
            float lineY = initialPosition.y + 1 * i;
            for(int j = 0; j < matrixHeight; j++)
            {
                float lineX = initialPosition.x + 1 * j;

                //Se i+j for par, usa o darkTile, caso contrário usa o light
                prefabToIntantiate = (i + j) % 2 == 0 ? darkTilePrefab : lightTilePrefab;

                Vector2 positionToInstantiate = new Vector2(lineX, lineY);

                GameObject instantiatedObject = Instantiate(prefabToIntantiate, positionToInstantiate, Quaternion.identity);
                Tile instantiatedTile = instantiatedObject.GetComponent<Tile>();
                instantiatedTile.Board = this;

                instantiatedTile.SetMatrixCoordinates(i, j);
                tilesMatrix[i, j] = instantiatedTile;


            }
        }
    }

    private void SpawnPieces()
    {

        // Spawn one side
        SpawnPieceInThisSubMatrix(0, 3, 0, matrixWidth, PieceFiliation.player1);
        //Spawn other side
        SpawnPieceInThisSubMatrix(5, matrixHeight, 0, matrixWidth, PieceFiliation.player2);

    }

    private void SpawnPieceInThisSubMatrix(int linhaI, int linhaF, int colunaI, int colunaF, PieceFiliation filiation)
    {
        for(int i = linhaI; i < linhaF; i++)
        {
            for(int j = colunaI; j < colunaF; j++)
            {
                if ((i + j) % 2 == 0)
                {

                    Tile tileToPutInto = tilesMatrix[i, j];
                    Vector2 positionToSpawn = tileToPutInto.PiecePosition.position;

                    GameObject prefabToInstantiate = null;
                    prefabToInstantiate = filiation == PieceFiliation.player1 ? darkPiece : lightPiece;


                    PieceAbstract piece = Instantiate(prefabToInstantiate, positionToSpawn, Quaternion.identity).GetComponent<PieceAbstract>();
                    piece.filiation = filiation;
                    tileToPutInto.MyPiece = piece;

                }
                    

            }
        }
    }

    private void TryMovePieceFromTileToOther(Tile tileToGetPieceFrom, Tile tileToPutPieceOn)
    {
        if (tileToGetPieceFrom.MyPiece == null || tileToPutPieceOn.MyPiece != null)
        {
            Debug.LogWarning("[Board] Calling MovePiece with incorrect configuration");
            return;
        }


        var positionsList = GetPositionsToMove(tileToGetPieceFrom);
        MovementInBoard movementToMake = GetMovementFromTileInList(positionsList, tileToPutPieceOn);

        if (movementToMake != null)
        {

            if (movementToMake.HasTileToEat)
            {
                Tile eatenTile = movementToMake.eatenTile;
                Destroy(eatenTile.MyPiece.gameObject);
                eatenTile.MyPiece = null;
            }
            
            tileToPutPieceOn.MyPiece = tileToGetPieceFrom.MyPiece;
            tileToGetPieceFrom.MyPiece = null;
        }
    }

    private List<MovementInBoard> GetPositionsToMove(Tile tileWithPiece)
    {

        List<MovementInBoard> tilesToMoveTo = new List<MovementInBoard>();
        List<PieceMoveV2> moveList = tileWithPiece?.MyPiece?.Move();

        var piecesToEat = CanEatAny(tileWithPiece);
        if (piecesToEat.Count != 0)
        {
            foreach(var movement in piecesToEat)
            {
                tilesToMoveTo.Add(movement);
            }
            return tilesToMoveTo;
        }

        foreach(var piecePosition in moveList)
        {
            Vector2Int currentTile = new Vector2Int(tileWithPiece.xPosInMat, tileWithPiece.yPosInMat);

            FindMovementRecursive(currentTile, piecePosition, tilesToMoveTo);

        }

        return tilesToMoveTo;

    }

    private void FindMovementRecursive(Vector2Int currentTile, PieceMoveV2 piecePosition, List<MovementInBoard> tilesToMoveTo)
    {

        currentTile.x += piecePosition.x;
        currentTile.y += piecePosition.y;

        if (IsInBoardRanges(currentTile))
        {

            if (!HasPieceInPos(currentTile))
            {
                //SelectTile(currentTile);
                Tile current = tilesMatrix[currentTile.x, currentTile.y];
                tilesToMoveTo.Add(new MovementInBoard(current));

            }
            else
                return;

            if (piecePosition.recursive)
            {

                FindMovementRecursive(currentTile, piecePosition, tilesToMoveTo);
            }
        }

    }

    private List<MovementInBoard> CanEatAny(Tile tileWithPiece)
    {
        var movementList = new List<MovementInBoard>();
        var tilePos = new Vector2Int(tileWithPiece.xPosInMat, tileWithPiece.yPosInMat);
        
        var listToEat = tileWithPiece.MyPiece.Eat();
        foreach(var eatPos in listToEat)
        {

            var movementAfterEatingPos = tilePos + new Vector2Int(eatPos.x, eatPos.y);
            if (IsInBoardRanges(movementAfterEatingPos) && !HasPieceInPos(movementAfterEatingPos))
            {
                var otherPiecePos = tilePos + new Vector2Int(eatPos.otherPieceX, eatPos.otherPieceY);
                var otherPieceTile = tilesMatrix[otherPiecePos.x, otherPiecePos.y];
                if (HasPieceInPos(otherPiecePos)
                    && !tileWithPiece.MyPiece
                    .HasSameFiliation(otherPieceTile.MyPiece))
                {
                    Tile moveTo = tilesMatrix[movementAfterEatingPos.x, movementAfterEatingPos.y];
                    Tile eatenPiece = tilesMatrix[otherPiecePos.x, otherPiecePos.y];

                    movementList.Add(new MovementInBoard(moveTo, eatenPiece));
                }
            }

        }

        return movementList;
    }

    private void SelectTile(Vector2 tileToSelect)
    {
        SelectTile(tileToSelect.x, tileToSelect.y);
    }

    private void SelectTile(float tileToSelectX, float tileToSelectY)
    {
        Debug.Log("Selected: " + (tileToSelectX) + " " + (tileToSelectY));
    }

    private bool IsInBoardRanges(float x, float y)
    {
        if (x < 0 || y < 0)
            return false;

        return  (x < this.matrixWidth) && (y < this.matrixHeight);
    }

    private bool IsInBoardRanges(Vector2 tilePos)
    {
        return IsInBoardRanges(tilePos.x, tilePos.y);
    }

    private bool IsInBoardRanges(Tile tile)
    {
        return IsInBoardRanges(tile.xPosInMat, tile.yPosInMat);
    }

    private MovementInBoard GetMovementFromTileInList(List<MovementInBoard> list, Tile tile)
    {
        foreach(var movement in list)
        {
            if (tile.xPosInMat == movement.endMovementTile.xPosInMat && tile.yPosInMat == movement.endMovementTile.yPosInMat)
            {
                return movement;
            }
        }

        return null;
    }

    private bool HasPieceInPos(Vector2 pos)
    {
        return tilesMatrix[(int) pos.x, (int) pos.y].MyPiece != null;
    }

    private bool CanMoveTo(Tile tile)
    {
        return !tile.HasPiece && IsInBoardRanges(tile);
    }

}

public enum PieceFiliation
{
    player1,
    player2
}

public class MovementInBoard
{


    public bool HasTileToEat { get => eatenTile != null; }
    public Tile endMovementTile;
    public Tile eatenTile;


    public MovementInBoard(Tile endMovementTile)
    {
        this.endMovementTile = endMovementTile;
        eatenTile = null;
    }

    public MovementInBoard(Tile endMovementTile, Tile eatenTile)
    {
        this.endMovementTile = endMovementTile;
        this.eatenTile = eatenTile;
    }

}
