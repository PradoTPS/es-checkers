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
        PieceAbstract normalPiece = gameObject.AddComponent<NormalPiece>();

        //Act
        normalPiece.Filiation = PieceFiliation.player1;
        var result = normalPiece.Move();
        Assert.IsNotNull(result);

        normalPiece.Filiation = PieceFiliation.player2;
        var result2 = normalPiece.Move();
        Assert.IsNotNull(result);
        //Assert
    }

    [Test]
    public void check_normal_piece_eat_not_null()
    {
        //Arrange
        var gameObject = new GameObject();
        PieceAbstract normalPiece = normalPiece = gameObject.AddComponent<NormalPiece>();

        //Act
        var result = normalPiece.Eat();

        //Assert
        Assert.IsNotNull(result);
    }

}
