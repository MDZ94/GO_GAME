using Assets.Scripts;
using Assets.Scripts.WebApi;
using Assets.Scripts.WebApi.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public TurnManager turnManger;
    public GameObject result;
    public Text ResultText;

    //public GameObject P1T;
    //public GameObject P1CT;
    //public GameObject P2T;
    //public GameObject P2CT;


    public TextMeshProUGUI Player1Text;
    public TextMeshProUGUI Player1ColorText;
    public Text Player1PassText;
    public TextMeshProUGUI Player2Text;
    public TextMeshProUGUI Player2ColorText;
    public TextMeshProUGUI Player2PassText;

    public static BoardManager Instance { set; get; }
    private BoardLogic logic;
    public Token[,] Tokens { get; set; }
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    private int selectionX = -1;
    private int selectionY = -1;
    public int boardsize = 9;
    private LineDrawer drawer;
    private ResultCounter resultCounter;
    public List<GameObject> tokenPrefabs;
    private List<StoneOnline> stonesOnline = new List<StoneOnline>();
    private List<MoveDTO> moves = new List<MoveDTO>();
    private GameSettings gameSettings;
    private WebApiManager webApiManager;
    private static CancellationTokenSource cts = new CancellationTokenSource();
    private int passes = 0;
    private bool gameFinished = false;
    public ITokenManager TokenFactory;

    private void Start()
    {
        Instance = this;
        result.SetActive(false);
        Tokens = new Token[boardsize + 1, boardsize + 1];
        
        logic = new BoardLogic();
        logic.LogicInit();

        resultCounter = new ResultCounter();

        gameSettings = FindObjectOfType<GameSettings>();
        webApiManager = FindObjectOfType<WebApiManager>();

        if (gameSettings.GameOnline) {
            Player1Text.text = gameSettings.GameDTO.gamePlayers[0].apiUser.nick;
            Player1ColorText.text = gameSettings.GameDTO.gamePlayers[0].blackColor ? "Czarny" : "Bia�y";

            Player2Text.text = gameSettings.GameDTO.gamePlayers[1].apiUser.nick;
            Player2ColorText.text = gameSettings.GameDTO.gamePlayers[1].blackColor ? "Czarny" : "Bia�y";

            InvokeRepeating("CheckMovesOnline", 1.0f, 0.8f);
        }
        
        TokenFactory = this.GetComponent<TokenManager>();
        TokenFactory.FactoryInit();
        drawer = this.GetComponent<LineDrawer>();
        DrawBoard();
    }

    private async void Update()
    {
        if (!gameFinished) {
            UpdateSelection();
            if (!gameSettings.GameOnline) {
                //DrawBoard();
                logic.AllowedMovesChecking(); // do weryfikacji !!!!!!!!!!!!
            }


            if (Input.GetMouseButtonDown(0)) {
                if (selectionX >= 0 && selectionY >= 0) {
                    if (!gameSettings.GameOnline) {
                        logic.LogicChecking(GetTileCenter(selectionX, selectionY));
                        turnManger.NewTurn();
                    }
                    else {
                        try {
                            var selection = GetTileCenter(selectionX, selectionY);
                            var moveDTO = await webApiManager.MoveAddAsync(gameSettings.GameDTO.id, MoveType.putStone, (short)(selection.x+1), (short)(selection.z+1), cts.Token);
                            if (moves.Where(m => m.Id == moveDTO.Id).Count() == 0) moves.Add(moveDTO);
                            SpawnTokenOnline(moveDTO.ApiUser.id == gameSettings.GameDTO.gamePlayers.FirstOrDefault(p => p.blackColor).apiUser.id ? 0 : 1, moveDTO.PosX, moveDTO.PosY);
                            turnManger.NewTurn();
                            passes = 0;
                        }
                        catch (Exception) {
                        
                        }
                    }

                }
            }
        }

    }

    private void UpdateSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("BoardLayer")))
        {
            selectionX = (int)(hit.point.x + TILE_OFFSET);
            selectionY = (int)(hit.point.z + TILE_OFFSET);
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    public void SpawnToken(int index, int x, int y)
    {
        TokenFactory.SpawnToken(index,GetTileCenter(x, y));
        /*GameObject go = Instantiate(tokenPrefabs[index], GetTileCenter(x, y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        Tokens[x, y] = go.GetComponent<Token>();
        Tokens[x, y].SetPosition(x, y);*/

    }

    public void SpawnTokenOnline(int index, int x, int y) {
        x--;
        y--;
        GameObject go = Instantiate(tokenPrefabs[index], GetTileCenter(x, y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        stonesOnline.Add(new StoneOnline { X = x, Y = y, GameObject = go });
        turnManger.NewTurn();
    }

    public void RemoveToken(int x, int y)
    {
        TokenFactory.RemoveToken(GetTileCenter(x, y));
        /*if (Tokens[x, y] != null)
        {
            Destroy(Tokens[x, y].gameObject);
        }
        Tokens[x, y] = null;*/
    }

    public void RemoveTokenOnline(int x, int y) {
        x--;
        y--;
        var stone = stonesOnline.FirstOrDefault(s => s.X == x && s.Y == y);
        if(stone != null) {
            Destroy(stone.GameObject);
            stonesOnline.Remove(stone);
        }
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x);
        origin.z += (TILE_SIZE * y);
        return origin;
    }
    private void DrawBoard()
    {
        Vector3 widthLine = Vector3.right * boardsize;
        Vector3 heightLine = Vector3.forward * boardsize;

        for (int rowCounter = 0; rowCounter <= boardsize; rowCounter++)
        {

            Vector3 start = Vector3.forward * rowCounter/2;
            //Debug.DrawLine(start, start + widthLine);
            drawer.DrawLine(start, start + widthLine);

            for (int rollCounter = 0; rollCounter <= boardsize; rollCounter++)
            {
                start = Vector3.right * rollCounter/2;
                //Debug.DrawLine(start, start + heightLine);
                drawer.DrawLine(start, start + heightLine);
            }
        }
        //Draw the selection
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * ((float)selectionY + 0.25f) + Vector3.right * ((float)selectionX + 0.25f),
                Vector3.forward * ((float)selectionY - 0.25f) + Vector3.right * ((float)selectionX - 0.25f)
            );

            Debug.DrawLine(
                Vector3.forward * ((float)selectionY + 0.25f) + Vector3.right * ((float)selectionX - 0.25f),
                Vector3.forward * ((float)selectionY - 0.25f) + Vector3.right * ((float)selectionX + 0.25f)
            );
        }
    }



    private async void CheckMovesOnline() {
        int lastMoveId = 0;
        if(moves.Count() > 0) lastMoveId = moves[moves.Count - 1].Id;

        var lastMoves = await webApiManager.GetMoves(gameSettings.GameDTO.id, lastMoveId, cts.Token);

        foreach(MoveDTO moveDTO in lastMoves) {
            if (moveDTO.Type == MoveType.capture) RemoveTokenOnline(moveDTO.PosX, moveDTO.PosY);
            else if (moveDTO.Type == MoveType.putStone) {
                SpawnTokenOnline(moveDTO.ApiUser.id == gameSettings.GameDTO.gamePlayers.FirstOrDefault(p => p.blackColor).apiUser.id ? 0 : 1, moveDTO.PosX, moveDTO.PosY);
                turnManger.NewTurn();
                passes = 0;
            }
            else if (moveDTO.Type == MoveType.pass) passes++;
        }
    }

    public async void Pass() {
        await webApiManager.MoveAddAsync(gameSettings.GameDTO.id, MoveType.pass, 0, 0, cts.Token);
        passes++;
        if (passes == 2) {
            gameFinished = true;
            gameSettings.GameDTO = await webApiManager.GameGetAsync(gameSettings.GameDTO.id, cts.Token);
            result.SetActive(true);
            var score = await webApiManager.GetScore(gameSettings.GameDTO.id, cts.Token);
            ResultText.text = "czarny: " + score.BlackTerritory + Environment.NewLine +
                    "bia�y: " + score.WhiteTerritory + Environment.NewLine +
                    "neutralne: " + score.NeutralTerritory;
        }
    }
}
