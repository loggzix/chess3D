using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    private Vector2Int[] directions = new Vector2Int[] { Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down };
    public override List<Vector2Int> SelectAvaliableSquares(Piece[,] grid)
    {
        avaliableMoves.Clear();

        float range = Board.BOARD_SIZE;
        foreach (var direction in directions)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int nextCoords = occupiedSquare + direction * i;
                Piece piece;
                  //Lay piece on square
                if (!board.CheckIfCoordinatesAreOnBoard(nextCoords)) //ổn
                    break;
                
                else piece = grid[nextCoords.x, nextCoords.y]; 
                if (piece == null)  //nếu ô đó còn trống    
                    TryToAddMove(nextCoords);   //Thêm ô đó vào ô khả thi
                else if (!piece.IsFromSameTeam(this))   //nếu gặp team địch
                {
                    TryToAddMove(nextCoords);
                    break;
                }
                else if (piece.IsFromSameTeam(this))    //Nếu gặp team mình
                    break;
            }
        }
        return avaliableMoves;
    }

}
