using Assets.Scripts.WebApi;
using Assets.Scripts.WebApi.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HostCheck : MonoBehaviour
{
    public InputField NameInput;
    public Dropdown SizeDropdown;
    public Dropdown TimeDropdown;
    public Dropdown ColorDropdown;

    public GameObject NextMenu;
    public GameObject CurrentMenu;
    public GameObject ErrorMenu;

    public TextMeshProUGUI Player1Text;
    public TextMeshProUGUI Player1ColorText;
    public GameObject Player1Ready;

    public GameObject Player2;
    public TextMeshProUGUI Player2Text;
    public GameObject Player2Color;
    public TextMeshProUGUI Player2ColorText;
    public GameObject Player2Ready;

    public TextMeshProUGUI GameIdText;

    private ErrorHandler errorHandler;

    private static CancellationTokenSource cts = new CancellationTokenSource();
    private WebApiManager apiManager;
    private GameSettings gameSettings;

    void Start() {
        apiManager = FindObjectOfType<WebApiManager>();
        gameSettings = FindObjectOfType<GameSettings>();
    }

    public async void Host() {
        var name = NameInput.text;
        var blackColor = ColorDropdown.value == 0 ? true : false;
        var sizeValue = SizeDropdown.value;
        BoardSize boardSize = (BoardSize)sizeValue;
        var time = TimeDropdown.value == 0 ? 15 : 30;

        try {
            gameSettings.GameDTO = await apiManager.GameCreateAsync(name, blackColor, boardSize, time, cts.Token);
            GameIdText.text = "Id gry: " + gameSettings.GameDTO.id;
            Player1Text.text = gameSettings.GameDTO.gamePlayers[0].apiUser.nick;
            Player1ColorText.text = gameSettings.GameDTO.gamePlayers[0].blackColor ? "Czarny" : "Bia³y";
            Player1Ready.SetActive(false);
            Player2.SetActive(false);
            Player2Color.SetActive(false);
            Player2Ready.SetActive(false);
            gameSettings.WaitingForPlayer = true;
            InvokeRepeating("UpdateGame", 1.0f, 0.8f);

            NextMenu.SetActive(true);
            CurrentMenu.SetActive(false);
        }
        catch (System.Exception ex) {
            errorHandler.ErrorShow(ex.Message);
        }
    }


    private async void UpdateGame() {
        if(gameSettings.WaitingForPlayer || !gameSettings.AllPlayersReady) {
            gameSettings.GameDTO = await apiManager.GameGetAsync(gameSettings.GameDTO.id, cts.Token);
        }
        if(gameSettings.WaitingForPlayer) {
            if (gameSettings.GameDTO.gamePlayers.Count == 2) {
                Player2.SetActive(true);
                Player2Color.SetActive(true);
                Player2Text.text = gameSettings.GameDTO.gamePlayers[1].apiUser.nick;
                Player2ColorText.text = gameSettings.GameDTO.gamePlayers[1].blackColor ? "Czarny" : "Bia³y";
                gameSettings.WaitingForPlayer = false;
            }
        }

        if (!gameSettings.AllPlayersReady && gameSettings.GameDTO.gamePlayers.Count == 2) {
            if (gameSettings.GameDTO.gamePlayers.Count == 2 && gameSettings.GameDTO.gamePlayers[1].ready) {
                Player2Ready.SetActive(true);
            }
            if (gameSettings.GameDTO.gamePlayers[0].ready && gameSettings.GameDTO.gamePlayers[1].ready) {
                gameSettings.AllPlayersReady = true;
            }
        }
        
    }

}
