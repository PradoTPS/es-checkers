using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [HideInInspector] public int xPosInMat = -1;
    [HideInInspector] public int yPosInMat = -1;

    public PieceAbstract myPiece = null;

    //Function called by Unity when mouse is over the object
    private void OnMouseEnter()
    {
        //change color to hover
    }

    //Function called by Unity when mouse exits the object
    private void OnMouseExit()
    {
        //change color to normal
    }

    //Function called by Unity when mouse in clicked over the object
    private void OnMouseDown()
    {
        //Call select function on board
    }

    public void SetMatrixCoordinates(int x, int y)
    {
        xPosInMat = x;
        yPosInMat = y;
    }
}
