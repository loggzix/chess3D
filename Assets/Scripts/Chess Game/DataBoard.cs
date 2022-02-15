using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Data
    {
        internal Vector2Int Selected{get;}  //Quan co duoc chon
        internal List<Vector2Int> listAvalbleMove{get;} //Cac nuoc di co the cua quan co duoc chon
        public Data(Vector2Int Selected, List<Vector2Int> listAvalbleMove)
        {
            this.Selected = Selected;
            this.listAvalbleMove = listAvalbleMove;
        }
    }

[System.Serializable]
public class DataBoard
{
    AIController AI;
    public DataBoard(AIController aIController)
    {
        AI = aIController;
    }

    //Luu tat ca du lieu quan co cua ActiveTeam
     internal List<Data> addDataWithContext(Piece[,] grid, TeamColor team)
    {
        List<Data> listData = new List<Data>();
        foreach(var piece in grid) 
        {
            if(piece && piece.team == team)
            {
                List<Vector2Int> listAvalablemoves = new List<Vector2Int>();
                foreach(var move in piece.avaliableMoves)
                {          
                    listAvalablemoves.Add(move);    
                }
                Data data = new Data(piece.occupiedSquare, listAvalablemoves);
                listData.Add(data);
            }
        }

        foreach(var data in listData)
        {
            Piece piece = grid[data.Selected.x, data.Selected.y];
            List<Vector2Int> listUnvailble = new List<Vector2Int>();
            foreach(var move in data.listAvalbleMove)
            {
                
                Vector2Int startLocation = piece.occupiedSquare;
                Piece startPiece = piece;   
                Vector2Int endLocation = move;
                Piece endPiece = grid[move.x, move.y];
                startPiece.hasMoved++;
                AI.UpdateGrid(grid, startPiece, endLocation, null, startLocation);
                if(!AI.checkAvavilbleMoveCurrent(grid, piece.team))
                {
                    listUnvailble.Add(move);    //Danh sach cac nuoc di ko hop ly
                }          
                startPiece.hasMoved--;
                AI.UpdateGrid(grid, startPiece, startLocation, endPiece, endLocation);
            }
            foreach(var move in listUnvailble) data.listAvalbleMove.Remove(move);
        }
        return listData;
    }

}
