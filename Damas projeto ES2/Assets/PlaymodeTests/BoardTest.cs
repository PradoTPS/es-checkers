using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class BoardTest
    {

        [UnityTest]
        public IEnumerator board_matrix_initialized_correctly()
        {

            //Arrange
            GameManager.Instance.LoadScene("InGame");
            yield return new WaitForSeconds(1f);

            var board = Transform.FindObjectOfType<Board>();

            //Assert
            Assert.IsTrue(is_matrix_initialized_properly(board));


            yield return null;
        }

        [UnityTest]
        public IEnumerator board_tiles_spawned_correctly()
        {

            //Arrange
            GameManager.Instance.LoadScene("InGame");
            yield return new WaitForSeconds(1f);

            var board = Transform.FindObjectOfType<Board>();

            //Assert
            Assert.IsTrue(are_tiles_initialized_properly(board));


            yield return null;
        }
        
        [UnityTest]
        public IEnumerator board_spawned_pieces_correctly()
        {

            //Arrange
            GameManager.Instance.LoadScene("InGame");
            yield return new WaitForSeconds(1f);

            var board = Transform.FindObjectOfType<Board>();

            //Assert
            Assert.IsTrue(board_spawned_any_piece(board));


            yield return null;
        }


        [UnityTest]
        public IEnumerator board_select_one_tile()
        {
            //Arrange
            GameManager.Instance.LoadScene("InGame");
            yield return new WaitForSeconds(1f);

            var board = Transform.FindObjectOfType<Board>();

            var go = new GameObject();
            go.AddComponent<SpriteRenderer>();
            var tile = go.AddComponent<Tile>();

            yield return new WaitForSecondsRealtime(0.2f);


            //Act
            board.SelectTile(tile);

            //Assert
            Assert.IsTrue(tile.IsSelected);

        }

        [UnityTest]
        public IEnumerator board_unselect_tile()
        {
            //Arrange
            GameManager.Instance.LoadScene("InGame");
            yield return new WaitForSeconds(1f);

            var board = Transform.FindObjectOfType<Board>();

            var go = new GameObject();
            go.AddComponent<SpriteRenderer>();
            var tile = go.AddComponent<Tile>();

            yield return new WaitForSecondsRealtime(0.2f);


            //Act
            board.SelectTile(tile);
            board.SelectTile(tile);

            //Assert
            Assert.IsFalse(tile.IsSelected);

        }



        private bool board_spawned_any_piece(Board board)
        {
            for (int i = 0; i < board.tilesMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < board.tilesMatrix.GetLength(1); j++)
                {
                    if (board.tilesMatrix[i, j] != null && board.tilesMatrix[i,j].MyPiece != null)
                        return true;
                }
            }
            return false;
        }

        private bool is_matrix_initialized_properly(Board board)
        {
            return board.tilesMatrix != null;
        }

        private bool are_tiles_initialized_properly(Board board)
        {
            for (int i = 0; i < board.tilesMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < board.tilesMatrix.GetLength(1); j++)
                {
                    if (board.tilesMatrix[i, j] == null)
                        return false;
                }
            }
            return true;
        }
    }
}
