using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : Selectable
{

    public Board Board { private get => myBoard; set => myBoard = value; }
    public PieceAbstract MyPiece 
    { 
        get => myPiece; 
        set
        {
            myPiece = value;

            if(myPiece != null)
                myPiece.transform.position = PiecePosition.position;
        } 
    }
    public bool HasPiece { get => myPiece != null; }
    public Transform PiecePosition { get => piecePosition; set => piecePosition = value; }

    [HideInInspector] public int xPosInMat = -1;
    [HideInInspector] public int yPosInMat = -1;


    private PieceAbstract myPiece = null;
    private Board myBoard = null;

    [Header("Scene References")]
    [SerializeField] private Transform piecePosition = null;

    #region Unity functions
    protected override void OnMouseDown()
    {
        myBoard.SelectTile(this);
    }


    #endregion
    public void SetMatrixCoordinates(int x, int y)
    {
        xPosInMat = x;
        yPosInMat = y;
    }


}
