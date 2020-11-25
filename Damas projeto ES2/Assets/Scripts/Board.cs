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
    private List<Tile> possibleMovementSelected = new List<Tile>();

    //Funcion called by unity when object is created
    private void Awake()
    {
        InitializeMatrix();
        SpawnTiles();
        SpawnPieces();
    }


    public void SelectTile(Tile tileToSelect)
    {

        if (tileToSelect.HasPiece && !TurnosManager.Instance.CanDoAction(tileToSelect.MyPiece.Filiation))
            return;

        bool shouldMove = false;
        Tile tileToMove = null;

        if(currentSelectedTile == null)
        {
            currentSelectedTile = tileToSelect;
            tileToSelect.IsSelected = true;
        }
        else if(currentSelectedTile == tileToSelect)
        {
            currentSelectedTile.IsSelected = false;
            currentSelectedTile = null;
        }
        else
        {
            if (currentSelectedTile.HasPiece && !tileToSelect.HasPiece)
            {
                if (IsTileInMovementList(tileToSelect))
                {
                    tileToMove = currentSelectedTile;
                    shouldMove = true;
                }

            }
            else
            {
                currentSelectedTile.IsSelected = false;
                currentSelectedTile = tileToSelect;
                tileToSelect.IsSelected = true;
            }



             
        }


        ResetMovementSelectedTiles();
        if(currentSelectedTile != null && currentSelectedTile.HasPiece)
        {
            
            var movementList = GetPossibleMovementsList(currentSelectedTile);

            if (shouldMove)
            {
                DoMove(tileToMove, tileToSelect, movementList);
                TurnosManager.Instance.ChangeTurno();
                currentSelectedTile.IsSelected = false;
                currentSelectedTile = null;
                //ResetMovementSelectedTiles();
                return;
            }

            HighLightPossibleMovements(movementList);

        }


    }

    private bool IsTileInMovementList(Tile tileToSelect)
    {
        foreach(var tile in possibleMovementSelected)
        {
            if(tile == tileToSelect)
            {
                return true;
            }
        }

        return false;
    }

    private void HighLightPossibleMovements(List<MovementInBoard> possibleMovements)
    {
        if (possibleMovements == null)
            return;

        foreach(var movement in possibleMovements)
        {
            if (movement.HasTileToEat)
            {
                movement.eatenTile.State = SelectableColor.tileInCheck;
                possibleMovementSelected.Add(movement.eatenTile);

            }

            movement.endMovementTile.State = SelectableColor.movementSelected;
            possibleMovementSelected.Add(movement.endMovementTile);

        }
    }

    private void ResetMovementSelectedTiles()
    {

        foreach(var tile in possibleMovementSelected)
        {
            tile.State = SelectableColor.initial;
        }

        possibleMovementSelected.Clear();
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
            float lineX = initialPosition.x + 1 * i;
            for(int j = 0; j < matrixHeight; j++)
            {
                float lineY = initialPosition.y + 1 * j;

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
        SpawnPieceInThisSubMatrix(0, matrixHeight/2 - 1, 0, matrixWidth, PieceFiliation.player1);
        //Spawn other side
        SpawnPieceInThisSubMatrix(matrixHeight/2 + 1, matrixHeight, 0, matrixWidth, PieceFiliation.player2);

    }

    private void SpawnPieceInThisSubMatrix(int linhaI, int linhaF, int colunaI, int colunaF, PieceFiliation filiation)
    {
        for(int i = colunaI; i < colunaF; i++)
        {
            for(int j = linhaI; j < linhaF; j++)
            {
                if ((i + j) % 2 == 0)
                {

                    Tile tileToPutInto = tilesMatrix[i, j];
                    Vector2 positionToSpawn = tileToPutInto.PiecePosition.position;

                    GameObject prefabToInstantiate = null;
                    prefabToInstantiate = filiation == PieceFiliation.player1 ? darkPiece : lightPiece;


                    PieceAbstract piece = Instantiate(prefabToInstantiate, positionToSpawn, Quaternion.identity).GetComponent<PieceAbstract>();
                    piece.Filiation = filiation;
                    tileToPutInto.MyPiece = piece;

                    GameManager.Instance.RegisterPiece(piece);
                }
                    

            }
        }
    }

    private List<MovementInBoard> GetPossibleMovementsList(Tile tileToGetPieceFrom)
    {
        if (tileToGetPieceFrom.MyPiece == null)
        {
            Debug.LogWarning("[Board] Calling MovePiece with incorrect configuration");
            return null;
        }


        var positionsList = GetPositionsToMove(tileToGetPieceFrom);

        return positionsList;
    }

    public void DoMove(Tile tileToGetPieceFrom, Tile tileToPutPieceOn, List<MovementInBoard> positionsList)
    {

        if (positionsList == null)
            return;

        MovementInBoard movementToMake = GetMovementFromListWithTile(positionsList, tileToPutPieceOn);
        if (movementToMake != null)
        {

            if (movementToMake.HasTileToEat)
            {
                Tile eatenTile = movementToMake.eatenTile;

                PlacarManager.Instance.PieceEaten(eatenTile.MyPiece);
                GameManager.Instance.UnregisterPiece(eatenTile.MyPiece);
                
                Destroy(eatenTile.MyPiece.gameObject);
                eatenTile.MyPiece = null;
            }


            if(tileToGetPieceFrom.MyPiece.Filiation == PieceFiliation.player1)
            {
                if (tileToPutPieceOn.yPosInMat == matrixHeight - 1)
                {
                    tileToPutPieceOn.MyPiece = tileToGetPieceFrom.MyPiece.Promote();
                }
                else
                {
                    tileToPutPieceOn.MyPiece = tileToGetPieceFrom.MyPiece;
                }

            }
            else
            {
                if (tileToPutPieceOn.yPosInMat == 0)
                {
                    tileToPutPieceOn.MyPiece = tileToGetPieceFrom.MyPiece.Promote();
                }
                else
                {
                    tileToPutPieceOn.MyPiece = tileToGetPieceFrom.MyPiece;
                }
            }

            tileToGetPieceFrom.MyPiece = null;

            positionsList.Remove(movementToMake);
        }
    }

    public List<MovementInBoard> GetPositionsToMove(Tile tileWithPiece)
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

    public List<MovementInBoard> CanEatAny(Tile tileWithPiece)
    {
        var movementList = new List<MovementInBoard>();
        var tilePos = new Vector2Int(tileWithPiece.xPosInMat, tileWithPiece.yPosInMat);

        var listToEat = tileWithPiece.MyPiece.Eat();
        foreach(var eatPos in listToEat)
        {
            EatRecursive(tilePos, eatPos, movementList, tileWithPiece.MyPiece.Filiation);
        }




        return movementList;
    }

    private void EatRecursive(Vector2Int currentCheckPos, PieceEatV2 eatPos, List<MovementInBoard> movementList, PieceFiliation eaterFiliation)
    {
        var afterEatingPos = currentCheckPos + new Vector2Int(eatPos.x, eatPos.y);

        if (IsInBoardRanges(afterEatingPos))
        {
            var otherPiecePos = currentCheckPos + new Vector2Int(eatPos.otherPieceX, eatPos.otherPieceY);
            
            if (!HasPieceInPos(afterEatingPos) && HasPieceInPos(otherPiecePos))
            {
                var otherPieceTile = tilesMatrix[otherPiecePos.x, otherPiecePos.y];
                if(otherPieceTile.MyPiece.Filiation != eaterFiliation)
                {
                    Tile moveTo = tilesMatrix[afterEatingPos.x, afterEatingPos.y];
                    Tile eatenPiece = tilesMatrix[otherPiecePos.x, otherPiecePos.y];

                    movementList.Add(new MovementInBoard(moveTo, eatenPiece));
                }
            }
            else
            {
          
                if(tilesMatrix[afterEatingPos.x, afterEatingPos.y].MyPiece?.Filiation != eaterFiliation && eatPos.recursive)
                    EatRecursive(otherPiecePos, eatPos, movementList, eaterFiliation);
            }
        }
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

    private MovementInBoard GetMovementFromListWithTile(List<MovementInBoard> list, Tile tile)
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
