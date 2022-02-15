using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TeamColor
{
    Black, White, 
}

public enum PieceType
{
    Pawn, Bishop, Knight, Rook, Queen, King
}

//Tao ra menu nhap o ngoai
[CreateAssetMenu(menuName = "Scriptable Objects/Board/Layout")]
public class BoardLayout : ScriptableObject
{
    [Serializable]  //Luu duu lieu duoi dang dong byte
    private class BoardSquareSetup
    {
        public Vector2Int position; //Toa do tren ban co
        public PieceType pieceType; //Kieu quan co
        public TeamColor teamColor; //Team
        public int scores;  //Suc manh cua quan co
    }

    [SerializeField] private BoardSquareSetup[] boardSquares;

    //So luong quan co
    public int GetPiecesCount()
    {
        return boardSquares.Length;
    }

    //Cac ham lay du lieu tu menu da tao o ben tren theo chi so index
    public Vector2Int GetSquareCoordsAtIndex(int index)
    {
        return new Vector2Int(boardSquares[index].position.x - 1, boardSquares[index].position.y - 1);
    }
    public string GetSquarePieceNameAtIndex(int index)
    {
        return boardSquares[index].pieceType.ToString();
    }
    public TeamColor GetSquareTeamColorAtIndex(int index)
    {
        return boardSquares[index].teamColor;
    }

    public int GetScoresAtIndex(int index)
    {
        return boardSquares[index].scores;
    }
}
