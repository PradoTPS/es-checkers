using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalPieceTests 
{

    [Test]
    public void check_normal_piece_move_not_null()
    {
        //Arrange
        var gameObject = new GameObject();
        INormalPiece normalPiece = gameObject.AddComponent<NormalPiece>();

        //Act
        var result = normalPiece.Move();

        //Assert
        Assert.IsNotNull(result);
    }

    [Test]
    public void check_normal_piece_eat_not_null()
    {
        //Arrange
        var gameObject = new GameObject();
        INormalPiece normalPiece = normalPiece = gameObject.AddComponent<NormalPiece>();

        //Act
        var result = normalPiece.Eat();

        //Assert
        Assert.IsNotNull(result);
    }

}
