using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.UI;

public enum Team
{
    White,
    Black,
}

public enum SpecialMove
{
    None = 0,
    EnPassant,
    Castling,
    Promotion
}


public class Chessboard : MonoBehaviour
{
    [Header("Art")]
    [SerializeField] private Material _tileMaterial;
    [SerializeField] private float _tileSize;
    [SerializeField] private float _yOffset;
    [SerializeField] private Vector3 _boardCenter;
    [SerializeField] private LayerMask _tileLayer;

    [Header("Prefab and Team Materials")]
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private Material[] _teamMaterials;
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private Material _moveHighlightMaterial;
    [SerializeField] private float _deadSpacing = 0.3f;
    [SerializeField] private float _deadScale = 0.5f;
    [SerializeField] private GameObject _victoryScreen;
    [SerializeField] private GameObject _rematchIndicator;
    [SerializeField] private Button _rematchButton;

    private ChessPiece _currentPiece;
    private List<Vector2Int> _currentAvailableMoves = new List<Vector2Int>();
    //private Team _currentTeam = Team.White;
    private bool _isWhiteTurn;

    private List<ChessPiece> deadWhites = new List<ChessPiece>();
    private List<ChessPiece> deadBlacks = new List<ChessPiece>();

    private ChessPiece[,] _chessPieces;
    private static int TILE_COUNT_X = 8;
    private static int TILE_COUNT_Y = 8;
    private Camera _currentCamera;
    private GameObject[,] _tileGameobjects;
    private Tile[,] _tiles;
    private Vector3 _bounds;

    //multiplayer logic
    private int _playerCount = -1;
    private int _currentTeam = -1;
    private bool[] playerRematch = new bool[2];

    //special moves
    private List<Vector2Int[]> _moveList = new List<Vector2Int[]>();
    private SpecialMove _specialMove;

    private bool  isLocalGame;
    private void Start()
    {
        GenerateAllTiles(_tileSize, TILE_COUNT_X, TILE_COUNT_Y);
        StartNewGame();
        RegisterToEvent();
    }


    public void StartNewGame()
    {
        _isWhiteTurn = true;
        SpawnAllPieces();
        PositioningAllPieces();
    }

    //Generating board
    private void GenerateAllTiles(float tileSize, float tileCountX, float tileCountY)
    {
        _bounds = new Vector3((tileCountX / 2) * tileSize, 0, (tileCountY / 2) * tileSize) + _boardCenter;
        _yOffset += transform.position.y;

        _tileGameobjects = new GameObject[TILE_COUNT_X, TILE_COUNT_Y];
        _tiles = new Tile[TILE_COUNT_X, TILE_COUNT_Y];
        for (int x = 0; x < TILE_COUNT_X; x++)
        {
            for (int y = 0; y < TILE_COUNT_Y; y++)
            {
                _tileGameobjects[x, y] = GenerateSingleTile(tileSize, x, y);

            }
        }
    }
    private GameObject GenerateSingleTile(float tileSize, int x, int y)
    {
        GameObject tileObject = new GameObject(String.Format("X:{0}, Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = _tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, _yOffset, y * tileSize) - _bounds;
        vertices[1] = new Vector3(x * tileSize, _yOffset, (y + 1) * tileSize) - _bounds;
        vertices[2] = new Vector3((x + 1) * tileSize, _yOffset, y * tileSize) - _bounds;
        vertices[3] = new Vector3((x + 1) * tileSize, _yOffset, (y + 1) * tileSize) - _bounds;

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };

        mesh.vertices = vertices;
        mesh.triangles = tris;

        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Tile");
        tileObject.AddComponent<BoxCollider>();
        Tile tile = tileObject.AddComponent<Tile>();
        tile.HightlightMaterial = _highlightMaterial;
        tile.MoveHightlightMaterial = _moveHighlightMaterial;
        _tiles[x, y] = tile;

        return tileObject;
    }

    private void SpawnAllPieces()
    {

        _chessPieces = new ChessPiece[TILE_COUNT_X, TILE_COUNT_Y];
        Team white = Team.White; Team black = Team.Black;
        //spawing white pieces
        //rook
        _chessPieces[0, 0] = SpawnSinglePiece(ChessPieceType.Rook, white);
        _chessPieces[1, 0] = SpawnSinglePiece(ChessPieceType.Knight, white);
        _chessPieces[2, 0] = SpawnSinglePiece(ChessPieceType.Bishop, white);
        _chessPieces[3, 0] = SpawnSinglePiece(ChessPieceType.Queen, white);
        _chessPieces[4, 0] = SpawnSinglePiece(ChessPieceType.King, white);
        _chessPieces[5, 0] = SpawnSinglePiece(ChessPieceType.Bishop, white);
        _chessPieces[6, 0] = SpawnSinglePiece(ChessPieceType.Knight, white);
        _chessPieces[7, 0] = SpawnSinglePiece(ChessPieceType.Rook, white);

        for (int w = 0; w < TILE_COUNT_X; w++)
        {
            _chessPieces[w, 1] = SpawnSinglePiece(ChessPieceType.Pawn, white);
        }

        _chessPieces[0, 7] = SpawnSinglePiece(ChessPieceType.Rook, black);
        _chessPieces[1, 7] = SpawnSinglePiece(ChessPieceType.Knight, black);
        _chessPieces[2, 7] = SpawnSinglePiece(ChessPieceType.Bishop, black);
        _chessPieces[3, 7] = SpawnSinglePiece(ChessPieceType.Queen, black);
        _chessPieces[4, 7] = SpawnSinglePiece(ChessPieceType.King, black);
        _chessPieces[5, 7] = SpawnSinglePiece(ChessPieceType.Bishop, black);
        _chessPieces[6, 7] = SpawnSinglePiece(ChessPieceType.Knight, black);
        _chessPieces[7, 7] = SpawnSinglePiece(ChessPieceType.Rook, black);

        for (int b = 0; b < TILE_COUNT_X; b++)
        {
            _chessPieces[b, 6] = SpawnSinglePiece(ChessPieceType.Pawn, black);
        }

    }

    private void PositioningAllPieces()
    {
        for (int x = 0; x < TILE_COUNT_X; x++)
        {
            for (int y = 0; y < TILE_COUNT_Y; y++)
            {
                if (_chessPieces[x, y] != null)
                {
                    PositioningSinglePiece(x, y, true);
                }
            }
        }
    }

    private void PositioningSinglePiece(int x, int y, bool force = false)
    {
        _chessPieces[x, y].CurrentX = x;
        _chessPieces[x, y].CurrentY = y;
        _chessPieces[x, y].transform.position = GetTileCenter(x, y);
    }
    private ChessPiece SpawnSinglePiece(ChessPieceType type, Team team)
    {

        ChessPiece cp = Instantiate(_prefabs[(int)type - 1]).GetComponent<ChessPiece>();

        cp.Type = type;
        cp.Team = team;
        cp.GetComponent<MeshRenderer>().material = _teamMaterials[(int)team];

        return cp;
    }


    //Game play - moving pieces
    private void Update()
    {
        if (_currentCamera == null)
        {
            _currentCamera = Camera.main;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = _currentCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo, 100, _tileLayer))
            {
                Vector2Int hitPosition = LookupTileIndex(hitInfo.transform.gameObject);

                //look if we can move
                if (_currentPiece != null && _currentAvailableMoves.Count > 0 && _currentAvailableMoves.Contains(hitPosition))
                {

                    //check for turn and move
                    if ((_currentPiece.Team == Team.White && _isWhiteTurn && _currentTeam == 0) || (_currentPiece.Team == Team.Black && !_isWhiteTurn && _currentTeam == 1))
                    {
                        MoveTo(_currentPiece.CurrentPosition, hitPosition, 0.2f);
                        //tell server we made a move
                        NetMakeMove nmm = new NetMakeMove();
                        nmm.originalX = _currentPiece.CurrentPosition.x;
                        nmm.originalY = _currentPiece.CurrentPosition.y;
                        nmm.destinationX = hitPosition.x;
                        nmm.destinationY = hitPosition.y;
                        nmm.teamId = _currentTeam;

                        Client.Instance.SendToServer(nmm);
                    }
                    //clear previous highligted tiles moves if the exist
                    UnhighlightAvailableTiles(ref _currentAvailableMoves);
                    return;
                }

                //if we click same piece drop ref and unhighlight blocks
                if (_currentPiece != null)
                {

                    if (_currentPiece == _chessPieces[hitPosition.x, hitPosition.y])
                    {
                        _currentPiece?.Deselect();
                        //clear previous highligted tiles moves if the exist
                        UnhighlightAvailableTiles(ref _currentAvailableMoves);
                        _currentPiece = null;
                        return;
                    }
                    else
                    {
                        _currentPiece?.Deselect();
                        //clear previous highligted tiles moves if the exist
                        UnhighlightAvailableTiles(ref _currentAvailableMoves);
                        _currentPiece = null;
                    }
                }


                //we click a new piece
                _currentPiece = _chessPieces[hitPosition.x, hitPosition.y];


                //we select it and highlight available blocks
                if (_currentPiece != null && (int)_currentPiece.Team == _currentTeam)
                {
                    _currentPiece.Select();

                    //clear previous highligted tiles moves if the exist
                    UnhighlightAvailableTiles(ref _currentAvailableMoves);

                    //get all new available tiles to moves
                    _currentAvailableMoves = _currentPiece.GetAvailableMoves(ref _chessPieces, TILE_COUNT_X, TILE_COUNT_Y);


                    //get list of special moves and add them to _currentAvailableMoves (passed as ref)
                    _specialMove = _currentPiece.GetSpecialMoves(ref _chessPieces, ref _moveList, ref _currentAvailableMoves);

                    //remove any move - that may lead to check
                    PreventCheck();

                    //highlight tiles for new available moves
                    HighlightAvailableTiles(ref _currentAvailableMoves);
                }


            }
        }





    }
    private void MoveTo(Vector2Int previousPosIndex, Vector2Int newPosIndex, float time)
    {
        Coroutine cor = StartCoroutine(LerpPosition(previousPosIndex, newPosIndex, time));
    }
    private IEnumerator LerpPosition(Vector2Int previousPosIndex, Vector2Int newPosIndex, float time)
    {
        _currentPiece = _chessPieces[previousPosIndex.x, previousPosIndex.y];

        //Vector3 previousPosition = _chessPieces[previousPosIndex.x, previousPosIndex.y].transform.position;
        Vector3 previousPosition = GetTileCenter(previousPosIndex.x, previousPosIndex.y);
        Vector3 newPosition = GetTileCenter(newPosIndex.x, newPosIndex.y);
        float elapsedTime = 0;


        while (elapsedTime < time)
        {
            _chessPieces[previousPosIndex.x, previousPosIndex.y].transform.position = Vector3.Lerp(previousPosition, newPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (_chessPieces[newPosIndex.x, newPosIndex.y] != null)
        {
            Debug.Log("Destroyed : " + _chessPieces[newPosIndex.x, newPosIndex.y].name);
            ChessPiece killedChessPiece = _chessPieces[newPosIndex.x, newPosIndex.y];
            MoveToSide(killedChessPiece);
        }

        _chessPieces[newPosIndex.x, newPosIndex.y] = _chessPieces[previousPosIndex.x, previousPosIndex.y];
        _chessPieces[newPosIndex.x, newPosIndex.y].CurrentX = newPosIndex.x;
        _chessPieces[newPosIndex.x, newPosIndex.y].CurrentY = newPosIndex.y;
        _chessPieces[previousPosIndex.x, previousPosIndex.y] = null;
        _currentPiece.Deselect();
        _currentPiece = null;
        _isWhiteTurn = !_isWhiteTurn;
        if (isLocalGame) {
            _currentTeam = (_isWhiteTurn) ? 0 : 1;
        }

        //adding move after it has been made
        _moveList.Add(new Vector2Int[] { new Vector2Int(previousPosIndex.x, previousPosIndex.y), new Vector2Int(newPosIndex.x, newPosIndex.y) });

        //some extra processing for special move
        ProcessSpecialMove();
        if (CheckForCheckmate())
        {
            CheckMate(_chessPieces[newPosIndex.x, newPosIndex.y].Team);
        }

        Debug.Log("This was called");
    }

    //special moves
    public void ProcessSpecialMove()
    {
        //en passant doesnt kill/replaces the enemy pawn directly
        //the position of killed pawn and the attacking pawn remains different
        //so when enPassant happens we need to make sure
        //we remove the killed pawn from the board 
        if (_specialMove == SpecialMove.EnPassant)
        {
            var newMove = _moveList[_moveList.Count - 1];
            ChessPiece myPawn = _chessPieces[newMove[1].x, newMove[1].y];
            var targetPawnPosition = _moveList[_moveList.Count - 2];
            ChessPiece enemyPawn = _chessPieces[targetPawnPosition[1].x, targetPawnPosition[1].y];

            if (myPawn.CurrentX == enemyPawn.CurrentX)
            {
                if (myPawn.CurrentY == enemyPawn.CurrentY - 1 || myPawn.CurrentY == enemyPawn.CurrentY + 1)
                {
                    if (enemyPawn.Team == Team.White)
                    {
                        deadWhites.Add(enemyPawn);
                        enemyPawn.SetPosition(new Vector3(8 * _tileSize, _yOffset)
                            - _bounds
                             + new Vector3(_tileSize / 2, 0, _tileSize / 2)
                             + Vector3.forward * deadWhites.Count * _deadSpacing); ;
                        enemyPawn.SetScale(enemyPawn.transform.localScale * _deadScale);
                    }
                    else
                    {
                        deadBlacks.Add(enemyPawn);
                        enemyPawn.SetPosition(new Vector3(0, _yOffset, 8 * _tileSize)
                            - _bounds
                            - new Vector3(_tileSize / 2, 0, _tileSize / 2)
                            + Vector3.back * deadBlacks.Count * _deadSpacing); ;
                        enemyPawn.SetScale(enemyPawn.transform.localScale * _deadScale);
                    }
                    _chessPieces[enemyPawn.CurrentX, enemyPawn.CurrentY] = null;
                }
            }
        }


        //promotion
        if (_specialMove == SpecialMove.Promotion)
        {
            Vector2Int[] lastMove = _moveList[_moveList.Count - 1];
            //landing position of pawn
            ChessPiece targetPawn = _chessPieces[lastMove[1].x, lastMove[1].y];

            if (targetPawn.Type == ChessPieceType.Pawn)
            {
                if (targetPawn.Team == Team.White && lastMove[1].y == 7)
                {
                    ChessPiece newQueen = SpawnSinglePiece(ChessPieceType.Queen, Team.White);
                    Destroy(_chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
                    _chessPieces[lastMove[1].x, lastMove[1].y] = newQueen;
                    PositioningSinglePiece(lastMove[1].x, lastMove[1].y, true);
                }

                if (targetPawn.Team == Team.Black && lastMove[1].y == 0)
                {
                    ChessPiece newQueen = SpawnSinglePiece(ChessPieceType.Queen, Team.Black);
                    Destroy(_chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
                    _chessPieces[lastMove[1].x, lastMove[1].y] = newQueen;
                    PositioningSinglePiece(lastMove[1].x, lastMove[1].y, true);
                }
            }

        }

        //we need to move two pieces
        //king and the rook
        if (_specialMove == SpecialMove.Castling)
        {
            Vector2Int[] lastMove = _moveList[_moveList.Count - 1];
            //we moved the king towards LEFT ROOK - we need to move rook now
            if (lastMove[1].x == 2)
            {
                if (lastMove[1].y == 0)
                { //white king move 
                    ChessPiece leftRook = _chessPieces[0, 0];//white left rook
                    _chessPieces[3, 0] = leftRook;
                    PositioningSinglePiece(3, 0);
                    _chessPieces[0, 0] = null;
                }

                else if (lastMove[1].y == 7)
                {//black king move
                    ChessPiece leftRook = _chessPieces[0, 7];//white left rook
                    _chessPieces[3, 7] = leftRook;
                    PositioningSinglePiece(3, 7);
                    _chessPieces[0, 7] = null;
                }
            }

            //we moved the king toward RIGHT ROOK
            else if (lastMove[1].x == 6)
            {
                if (lastMove[1].y == 0)
                { //white king move 
                    ChessPiece leftRook = _chessPieces[7, 0];//white left rook
                    _chessPieces[5, 0] = leftRook;
                    PositioningSinglePiece(5, 0);
                    _chessPieces[7, 0] = null;
                }

                else if (lastMove[1].y == 7)
                {//black king move
                    ChessPiece leftRook = _chessPieces[7, 7];//white left rook
                    _chessPieces[5, 7] = leftRook;
                    PositioningSinglePiece(5, 7);
                    _chessPieces[7, 7] = null;
                }
            }
        }

    }

    public void PreventCheck()
    {
        //to prevent any move that puts our king in danfer
        ChessPiece targetKing = null;

        for (int x = 0; x < TILE_COUNT_X; x++)
        {
            for (int y = 0; y < TILE_COUNT_Y; y++)
            {
                if (_chessPieces[x, y] != null)
                {
                    //our team king
                    if (_chessPieces[x, y].Type == ChessPieceType.King && _chessPieces[x, y].Team == _currentPiece.Team)
                    {
                        targetKing = _chessPieces[x, y];
                        break;
                    }
                }
            }
        }

        //simulating moves for our current piece - removing moves that lead to check
        //sending ref currentAvailableMoves so we can remove such moves
        SimulateMoveForSinglePiece(_currentPiece, ref _currentAvailableMoves, targetKing);

    }


    public void SimulateMoveForSinglePiece(ChessPiece cp, ref List<Vector2Int> moves, ChessPiece targetKing)
    {
        //Save the current values, to reset after the function call
        int actualX = cp.CurrentX;
        int actualY = cp.CurrentY;
        List<Vector2Int> movesToRemove = new List<Vector2Int>();

        //Going through all the moves, simulate them and check if we are in check
        //looping through all the moves of the current selected piece
        for (int i = 0; i < moves.Count; i++)
        {
            //possible moves of our pieces
            int simX = moves[i].x;
            int simY = moves[i].y;

            Vector2Int kingPositionThisSim = new Vector2Int(targetKing.CurrentX, targetKing.CurrentY);
            //if are we moving the king itself
            if (cp.Type == ChessPieceType.King)
            {
                kingPositionThisSim = new Vector2Int(simX, simY);
            }

            //lets copy chesspeices 2d array - so we dont chnage it
            ChessPiece[,] simPieces = new ChessPiece[TILE_COUNT_X, TILE_COUNT_Y];  //fake chess board
            //all the attacking pieces of the opposite team
            List<ChessPiece> simAttackingPieces = new List<ChessPiece>(); // fake enemy pieces
            for (int x = 0; x < TILE_COUNT_X; x++)
            {
                for (int y = 0; y < TILE_COUNT_Y; y++)
                {
                    if (_chessPieces[x, y] != null)
                    {
                        simPieces[x, y] = _chessPieces[x, y];
                        //add all the opposite team pieces
                        if (simPieces[x, y].Team != cp.Team)
                        {
                            simAttackingPieces.Add(simPieces[x, y]);  //here we are using copied data
                        }
                    }
                }
            }

            //simulate that move
            //let suppose we moved our piece
            simPieces[actualX, actualY] = null;
            //we will reset them later - just for sim
            cp.CurrentX = simX;
            cp.CurrentY = simY;
            //we moved the piece to this new place
            simPieces[simX, simY] = cp;

            //Did we killed one of the enemy team piece when we move
            var deadPiece = simAttackingPieces.Find(chessPiece => chessPiece.CurrentX == simX && chessPiece.CurrentY == simY);
            //remove the dead piece from simulation
            if (deadPiece != null)
            {
                simAttackingPieces.Remove(deadPiece);
            }

            //Get all the moves of attacking piece and check if our king is in danger
            List<Vector2Int> simMoves = new List<Vector2Int>();
            for (int a = 0; a < simAttackingPieces.Count; a++)
            {
                var piecesMoves = simAttackingPieces[a].GetAvailableMoves(ref simPieces, TILE_COUNT_X, TILE_COUNT_Y);
                for (int b = 0; b < piecesMoves.Count; b++)
                {
                    simMoves.Add(piecesMoves[b]);
                }
            }

            //is the king is under check ?if so remove the move
            //we are checking if their is piece that is attacking king present in the list of enemy moves
            if (ContainsValidMove(ref simMoves, kingPositionThisSim))
            {
                movesToRemove.Add(moves[i]);
            };

            //restore actual cp data
            cp.CurrentX = actualX;
            cp.CurrentY = actualY;
        }


        //remove from the current available moves
        for (int i = 0; i < movesToRemove.Count; i++)
        {
            moves.Remove(movesToRemove[i]);
        }
    }
    public bool CheckForCheckmate()
    {

        Debug.Log("CheckForCheckMate");
        var lastMove = _moveList[_moveList.Count - 1];
        //we made a checkmate move - so our target is opposite team and its king
        Team targetTeam = (_chessPieces[lastMove[1].x, lastMove[1].y].Team == Team.White) ? Team.Black : Team.White;

        List<ChessPiece> attackingPieces = new List<ChessPiece>();
        List<ChessPiece> defendingPieces = new List<ChessPiece>();
        ChessPiece targetKing = null;
        for (int x = 0; x < TILE_COUNT_X; x++)
        {
            for (int y = 0; y < TILE_COUNT_Y; y++)
            {
                if (_chessPieces[x, y] != null)
                {
                    //populating defending and attacking pieces
                    if (_chessPieces[x, y].Team == targetTeam)
                    {
                        defendingPieces.Add(_chessPieces[x, y]);
                        if (_chessPieces[x, y].Type == ChessPieceType.King)
                        {
                            targetKing = _chessPieces[x, y];
                        }
                    }
                    else
                    {
                        attackingPieces.Add(_chessPieces[x, y]);
                    }
                }
            }
        }

        //Is the king attacked right now?
        List<Vector2Int> attackingPiecesMoves = new List<Vector2Int>();
        for (int i = 0; i < attackingPieces.Count; i++)
        {
            var piecesMoves = attackingPieces[i].GetAvailableMoves(ref _chessPieces, TILE_COUNT_X, TILE_COUNT_Y);
            for (int b = 0; b < piecesMoves.Count; b++)
            {
                attackingPiecesMoves.Add(piecesMoves[b]);
            }
        }

        //check if our king is under attack - king under check
        if (ContainsValidMove(ref attackingPiecesMoves, new Vector2Int(targetKing.CurrentX, targetKing.CurrentY)))
        {
            //king is under attack - can we move piece to defend king?
            for (int i = 0; i < defendingPieces.Count; i++)
            {
                List<Vector2Int> defendingMoves = defendingPieces[i].GetAvailableMoves(ref _chessPieces, TILE_COUNT_X, TILE_COUNT_Y);
                //since we sending ref defendingMoves, we will delete moves
                SimulateMoveForSinglePiece(defendingPieces[i], ref defendingMoves, targetKing);
                //we have some moves to defend the king
                if (defendingMoves.Count != 0)
                {
                    Debug.Log("Checkmate False");
                    return false; // we are not checkmated

                }
            }

            Debug.Log("Checkmate true");
            return true;

        }


        Debug.Log("Checkmate False");
        return false;
    }



    public void MoveToSide(ChessPiece ocp)
    {
        if (ocp.Team == Team.White)
        {

            deadWhites.Add(ocp);
            ocp.SetPosition(new Vector3(8 * _tileSize, _yOffset)
                - _bounds
                 + new Vector3(_tileSize / 2, 0, _tileSize / 2)
                 + Vector3.forward * deadWhites.Count * _deadSpacing); ;
            ocp.SetScale(ocp.transform.localScale * _deadScale);
        }

        if (ocp.Team == Team.Black)
        {

            deadBlacks.Add(ocp);

            ocp.SetPosition(new Vector3(0, _yOffset, 8 * _tileSize)
                - _bounds
                - new Vector3(_tileSize / 2, 0, _tileSize / 2)
                + Vector3.back * deadBlacks.Count * _deadSpacing); ;
            ocp.SetScale(ocp.transform.localScale * _deadScale);

        }
    }

    //moves highlight
    public void HighlightAvailableTiles(ref List<Vector2Int> moves)
    {

        if (_currentAvailableMoves.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < moves.Count; i++)
        {
            _tiles[moves[i].x, moves[i].y].AvailableToMoveHighligt();
        }
    }
    public void UnhighlightAvailableTiles(ref List<Vector2Int> moves)
    {
        if (_currentAvailableMoves.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < moves.Count; i++)
        {
            _tiles[moves[i].x, moves[i].y].AvailableToMoveUnhighligt();
        }
    }


    //checkmate UI
    public void CheckMate(Team winnerTeam)
    {
        DisplayVictory(winnerTeam);
    }
    public void DisplayVictory(Team winnerTeam)
    {
        _victoryScreen.SetActive(true);
        _victoryScreen.transform.GetChild((int)winnerTeam).gameObject.SetActive(true);
    }
    public void OnRematchButton()
    {

        if (isLocalGame)
        {
            //faking that both team 0 white and 1 black wants a rematch 
            NetRematch wrm = new NetRematch();
            wrm.teamId = 0;
            wrm.wantRematch = 1;
            Client.Instance.SendToServer(wrm);
            
            NetRematch brm = new NetRematch();
            brm.teamId = 1;
            brm.wantRematch = 1;
            Client.Instance.SendToServer(brm);
        }
        else {

            //not a local game - we just telling server that we want a rematch
            NetRematch rm = new NetRematch();
            rm.teamId = _currentTeam;
            rm.wantRematch = 1;
            Client.Instance.SendToServer(rm);
        }

    }


    public void GameReset() {
        Debug.Log("ON RESET");

        _rematchButton.interactable = true;

        _rematchIndicator.transform.GetChild(0).gameObject.SetActive(false);
        _rematchIndicator.transform.GetChild(1).gameObject.SetActive(false);
        
        //disable UI
        _victoryScreen.SetActive(false);
        _victoryScreen.transform.GetChild(0).gameObject.SetActive(false);
        _victoryScreen.transform.GetChild(1).gameObject.SetActive(false);

        //clear chess board and peices
        for (int x = 0; x < TILE_COUNT_X; x++)
        {
            for (int y = 0; y < TILE_COUNT_Y; y++)
            {
                if (_chessPieces[x, y] != null)
                {
                    Destroy(_chessPieces[x, y].gameObject);
                }

                _chessPieces[x, y] = null;
            }
        }

        //clear dead pieces on side
        for (int i = 0; i < deadBlacks.Count; i++)
        {
            Destroy(deadBlacks[i].gameObject);
        }
        deadBlacks.Clear();
        for (int i = 0; i < deadWhites.Count; i++)
        {
            Destroy(deadWhites[i].gameObject);
        }
        deadWhites.Clear();

        //reseting fields
        _currentAvailableMoves.Clear();
        _moveList.Clear();
        _currentPiece = null;
        playerRematch[0] = playerRematch[1] = false;

        StartNewGame();
    }

    public void OnMenuButton()
    {
        NetRematch rm = new NetRematch();
        rm.teamId = _currentTeam;
        rm.wantRematch = 0;
        Client.Instance.SendToServer(rm);

        GameReset();
        GameUI.Instance.OnLeaveFromGameMenu();

        //since we sent a message - just went for a sec and shutdown client and server
        Invoke("Shutdown", 1.0f);

        //reset some values
        _playerCount = -1;
        _currentTeam = -1;
    }


    //Operations

    private bool ContainsValidMove(ref List<Vector2Int> moves, Vector2 pos)
    {
        for (int i = 0; i < moves.Count; i++)
        {
            if (moves[i].x == pos.x && moves[i].y == pos.y)
            {
                return true;
            }
        }

        return false;
    }


    private Vector3 GetTileCenter(int x, int y)
    {
        return new Vector3(x * _tileSize, _yOffset, y * _tileSize) - _bounds + new Vector3(_tileSize / 2, 0, _tileSize / 2);
    }

    private Vector2Int LookupTileIndex(GameObject tile)
    {
        for (int x = 0; x < TILE_COUNT_X; x++)
        {
            for (int y = 0; y < TILE_COUNT_Y; y++)
            {
                if (_tileGameobjects[x, y] == tile)
                {
                    return new Vector2Int(x, y);
                }
            }
        }


        return -Vector2Int.one; // -1 -1  Invalid
    }


    #region Events
    /// <summary>
    /// Registering to static events(triggered by network messages) that are called on this client
    /// </summary>
    private void RegisterToEvent()
    {
        NetUtility.S_WELCOME += OnWelcomeServer;
        NetUtility.C_WELCOME += OnWelcomeClient;

        NetUtility.C_START_GAME += OnStartGameClient;

        NetUtility.S_MAKE_MOVE += OnMakeMoveServer;
        NetUtility.C_MAKE_MOVE += OnMakeMoveClient;

        NetUtility.S_REMATCH += OnRematchServer;
        NetUtility.C_REMATCH += OnRematchClient;
        GameUI.Instance.SetLocalGame += OnSetLocalGame;
    }



    private void UnregisterToEvents() {
        NetUtility.S_WELCOME -= OnWelcomeServer;
        NetUtility.C_WELCOME -= OnWelcomeClient;
        NetUtility.C_START_GAME -= OnStartGameClient;
        NetUtility.S_MAKE_MOVE -= OnMakeMoveServer;
        NetUtility.C_MAKE_MOVE -= OnMakeMoveClient;

        NetUtility.S_REMATCH -= OnRematchServer;
        NetUtility.C_REMATCH -= OnRematchClient;

        GameUI.Instance.SetLocalGame -= OnSetLocalGame;
    }

    private void OnSetLocalGame(bool state)
    {
        _playerCount = -1;
        _currentTeam = -1;
        isLocalGame = state; 
    }

    //Server methods

    /// <summary>
    /// When Server recieved a welcome message form the client.
    /// Client has connected, assign a team and return the message back to him.
    /// </summary>
    /// <param name="msg">messare recieved</param>
    /// <param name="cnn">sender client connection</param>
    private void OnWelcomeServer(NetMessage msg, NetworkConnection cnn)
    {
        NetWelcome nw = msg as NetWelcome;

        //assign a team - when we started server and connects the same device as client
        // we get assigned a 0 team
        nw.AssignedTeam = ++_playerCount;

        //return back to client
        Server.Instance.SendToClient(cnn,nw);


        //If we have two players - and start the game
        if (_playerCount == 1) {
            Server.Instance.Broadcast(new NetStartGame()); 
        }
    }

    public void OnMakeMoveServer(NetMessage msg, NetworkConnection cnn) {
        NetMakeMove nmm = msg as NetMakeMove;

        //broadcast to all client
        Server.Instance.Broadcast(nmm);
    }

    public void OnRematchServer(NetMessage msg, NetworkConnection cnn)
    {
        NetRematch nr = msg as NetRematch;

        //broadcast to all client
        Server.Instance.Broadcast(nr);
    }

    //Client methods

    private void OnWelcomeClient(NetMessage msg)
    {
        //recived the connection message
        NetWelcome nw = msg as NetWelcome;
        //assign the team
        _currentTeam = nw.AssignedTeam;

        if (isLocalGame && _playerCount == 0) {
            Debug.Log("LOCAL GAME STARTED!!!");
             Server.Instance.Broadcast(new NetStartGame());
        }

        Debug.Log($"My assigned team is {nw.AssignedTeam}");
    }

    private void OnMakeMoveClient(NetMessage msg)
    {
        //recived the connection message
        NetMakeMove nmm = msg as NetMakeMove;

        Debug.Log($"Move : {nmm.teamId} : ({nmm.originalX}, {nmm.originalY}) -> ({nmm.destinationX}, {nmm.destinationY})");

        if (nmm.teamId != _currentTeam) {

            ChessPiece target = _chessPieces[nmm.originalX, nmm.originalY];

            //get moves and special moves changes the chesspieces array
            _currentAvailableMoves = target.GetAvailableMoves(ref _chessPieces, TILE_COUNT_X, TILE_COUNT_Y);
            _specialMove = target.GetSpecialMoves(ref _chessPieces, ref _moveList, ref _currentAvailableMoves);
            

            MoveTo(new Vector2Int(nmm.originalX, nmm.originalY), new Vector2Int(nmm.destinationX, nmm.destinationY), 0.2f);
        }
    }

    private void OnRematchClient(NetMessage msg)
    {
        //recived the connection message
        NetRematch rm = msg as NetRematch;

        //set boolean for rematch
        playerRematch[rm.teamId] = (rm.wantRematch == 1) ;

        //Activate piece of UI - other client sent this message
        if (rm.teamId != _currentTeam) {
            //opposite time want a rematch(value 1)/left (value 0)- so set that message active
            _rematchIndicator.transform.GetChild((rm.wantRematch == 1) ? 0 : 1).gameObject.SetActive(true);

            //opposite team doesnt want a rematch
            _rematchButton.interactable = false;
        }

        //if both want to rematch
        if (playerRematch[0] && playerRematch[1]) {
            GameReset();
        }
    }


    private void Shutdown() {
        Client.Instance.ShutDown();
        Server.Instance.ShutDown();
    }

    private void OnStartGameClient(NetMessage msg)
    {
        //we just need to change the camera
        GameUI.Instance.ChangeCamera((_currentTeam == 0) ? (int)CameraAngles.WHITETEAM : (int)CameraAngles.BLACKTEAM);
    }

    #endregion
}
