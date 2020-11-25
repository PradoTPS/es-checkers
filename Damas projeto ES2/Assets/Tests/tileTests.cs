using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class tileTests
{

    [Test]
    public void tile_test_set_coordinates()
    {

        // Arrange
        var gameObject = new GameObject();
        Tile tile = gameObject.AddComponent<Tile>();

        //Act
        int xCoord = 1;
        int yCoord = 1;
        tile.SetMatrixCoordinates(xCoord, yCoord);

        //Assert
        Assert.AreEqual(xCoord, tile.xPosInMat);
        Assert.AreEqual(yCoord, tile.yPosInMat);

    }


}
