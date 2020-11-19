using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pieceTest 
{

    [Test]
    public void check_normal_piece_move_not_null()
    {
        //Arrange
        INormalPiece normalPiece = Substitute.For<INormalPiece>();

        //Act
        var result = normalPiece.Move();

        //Assert
        Assert.IsNotNull(result);
    }

    [Test]
    public void check_normal_piece_eat_not_null()
    {
        //Arrange
        INormalPiece normalPiece = Substitute.For<INormalPiece>();

        //Act
        var result = normalPiece.Eat();

        //Assert
        Assert.IsNotNull(result);
    }

}
