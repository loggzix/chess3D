using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Luu va doc du lieu ban co theo chuan FEN
public class FenDataBoard
{
    private const string V = "";
    public List<string> listData;
    string prefix = "/";

    public FenDataBoard()
    {
        listData = new List<string>();
    }

    public void saveData(Piece[,] grid, bool isBlackTurn)
    {
        string data = null;
        for(int i =0; i < 8; i++)
        {
            int a = 0;
            for(int j = 0; j < 8; j++)
            {
                if(grid[i, j])
                {
                    if(a != 0) data += a.ToString();
                    data += CheckPiece(grid[i,j]);
                    a = 0;
                }
                else a++;   
            }
            data += prefix;
        }
        data += isBlackTurn ? "-w" : "-b" ;
        listData.Add(data);
    }

    public void readData(ChessGameController controller)
    {
        
        if(listData.Count > 0)
        {
            string data = listData[listData.Count - 2]; // lay data ở cuối

            bool isTurnBlack = true;       
            int row = 0;
            int column = 0;
            foreach(char i in data)
            {
                int number = (int) char.GetNumericValue(i);                
             
                
                if(i == '/') {row++; column = 0;}
                else if(number > 0 && number < 9) column += number;
                else if(i == data.Length-1) isTurnBlack = (i == 'b') ? true: false; 
                else 
                {
                    int scores = 0;
                    TeamColor team;
                    string type = readPypePiece(i, ref scores, out team);
                    Vector2Int location = new Vector2Int(row, column);
                    Type role = Type.GetType(type);
                    //Tao ra quan co
                    controller.CreatePieceAndInitialize(location, team, role, scores); 
                    column ++;
                }
            }

            if(isTurnBlack) controller.activePlayer = controller.blackPlayer;
            else controller.activePlayer = controller.whitePlayer;
            
            listData.RemoveRange(listData.Count-2 , listData.Count-1);
        }
        
        return;
    }


    private string CheckPiece(Piece piece)
    {
        string type = null;
        switch(piece.scores)
        {
            case 9000: type = (piece.team == TeamColor.Black) ? "k" : "K"; break;
            case 900: type = (piece.team == TeamColor.Black) ? "q" : "Q"; break;
            case 500: type = (piece.team == TeamColor.Black) ? "r" : "R"; break;
            case 300: type = (piece.team == TeamColor.Black) ? "n" : "N"; break;
            case 301: type = (piece.team == TeamColor.Black) ? "b" : "B"; break;
            case 100: type = (piece.team == TeamColor.Black) ? "p" : "P"; break;
            default : break;
        }
        return type;
    }

    public string readPypePiece(char type, ref int scores, out TeamColor team)
    {
        string tp = null;
        if (type == 'k' || type == 'K') {
            tp = PieceType.King.ToString();
            scores = 9000;}
        else if (type == 'n' || type == 'N') {
            tp = PieceType.Knight.ToString(); 
            scores = 300;}
        else if (type == 'b' || type == 'B') {
            tp = PieceType.Bishop.ToString(); 
            scores = 301;}
        else if (type == 'r' || type == 'R') {
            tp = PieceType.Rook.ToString(); 
             scores = 500;}
        else if (type == 'q' || type == 'Q') {
            tp = PieceType.Queen.ToString(); 
            scores = 900;}
        else if (type == 'p' || type == 'P') {
            tp = PieceType.Pawn.ToString(); 
            scores = 500;
        }
        if(type == 'k' || type == 'n' || type == 'b' || type == 'r' || type == 'q' || type == 'p') team = TeamColor.Black;
        else team = TeamColor.White;

        return tp;
    }
    
}
