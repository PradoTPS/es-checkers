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


    [SerializeField] private Tile selectedTile = null;


    [Header("Scene References")]
    [SerializeField] private Transform initialSpawnPoint = null;
    [SerializeField] private GameObject darkTilePrefab = null;
    [SerializeField] private GameObject lightTilePrefab = null;

    //Funcion called by unity when object is created
    private void Awake()
    {
        InitializeMatrix();
        SpawnTiles();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectTilesToMove();
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

                instantiatedTile.SetMatrixCoordinates(i, j);
                tilesMatrix[i, j] = instantiatedTile;

            }
        }
    }

    private void SelectTilesToMove()
    {
        
        List<PiecePositionV2> moveList = selectedTile.myPiece?.Move();

        foreach(var piecePosition in moveList)
        {
            Vector2 currentTile = new Vector2(selectedTile.xPosInMat, selectedTile.yPosInMat);

            SelectRecursive(currentTile, piecePosition);

        }

    }


    private void SelectRecursive(Vector2 currentTile, PiecePositionV2 piecePosition)
    {

        currentTile.x += piecePosition.x;
        currentTile.y += piecePosition.y;

        if (IsInBoardRanges(currentTile))
        {
            SelectTile(currentTile);
            if (piecePosition.recursive)
            {

                SelectRecursive(currentTile, piecePosition);
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



}
