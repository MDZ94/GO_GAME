using Assets.Scripts.WebApi;
using Assets.Scripts.WebApi.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class OnlineGameManagerKM : MonoBehaviour
{

    private GameSettings gameSettings;
    private WebApiManager webApiManager;
    private CancellationTokenSource cts = new CancellationTokenSource();
    private BoardManagerKM boardManager;
    private UIManagerKM uiManager;

    private List<MoveDTO> moves = new List<MoveDTO>();


    private bool blackRound = true;
    private int passes = 0;
    private bool gameFinished = false;


    void Start() {
        gameSettings = FindObjectOfType<GameSettings>();
        webApiManager = FindObjectOfType<WebApiManager>();
        boardManager = FindObjectOfType<BoardManagerKM>();
        uiManager = FindObjectOfType<UIManagerKM>();
        InvokeRepeating("CheckMovesOnline", 1.0f, 0.3f);
    }

    void Update() {

    }

    public async void PutStone(Vector2 boardPos) {
        var moveDTO = await webApiManager.MoveAddAsync(gameSettings.GameDTO.id, MoveType.putStone, (short)(boardPos.x + 1), (short)(boardPos.y + 1), cts.Token);
        if (moves.Where(m => m.Id == moveDTO.Id).Count() == 0) moves.Add(moveDTO);
        boardManager.SpawnStone(new Vector2Int(moveDTO.PosX - 1, moveDTO.PosY - 1), moveDTO.ApiUser.id == gameSettings.GameDTO.gamePlayers.FirstOrDefault(p => p.blackColor).apiUser.id ? true : false);
        NextTurn();
        passes = 0;
    }

    private async void CheckMovesOnline() {
        if (!gameFinished) {
            int lastMoveId = 0;
            if (moves.Count() > 0) lastMoveId = moves[moves.Count - 1].Id;

            var lastMoves = await webApiManager.GetMoves(gameSettings.GameDTO.id, lastMoveId, cts.Token);

            foreach (MoveDTO moveDTO in lastMoves) {
                if (moveDTO.Type == MoveType.capture) boardManager.DestroyStone(new Vector2Int(moveDTO.PosX - 1, moveDTO.PosY - 1));
                else if (moveDTO.Type == MoveType.putStone) {
                    boardManager.SpawnStone(new Vector2Int(moveDTO.PosX - 1, moveDTO.PosY - 1), moveDTO.ApiUser.id == gameSettings.GameDTO.gamePlayers.FirstOrDefault(p => p.blackColor).apiUser.id ? true : false);
                    NextTurn();
                    passes = 0;
                }
                else if (moveDTO.Type == MoveType.pass) {
                    passes++;
                    await CheckPasses();
                }
                else if (moveDTO.Type == MoveType.surrender) {
                    await FinishGame(false);
                }
                if (moves.Where(m => m.Id == moveDTO.Id).Count() == 0) moves.Add(moveDTO);
            }
        }
    }

    public async void Pass() {
        var moveDTO = await webApiManager.MoveAddAsync(gameSettings.GameDTO.id, MoveType.pass, 0, 0, cts.Token);
        if (moves.Where(m => m.Id == moveDTO.Id).Count() == 0) moves.Add(moveDTO);
        passes++;
        await CheckPasses();
        Debug.Log("Pass");
    }

    private async Task CheckPasses() {
        if (passes == 2) {
            await FinishGame(false);
        }
    }

    public async void Surrender() {
        await webApiManager.MoveAddAsync(gameSettings.GameDTO.id, MoveType.surrender, 0, 0, cts.Token);
        await FinishGame(true);
    }

    private async Task FinishGame(bool surrender) {
        gameFinished = true;
        gameSettings.GameDTO = await webApiManager.GameGetAsync(gameSettings.GameDTO.id, cts.Token);
        var score = await webApiManager.GetScore(gameSettings.GameDTO.id, cts.Token);


        uiManager.ShowResult("czarny: " + score.BlackTerritory + Environment.NewLine +
                "bia³y: " + score.WhiteTerritory + Environment.NewLine +
                "neutralne: " + score.NeutralTerritory);
        gameFinished = true;
    }

    private void NextTurn() {
        blackRound = !blackRound;
        uiManager.SetTurnText("Runda gracza: " + (blackRound ? "Czarny" : "Bia³y"));
    }
}
