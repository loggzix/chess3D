using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChessPlayer
{
	public TeamColor team { get; set; }
	public Board board { get; set; }
	public List<Piece> activePieces {get; private set; }	//cac quan co con song

	//ham khoi tao
	public ChessPlayer(TeamColor team, Board board)
	{
		activePieces = new List<Piece>();
		this.board = board;
		this.team = team;
	}

	//Them quan co vao ActivePieces
	public void AddPiece(Piece piece)
	{
		if (!activePieces.Contains(piece))
			activePieces.Add(piece);
	}

	//Go quan co khoi ActivePieces
	public void RemovePiece(Piece piece)
	{
		if (activePieces.Contains(piece))
			activePieces.Remove(piece);
	}

	//Lam moi tat ca cac quan co con song
	public void GenerateAllPossibleMoves()
	{
		foreach (var piece in activePieces)
		{
			if(board.HasPiece(piece))
			{
				piece.SelectAvaliableSquares(board.grid);
			}
		}
	}

	//Lay cac quan co tan cong quan co Kieu <T>
	public Piece[] GetPieceAtackingOppositePiceOfType<T>() where T : Piece
	{
		return activePieces.Where(p => p.IsAttackingPieceOfType<T>()).ToArray();
	}

	//Lay cac quan co kieu <T>
	public Piece[] GetPiecesOfType<T>() where T : Piece
	{
		return activePieces.Where(p => p is T).ToArray();
	}
	
	//Lay tat ca cac vi tri cua cac quan co con song
	internal List<Vector2Int> getAllLocationActive()
	{
		List<Vector2Int> data = new List<Vector2Int>();
		foreach(var piece in activePieces)
		{
			Vector2Int location = piece.occupiedSquare;
			data.Add(location);
		}
		return data;
	}
	
	//Go bo cac nuoc di lam cho quan co <T> bi tan cong
	public List<Vector2Int> RemoveMovesEnablingAttakOnPieceOfType<T>(ChessPlayer opponent, Piece selectedPiece) where T : Piece
	{
		List<Vector2Int> coordsToRemove = new List<Vector2Int>();
		coordsToRemove.Clear();
		List<Vector2Int> list = selectedPiece.avaliableMoves;
		foreach (var coords in selectedPiece.avaliableMoves)
		{
			Piece pieceOnCoords = board.GetPieceOnSquare(coords);
			board.UpdateBoardOnPieceMove(coords, selectedPiece.occupiedSquare, selectedPiece, null);
			opponent.GenerateAllPossibleMoves();	
			if (opponent.CheckIfIsAttacigPiece<T>())	
				coordsToRemove.Add(coords);
			board.UpdateBoardOnPieceMove(selectedPiece.occupiedSquare, coords, selectedPiece, pieceOnCoords);
		}
		foreach (var coords in coordsToRemove)
		{
			list.Remove(coords);
		}
		return list;
	}

	internal bool CheckIfIsAttacigPiece<T>() where T : Piece
	{
		foreach (var piece in activePieces)
		{
			if (board.HasPiece(piece) && piece.IsAttackingPieceOfType<T>())
				return true;
		}
		return false;
	}

	//an cac quan co tan cong T
	public bool CanHidePieceFromAttack<T>(ChessPlayer opponent) where T : Piece
	{
		foreach (var piece in activePieces)
		{
			foreach (var coords in piece.avaliableMoves)
			{
				Piece pieceOnCoords = board.GetPieceOnSquare(coords);
				board.UpdateBoardOnPieceMove(coords, piece.occupiedSquare, piece, null);
				opponent.GenerateAllPossibleMoves();
				if (!opponent.CheckIfIsAttacigPiece<T>())
				{
					board.UpdateBoardOnPieceMove(piece.occupiedSquare, coords, piece, pieceOnCoords);
					return true;
				}
				board.UpdateBoardOnPieceMove(piece.occupiedSquare, coords, piece, pieceOnCoords);
			}
		}
		return false;
	}

	//Xoa du lieu Chessplayer
	internal void OnGameRestarted()
	{
		activePieces.Clear();
	}

}