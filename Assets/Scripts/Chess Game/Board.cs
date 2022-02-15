using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SquareSelectorCreator))]
public class Board : MonoBehaviour
{
    public const int BOARD_SIZE = 8;

    [SerializeField] private Transform bottomLeftSquareTransform;   //Thuoc tinh transform objectGame
    [SerializeField] private float squareSize;  //kich co mot canh cua o vuong nho
    // private Piece OldPiece;
    public Piece[,] grid{set;get;}  //Mang quan co tren ban co
    public Piece selectedPiece; //Quan co duoc chon de di chuyen
    private ChessGameController chessController;    
    private SquareSelectorCreator squareSelector;   //O vuong hien thi cac nuoc kha thi
    
    //ham khoi tao
    private void Awake()
    {
        squareSelector = GetComponent<SquareSelectorCreator>();
        CreateGrid();
    }

    //khoi tao ChessGameCotroller
    public void SetDependencies(ChessGameController chessController)
    {
        this.chessController = chessController;
    }

    //Ham khoi tao mang quan co
    private void CreateGrid()
    {
        grid = new Piece[BOARD_SIZE, BOARD_SIZE];
    }

    //Chuyen doi toa do 2D sang 3D
    public Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        return bottomLeftSquareTransform.position + new Vector3(coords.x * squareSize, 0f, coords.y * squareSize);
    }

    //Chuyen doi vi tri con tro 3D duoc chon sang toa do 2D tren ban co
    private Vector2Int CalculateCoordsFromPosition(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).x / squareSize) + BOARD_SIZE / 2;
        int y = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).z / squareSize) + BOARD_SIZE / 2;
        return new Vector2Int(x, y);
    }

    //Ham xu ly quan co duoc chon
    public void OnSquareSelected(Vector3 inputPosition) 
    {
        Vector2Int coords = CalculateCoordsFromPosition(inputPosition); 
        Piece piece = GetPieceOnSquare(coords);
        if (selectedPiece)
        {
            if (piece != null && selectedPiece == piece){
                //huy chon (Chon 2 lan mot quan co)
                DeselectPiece();
            }
            else if (piece != null && selectedPiece != piece && chessController.IsTeamTurnActive(piece.team)){
                //cung team(Chon 2 quan co cung team khac nhau)
                SelectPiece(piece);
            }
            else if (selectedPiece.CanMoveTo(coords)){
                 //Di chuyen (Di chuyen quan co )
                OnSelectedPieceMoved(coords, selectedPiece);
                
            }
        }
        else
        {
            if (piece != null && chessController.IsTeamTurnActive(piece.team))
                {
                    SelectPiece(piece);
                }
        }
    }

    //Ham chon quan co
    public void SelectPiece(Piece piece)
    {
        selectedPiece = piece;
        List<Vector2Int> selection = chessController.RemoveMovesEnablingAttakOnPieceOfType<King>(piece);//x
        ShowSelectionSquares(selection);
    }

    //Get cac ve tri co the theo data
    internal List<Vector2Int> getAnavailibleMoves(Vector2Int data)
    {        
        Piece piece = GetPieceOnSquare(data);
        List<Vector2Int> list = chessController.RemoveMovesEnablingAttakOnPieceOfType<King>(piece);
        
        return list;
    }

    //Show cac nuoc di kha thi cua mot quan co
    private void ShowSelectionSquares(List<Vector2Int> selection)
    {
        Dictionary<Vector3, bool> squaresData = new Dictionary<Vector3, bool>();
        for (int i = 0; i < selection.Count; i++)
        {
            Vector3 position = CalculatePositionFromCoords(selection[i]);
            bool isSquareFree = GetPieceOnSquare(selection[i]) == null;
            squaresData.Add(position, isSquareFree);
        }
        squareSelector.ShowSelection(squaresData);
    }
    
    //Huy chon quan co da chon
    private void DeselectPiece()
    {
        selectedPiece = null;
        squareSelector.ClearSelection();
    }

    //Di chuyen quan co
    public void OnSelectedPieceMoved(Vector2Int coords, Piece piece)
    {
        TryToTakeOppositePiece(coords); //Gỡ ở vị trí new
        UpdateBoardOnPieceMove(coords, piece.occupiedSquare, piece, null);//Thay dữ liệu ở grid
        selectedPiece.MovePiece(coords);//di chuyển trên hình
        DeselectPiece(); //clear data piece selected
        EndTurn();
    }

    //Ket thuc 1 turn
    private void EndTurn()
    {
        chessController.EndTurn();
    }
    //Thay doi du lieu 2 o tren mang grid
    public void UpdateBoardOnPieceMove(Vector2Int newCoords, Vector2Int oldCoords, Piece newPiece, Piece oldPiece)
    {
        grid[oldCoords.x, oldCoords.y] = oldPiece;
        grid[newCoords.x, newCoords.y] = newPiece;
    }

    //Lay quan co theo toa do tren ban co
    public Piece GetPieceOnSquare(Vector2Int coords)
    {
        if (CheckIfCoordinatesAreOnBoard(coords))
            return grid[coords.x, coords.y];
        return null;
    }

    //Kiem tra xem toa do (x,y) co tren ban co hay khing
    public bool CheckIfCoordinatesAreOnBoard(Vector2Int coords)
    {
        if (coords.x < 0 || coords.y < 0 || coords.x >= BOARD_SIZE || coords.y >= BOARD_SIZE)
            return false;
        return true;
    }

    //Danh dau cac vi tri co co
    public bool HasPiece(Piece piece)
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if (grid[i, j] == piece)
                    return true;
            }
        }
        return false;
    }

    //Dat co tren ban co
    public void SetPieceOnBoard(Vector2Int coords, Piece piece)
    {
        if  (CheckIfCoordinatesAreOnBoard(coords))
            grid[coords.x, coords.y] = piece;
    }

    //Ham kiem tra vi tri di chuyen
    public void TryToTakeOppositePiece(Vector2Int coords)
    {
        Piece piece = GetPieceOnSquare(coords);
        if (piece && !selectedPiece.IsFromSameTeam(piece))
        {
            TakePiece(piece);
        }
    }

    //Xoa bo quan co piece
    private void TakePiece(Piece piece)
    {
        if (piece)
        {
            grid[piece.occupiedSquare.x, piece.occupiedSquare.y] = null;
            chessController.OnPieceRemoved(piece);
            Destroy(piece.gameObject);
        }
    }

    //Nhap thanh
    public void PromotePiece(Piece piece)
    {
        TakePiece(piece);
        chessController.CreatePieceAndInitialize(piece.occupiedSquare, piece.team, typeof(Queen), piece.scores);
    }

    //Khoi dong lai game
    internal void OnGameRestarted()
    {
        selectedPiece = null;
        CreateGrid();
    }

}
