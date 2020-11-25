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
        PieceAbstract kingPiece = gameObject.AddComponent<KingPiece>();


        kingPiece.Filiation = PieceFiliation.player1;
        var result = kingPiece.Move();
        Assert.IsNotNull(result);


        kingPiece.Filiation = PieceFiliation.player2;
        var result2 = kingPiece.Move();
        Assert.IsNotNull(result2);


    }

    [Test]
    public void check_king_piece_eat_not_null()
    {
        //Arrange
        var gameObject = new GameObject();
        PieceAbstract kingPiece = kingPiece = gameObject.AddComponent<KingPiece>();

        //Act
        var result = kingPiece.Eat();

        //Assert
        Assert.IsNotNull(result);
    }

}
