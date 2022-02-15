using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MaterialSetter))]
[RequireComponent(typeof(IObjectTweener))]
public abstract class Piece : MonoBehaviour
{
	[SerializeField] private MaterialSetter materialSetter;
	public Board board { protected get; set; }
	public Vector2Int occupiedSquare { get; set; }
	public TeamColor team { get; set; }
	public int hasMoved { get; set; }
	public List<Vector2Int> avaliableMoves;
	public int[,] attackDistribution;
	private IObjectTweener tweener;
	public int scores{ set; get;}
	public abstract List<Vector2Int> SelectAvaliableSquares(Piece[,] grid);

	//Ham khoi tao
	private void Awake()
	{
		avaliableMoves = new List<Vector2Int>();
		tweener = GetComponent<IObjectTweener>();
		materialSetter = GetComponent<MaterialSetter>();
		hasMoved = 0;
	}

	//SetMaterial cua gameObject
	public void SetMaterial(Material selectedMaterial)
	{
		materialSetter.SetSingleMaterial(selectedMaterial);
	}

	//Kiem tra xem piece cung team hay khong
	public bool IsFromSameTeam(Piece piece)
	{
		return team == piece.team;
	}

	//Tra ve cac nuoc di co the di chuyen
	public bool CanMoveTo(Vector2Int coords)
	{
		return avaliableMoves.Contains(coords);
	}

	//Di chuyen quan co
	public virtual void MovePiece(Vector2Int coords)
	{
		Vector3 targetPosition = board.CalculatePositionFromCoords(coords);
		occupiedSquare = coords;
		hasMoved++;
		tweener.MoveTo(transform, targetPosition);
	}

	//Them cac nuoc di hop le
	protected void TryToAddMove(Vector2Int coords)
	{
		avaliableMoves.Add(coords);
	}

	//Khoi tao du lieu quan co
	public void SetData(Vector2Int coords, TeamColor team, Board board, int scores)
	{
		this.team = team;
		occupiedSquare = coords;
		this.board = board;
		this.scores = scores;
		transform.position = board.CalculatePositionFromCoords(coords);
	}

	//Kiem tra xem piece co tan cong quan co loai T khong
	public bool IsAttackingPieceOfType<T>() where T : Piece
	{
		foreach (var square in avaliableMoves)
		{
			if (board.GetPieceOnSquare(square) is T)
				return true;
		}
		return false;
	}


}