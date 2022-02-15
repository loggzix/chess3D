using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(PiecesCreator))]
public class ChessGameController : MonoBehaviour
{
    private enum GameState
    {
        Init, Play, Finished
    }

    [SerializeField] private BoardLayout startingBoardLayout;      //Layout quan co o vi tri ban dau
    [SerializeField] private Board board;   
    [SerializeField] private ChessUIManager UIManager;
    [SerializeField] private TextMeshProUGUI timePlayer1;
    [SerializeField] private TextMeshProUGUI timePLayer2;

    private PiecesCreator pieceCreator; //Doi tuong quan ly, gan cac thuoc tinh, du lieu 3D vao gameObject
    public ChessPlayer whitePlayer;
    public ChessPlayer blackPlayer;
    public ChessPlayer activePlayer;
    private GameState state;
    private FenDataBoard fenControler;  //Doi tuong quan ly du lieu dang FEN
    private ArrayList arrData;  //luu du lieu dang FEN

    //Khoi tao
    private void Awake()
    {
        SetDependencies();
        CreatePlayers();
        fenControler = new FenDataBoard();
        arrData = new ArrayList();
    }

    //Khoi tao doi tuong pieceCreate
    private void SetDependencies()
    {
        pieceCreator = GetComponent<PiecesCreator>();
    }

    //Khoi tao nguoi choi
    private void CreatePlayers()
    {
        whitePlayer = new ChessPlayer(TeamColor.White, board);
        blackPlayer = new ChessPlayer(TeamColor.Black, board);
    }

    //Bat dau game
    private void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        SetGameState(GameState.Init);
        UIManager.HideUI();
        board.SetDependencies(this);
        CreatePiecesFromLayout(startingBoardLayout);
        activePlayer = whitePlayer;
        GenerateAllPossiblePlayerMoves(activePlayer);
        SetGameState(GameState.Play);
    }

    private void StartNewGameFromData()
    {

    }
    private void SetGameState(GameState state)
    {
        this.state = state;
    }

    internal bool IsGameInProgress()
    {
        return state == GameState.Play;
    }

    //Tao cac quan co tu du lieu menu layout
    private void CreatePiecesFromLayout(BoardLayout layout)
    {
        for (int i = 0; i < layout.GetPiecesCount(); i++)
        {
            Vector2Int squareCoords = layout.GetSquareCoordsAtIndex(i);
            TeamColor team = layout.GetSquareTeamColorAtIndex(i);
            string typeName = layout.GetSquarePieceNameAtIndex(i);
            int scores = layout.GetScoresAtIndex(i);
            Type type = Type.GetType(typeName);
            CreatePieceAndInitialize(squareCoords, team, type, scores);
        }
    }

    //Tao mot quan co va set cac du lieu 3D
    public Piece CreatePieceAndInitialize(Vector2Int squareCoords, TeamColor team, Type type, int scores)
    {
        Piece newPiece = pieceCreator.CreatePiece(type).GetComponent<Piece>();
        newPiece.SetData(squareCoords, team, board, scores);

        Material teamMaterial = pieceCreator.GetTeamMaterial(team);
        newPiece.SetMaterial(teamMaterial);

        board.SetPieceOnBoard(squareCoords, newPiece);

        ChessPlayer currentPlayer = team == TeamColor.White ? whitePlayer : blackPlayer;
        currentPlayer.AddPiece(newPiece);
        
        AIController.InitBoardAtack(newPiece, type);
        return newPiece;
    }

    //Lam moi tat ca nuoc di cua player
    public void GenerateAllPossiblePlayerMoves(ChessPlayer player)
    {
        player.GenerateAllPossibleMoves();
    }

    //Tra ve team dang danh
    public bool IsTeamTurnActive(TeamColor team)
    {
        return activePlayer.team == team;
    }

    //ket thuc mot turn
    public void EndTurn()
    {
        GenerateAllPossiblePlayerMoves(activePlayer);
        GenerateAllPossiblePlayerMoves(GetOpponentToPlayer(activePlayer));
        if (CheckIfGameIsFinished())
        {
            EndGame();
        }
        else
        {
            ChangeActiveTeam();
            if(activePlayer.team == TeamColor.Black) StartCoroutine(AutoMove());
        }
    }

    private void Update()
    {
        if(activePlayer == whitePlayer)
        {
            UpdateTime(ref minutesWhite,ref secondsWhite);
            timePlayer1.SetText($"{minutesWhite}:{Math.Round(secondsWhite, 2)}");
        }
        else
        {
            UpdateTime(ref minutesBlack,ref secondsBlack);
            timePLayer2.SetText($"{minutesBlack}:{Math.Round(secondsBlack, 2)}");
        }
        
    }

    private float minutesWhite = 30, minutesBlack = 30;
    private float secondsWhite = 00, secondsBlack = 00;
    private void UpdateTime(ref float minutes,ref  float seconds)
    {
            if(seconds <= 0)
            {
                if(minutes > 0)
                {
                    minutes--; 
                    seconds= 59.59f;
                }
                else
                {
                    ChangeActiveTeam();
                    EndGame();
                }
            }
            else
            {
                seconds -= Time.deltaTime;
            }
    }

    //Ham di chuyen cua AI
    private IEnumerator AutoMove()
    {  
        yield return new WaitForSeconds(0.6f);
        AIController AI = new AIController();
        Piece[,] grid = board.grid;

        AI.Root(grid, Menu.LEVELS, true);     //(3) Do sau cua thuat toan 

        Piece pieceSelected = AI.selectedPiece;
        Vector2Int bestMove = AI.bestMove;

        //Neu AI khong con nuoc di nao => nguoi choi thang
        if(pieceSelected != null && bestMove != null)
        {
            board.SelectPiece(pieceSelected);
            board.OnSelectedPieceMoved(bestMove, pieceSelected);
        }
        else
        {
            SetGameState(GameState.Finished);
            UIManager.OnGameFinished(whitePlayer.team.ToString());
        }
        
    }
    
    //Kiem tra xem trang thai game da ket thuc chua
    private bool CheckIfGameIsFinished()
    {
        Piece[] kingAttackingPieces = activePlayer.GetPieceAtackingOppositePiceOfType<King>();
        if (kingAttackingPieces.Length > 0)
        {
            ChessPlayer oppositePlayer = GetOpponentToPlayer(activePlayer);
            Piece attackedKing = oppositePlayer.GetPiecesOfType<King>().FirstOrDefault();
            oppositePlayer.RemoveMovesEnablingAttakOnPieceOfType<King>(activePlayer, attackedKing);

            int avaliableKingMoves = attackedKing.avaliableMoves.Count;
            if (avaliableKingMoves == 0)
            {
                bool canCoverKing = oppositePlayer.CanHidePieceFromAttack<King>(activePlayer);
                if (!canCoverKing)
                    return true;
            }
        }
        return false;
    }

    //Endgame sau khi da win
    private void EndGame()
    {
        SetGameState(GameState.Finished);
        UIManager.OnGameFinished(activePlayer.team.ToString());
    }

    //Khoi dong lai game 
    public void RestartGame()
    {
        DestroyPieces();
        board.OnGameRestarted();
        whitePlayer.OnGameRestarted();
        blackPlayer.OnGameRestarted();
        StartNewGame();
    }

    //Xoa bo doi tuong gameObject
    private void DestroyPieces()
    {
        whitePlayer.activePieces.ForEach(p => Destroy(p.gameObject));
        blackPlayer.activePieces.ForEach(p => Destroy(p.gameObject));
    }

    //Doi luot
    private void ChangeActiveTeam()
    {
        activePlayer = activePlayer == whitePlayer ? blackPlayer : whitePlayer;
    }

    public ChessPlayer GetOpponentToPlayer(ChessPlayer player)
    {
        return player == whitePlayer ? blackPlayer : whitePlayer;
    }

    //go bo ActivePiece cua chessPlayer
    internal void OnPieceRemoved(Piece piece)
    {
        ChessPlayer pieceOwner = (piece.team == TeamColor.White) ? whitePlayer : blackPlayer;
        pieceOwner.RemovePiece(piece);
    }

    //Go bo cac vi tri tan cong quan co co vai tro <T>
    internal List<Vector2Int> RemoveMovesEnablingAttakOnPieceOfType<T>(Piece piece) where T : Piece
    {
        activePlayer = piece.team == TeamColor.Black ? blackPlayer : whitePlayer;
        return activePlayer.RemoveMovesEnablingAttakOnPieceOfType<T>(GetOpponentToPlayer(activePlayer), piece);
    }
}

