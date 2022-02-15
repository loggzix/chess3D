using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController
{
//Bang vi tri suc manh cua moi quan
private static int[,] bishopWhite = {
        {-20, -10, -10, -10, -10, -10, -10, -20},
        {-10,   5,  10,   0,   5,   0,   0, -10},
        {-10,   0,  10,  10,   5,   5,   0, -10},
        {-10,   0,  10,  10,  10,  10,   0, -10},
        {-10,   0,  10,  10,  10,  10,   0, -10},
        {-10,   0,  10,  10,   5,   5,   0, -10},
        {-10,   5,  10,   0,   5,   0,   0, -10},
        {-20, -10, -10, -10, -10, -10, -10, -20}
    };
private static int[,] bishopBlack = {
        {-20, -10, -10, -10, -10, -10, -10, -20},
        {-10,   0,   0,   5,   0,  10,   5, -10},
        {-10,   0,   5,   5,  10,  10,   0, -10},
        {-10,   0,  10,  10,  10,  10,   0, -10},
        {-10,   0,  10,  10,  10,  10,   0, -10},
        {-10,   0,   5,   5,  10,  10,   0, -10},
        {-10,   0,   0,   0,   0,  10,   5, -10},
        {-20, -10, -10, -10, -10, -10, -10, -20}
    };

    private static int[,] kingWhite = {
        { 20,  20, -10, -20, -30, -30, -30, -30},
        { 30,  20, -20, -30, -40, -40, -40, -40},
        { 10,   0, -20, -30, -40, -40, -40, -40},
        {  0,   0, -20, -40, -50, -50, -50, -50},
        {  0,   0, -20, -40, -50, -50, -50, -50},
        { 10,   0, -20, -30, -40, -40, -40, -40},
        { 30,  20, -20, -30, -40, -40, -40, -40},
        { 20,  20, -10, -20, -30, -30, -30, -30}
    };
private static int[,] kingBlack = {
        {-30, -30, -30, -30, -20, -10,  20,  20},
        {-40, -40, -40, -40, -30, -20,  20,  30},
        {-40, -40, -40, -40, -30, -20,   0,  10},
        {-50, -50, -50, -50, -40, -20,   0,   0},
        {-50, -50, -50, -50, -40, -20,   0,   0},
        {-40, -40, -40, -40, -30, -20,   0,  10},
        {-40, -40, -40, -40, -30, -20,  20,  30},
        {-30, -30, -30, -30, -20, -10,  20,  20}
    };
private static int[,] knightWhite = {
        {-50, -40, -30, -30, -30, -30, -40, -50},
        {-40, -20,   5,   0,   5,   0, -20, -40},
        {-30,   0,  10,  15,  15,  10,   0, -30},
        {-30,   5,  15,  20,  20,  15,   0, -30},
        {-30,   5,  15,  20,  20,  15,   0, -30},
        {-30,   0,  10,  15,  15,  10,   0, -30},
        {-40, -20,   5,   0,   5,   0, -20, -40},
        {-50, -40, -30, -30, -30, -30, -40, -50}
    };
 private static int[,] knightBlack = {
        {-50, -40, -30, -30, -30, -30, -40, -50},
        {-40, -20,   0,   5,   0,   5, -20, -40},
        {-30,   0,  10,  15,  15,  10,   0, -30},
        {-30,   0,  15,  20,  20,  15,   5, -30},
        {-30,   0,  15,  20,  20,  15,   5, -30},
        {-30,   0,  10,  15,  15,  10,   0, -30},
        {-40, -20,   0,   5,   0,   5, -20, -40},
        {-50, -40, -30, -30, -30, -30, -40, -50}
    };
 private static int[,] pawnWhite = {
        {  0,   5,   5,   0,   5,  10,  50,   0},
        {  0,  10,  -5,   0,   5,  10,  50,   0},
        {  0,  10, -10,   0,  10,  20,  50,   0},
        {  0, -20,   0,  20,  25,  30,  50,   0},
        {  0, -20,   0,  20,  25,  30,  50,   0},
        {  0,  10, -10,   0,  10,  20,  50,   0},
        {  0,  10,  -5,   0,   5,  10,  50,   0},
        {  0,   5,   5,   0,   5,  10,  50,   0}
    };
 private static int[,] pawnBlack = {
        {  0,  50,  10,   5,   0,   5,   5,   0},
        {  0,  50,  10,   5,   0,  -5,  10,   0},
        {  0,  50,  20,  10,   0, -10,  10,   0},
        {  0,  50,  30,  25,  20,   0, -20,   0},
        {  0,  50,  30,  25,  20,   0, -20,   0},
        {  0,  50,  20,  10,   0, -10,  10,   0},
        {  0,  50,  10,   5,   0,  -5,  10,   0},
        {  0,  50,  10,   5,   0,  -5,   5,   0}
    };
private static int[,] queenWhite = {
        {-20, -10, -10,  -5,  -5, -10, -10, -20},
        {-10,   0,   0,   0,   0,   0,   0, -10},
        {-10,   0,   5,   5,   5,   5,   0, -10},
        { -5,   0,   5,   5,   5,   5,   0,  -5},
        { -5,   0,   5,   5,   5,   5,   0,  -5},
        {-10,   5,   5,   5,   5,   5,   0, -10},
        {-10,   0,   5,   0,   0,   0,   0, -10},
        {-20, -10, -10,   0,  -5, -10, -10, -20}
    };
private static int[,] queenBlack = {
        {-20, -10, -10,  -5,  -5, -10, -10, -20},
        {-10,   0,   0,   0,   0,   0,   0, -10},
        {-10,   0,   5,   5,   5,   5,   0, -10},
        { -5,   0,   5,   5,   5,   5,   0,  -5},
        { -5,   0,   5,   5,   5,   5,   0,  -5},
        {-10,   0,   5,   5,   5,   5,   5, -10},
        {-10,   0,   0,   0,   0,   5,   0, -10},
        {-20, -10, -10,  -5,   0, -10, -10, -20}
    };
private static int[,] rookWhite = {
        {  0,  -5,  -5,  -5,  -5,  -5,   5,   0},
        {  0,   0,   0,   0,   0,   0,  10,   0},
        {  0,   0,   0,   0,   0,   0,  10,   0},
        {  5,   0,   0,   0,   0,   0,  10,   0},
        {  5,   0,   0,   0,   0,   0,  10,   0},
        {  0,   0,   0,   0,   0,   0,  10,   0},
        {  0,   0,   0,   0,   0,   0,  10,   0},
        {  0,  -5,  -5,  -5,  -5,  -5,   5,   0}
    };
private static int[,] rookBlack = new int[8,8]{
        {  0,   5,  -5,  -5,  -5,  -5,  -5,   0},
        {  0,  10,   0,   0,   0,   0,   0,   0},
        {  0,  10,   0,   0,   0,   0,   0,   0},
        {  0,  10,   0,   0,   0,   0,   0,   5},
        {  0,  10,   0,   0,   0,   0,   0,   5},
        {  0,  10,   0,   0,   0,   0,   0,   0},
        {  0,  10,   0,   0,   0,   0,   0,   0},
        {  0,   5,  -5,  -5,  -5,  -5,  -5,   0}
    };

    public AIController()
    {
        dataBoard = new DataBoard(this);
    }
    
    internal static void InitBoardAtack(Piece piece, Type type)
    {
        string role = type.ToString();
        if(piece.team == TeamColor.White)
        {
            if(role == PieceType.Bishop.ToString()) piece.attackDistribution = bishopWhite;
            else if(role == PieceType.King.ToString()) piece.attackDistribution = kingWhite;
            else if(role == PieceType.Queen.ToString()) piece.attackDistribution = queenWhite;
            else if(role == PieceType.Knight.ToString()) piece.attackDistribution = knightWhite;
            else if(role == PieceType.Pawn.ToString()) piece.attackDistribution = pawnWhite;
            else if(role == PieceType.Rook.ToString()) piece.attackDistribution = rookWhite;
        }
        else if(piece.team == TeamColor.Black)
        {
            if(role == PieceType.Bishop.ToString()) piece.attackDistribution = bishopBlack;
            else if(role == PieceType.King.ToString()) piece.attackDistribution = kingBlack;
            else if(role == PieceType.Queen.ToString()) piece.attackDistribution = queenBlack;
            else if(role == PieceType.Knight.ToString()) piece.attackDistribution = knightBlack;
            else if(role == PieceType.Pawn.ToString()) piece.attackDistribution = pawnBlack;
            else if(role == PieceType.Rook.ToString()) piece.attackDistribution = rookBlack;
        }
    }

    private int envaluated; //so nhanh cua thuat toan
    internal Piece selectedPiece;   //Vi tri tot nhat cua quan co duoc chon
    internal Vector2Int bestMove;   //Nuoc di tot nhat
    private DataBoard dataBoard;    //Du lieu ban co

    //Thuat toan cat tia alpha beta
    internal void Root(Piece[,] grid, int depth, bool maximizingPlayer)
    {
        int bestScore = int.MinValue;
        List<Data> listData = dataBoard.addDataWithContext(grid, TeamColor.Black);        
        
        foreach(var data in listData)
            {
            Piece piece = grid[data.Selected.x, data.Selected.y];
            foreach(var move in data.listAvalbleMove)
            {
                Vector2Int startLocation = piece.occupiedSquare;
                Piece startPiece = piece;   //Cai nay hasmove = 0
                Vector2Int endLocation = move;
                Piece endPiece = grid[move.x, move.y];
                startPiece.hasMoved++;
                UpdateGrid(grid, startPiece, endLocation, null, startLocation);

                int value  = AlphaBeta(grid, depth - 1, int.MinValue, int.MaxValue, !maximizingPlayer);

                startPiece.hasMoved--;
                UpdateGrid(grid, startPiece, startLocation, endPiece, endLocation);
                if(value >= bestScore)
                {
                    bestScore = value;
                    selectedPiece = piece;
                    bestMove = move;
                }
            }            
        }
        // Debug.Log($"{envaluated}");
    }

    private int AlphaBeta(Piece[,] grid ,int depth, int alpha, int beta, bool maximizingPlayer)
    {   
        // envaluated++;
        if(depth == 0) return -CalculatorNodeCurrent(grid, maximizingPlayer);
        
        foreach(var piece in grid) if(piece) piece.SelectAvaliableSquares(grid);
        
        TeamColor team = maximizingPlayer ? TeamColor.Black : TeamColor.White;
        List<Data> listData = dataBoard.addDataWithContext(grid, team);
        
        if(maximizingPlayer)
        {
            int maxEval = int.MinValue; 
            foreach(var data in listData)
            {
                Piece piece = grid[data.Selected.x, data.Selected.y];
                foreach(var move in data.listAvalbleMove)
                {                   
                    Vector2Int startLocation = piece.occupiedSquare;
                    Piece startPiece = piece;   
                    Vector2Int endLocation = move;
                    Piece endPiece = grid[move.x, move.y];
                    startPiece.hasMoved++;
                    UpdateGrid(grid, startPiece, endLocation, null, startLocation);
                    
                    maxEval = Math.Max(maxEval, AlphaBeta(grid, depth - 1, alpha, beta, !maximizingPlayer));
                         
                    startPiece.hasMoved--;
                    UpdateGrid(grid, startPiece, startLocation, endPiece, endLocation);
                    alpha = Math.Max(alpha, maxEval);
                    if(alpha >= beta) break;
                } 
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
             foreach(var data in listData)
            {
                Piece piece = grid[data.Selected.x, data.Selected.y];                
                foreach(var move in data.listAvalbleMove)
                {
                    
                    Vector2Int startLocation = piece.occupiedSquare;
                    Piece startPiece = piece;   
                    Vector2Int endLocation = move;
                    Piece endPiece = grid[move.x, move.y];
                    startPiece.hasMoved++;
                    UpdateGrid(grid, startPiece, endLocation, null, startLocation);
                    
                    minEval = Math.Min(minEval, AlphaBeta(grid, depth - 1, alpha, beta, !maximizingPlayer));
                             
                    startPiece.hasMoved--;
                    UpdateGrid(grid, startPiece, startLocation, endPiece, endLocation);
                    beta = Math.Min(beta, minEval); 
                    if(alpha >= beta) break;
                } 
            }
            return minEval;
        }
    }   

    //doi vi tri 2 o tren ban co
    internal void UpdateGrid(Piece[,] grid ,Piece piece1, Vector2Int location1, Piece piece2, Vector2Int location2)
    {
        if(piece1) piece1.occupiedSquare = location1;
        if(piece2) piece2.occupiedSquare = location2;
        grid[location1.x, location1.y] = piece1;
        grid[location2.x, location2.y] = piece2;
        
    }

    //Kiem tra xem the co hien tai vua co bi tan cong hay khong
    internal bool checkAvavilbleMoveCurrent(Piece[,] grid, TeamColor team)
    {
        Vector2Int KingLocation = new Vector2Int();
        foreach(var piece in grid)
        {
            if(piece) piece.SelectAvaliableSquares(grid);
            if(piece && piece.scores == 9000 && piece.team == team) KingLocation = piece.occupiedSquare;
        }

        Vector2Int t1, t2;
        if(team == TeamColor.Black)
        {
            t1 = new Vector2Int(1,1);
            t2 = new Vector2Int(-1,1);
        }
        else 
        {
            t1 = new Vector2Int(-1,-1);
            t2 = new Vector2Int(1,-1);
        }
        foreach(var piece in grid)
        {
            
            if(piece && piece.team != team)
            { 
                if(piece && piece.scores == 100) 
                {
                    if(piece.occupiedSquare + t1 == KingLocation || piece.occupiedSquare + t2 == KingLocation) return false;
                }
                else if(piece && piece.scores != 100)
                {
                    foreach(var move in piece.avaliableMoves)
                    {
                        if(move == KingLocation) return false;                
                    }
                }
                
            }
        }

        return true;
    }

    //Ham tinh khoang cach alpha beta
    private int CalculatorNodeCurrent(Piece[,] node, bool blackToMove)
    {
        int sum = 0;
        foreach(var piece in node)
        {
            if(piece && piece.team == TeamColor.White) sum += piece.scores + piece.attackDistribution[piece.occupiedSquare.x, piece.occupiedSquare.y];
            else if(piece && piece.team == TeamColor.Black) sum =sum - piece.scores - piece.attackDistribution[piece.occupiedSquare.x, piece.occupiedSquare.y];
        }
        // int perspective = blackToMove == false ? 1 : -1;
        return sum ;
    }
}
