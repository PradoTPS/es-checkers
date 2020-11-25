using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kingPieceTests
{

    [Test]
    public void check_king_piece_move_not_null()
    {
        //Arrange
        var gameObject = new GameObject();
        IKingPiece normalPiece = gameObject.AddComponent<KingPiece>();

        //Act
        var result = normalPiece.Move();

        //Assert
        Assert.IsNotNull(result);
    }

    [Test]
    public void check_king_piece_eat_not_null()
    {
        //Arrange
        var gameObject = new GameObject();
        IKingPiece normalPiece = normalPiece = gameObject.AddComponent<KingPiece>();

        //Act
        var result = normalPiece.Eat();

        //Assert
        Assert.IsNotNull(result);
    }

}
