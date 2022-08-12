using Assets.Scripts.WebApi;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SearchMenuCheck : MonoBehaviour
{
    public InputField GameIdInput;
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

    //private ErrorHandler errorHandler;


    private static CancellationTokenSource cts = new CancellationTokenSource();
    private WebApiManager apiManager;
    private GameSettings gameSettings;
    //private ErrorHandler errorHandler;


    void Start() {
        apiManager = FindObjectOfType<WebApiManager>();
        gameSettings = FindObjectOfType<GameSettings>();
        //errorHandler = FindObjectOfType<ErrorHandler>();
    }

    public async void Join() {
        try {
            int gameId = int.Parse(GameIdInput.text);
            await apiManager.GameJoinAsync(gameId, cts.Token);

            gameSettings.GameDTO = await apiManager.GameGetAsync(gameId, cts.Token);

            GameIdText.text = "Id gry: " + gameSettings.GameDTO.id;

            Player1Ready.SetActive(false);
            Player2Ready.SetActive(false);

            Player1Text.text = gameSettings.GameDTO.gamePlayers[0].apiUser.nick;
            Player1ColorText.text = gameSettings.GameDTO.gamePlayers[0].blackColor ? "Czarny" : "Bia³y";
            if (gameSettings.GameDTO.gamePlayers[0].ready) Player1Ready.SetActive(true);
            
            Player2.SetActive(true);
            Player2Color.SetActive(true);
            Player2Text.text = gameSettings.GameDTO.gamePlayers[1].apiUser.nick;
            Player2ColorText.text = gameSettings.GameDTO.gamePlayers[1].blackColor ? "Czarny" : "Bia³y";

            InvokeRepeating("UpdateGame", 1.0f, 0.8f);

            NextMenu.SetActive(true);
            CurrentMenu.SetActive(false);
        }
        catch (System.Exception) {
            ErrorMenu.SetActive(true);
        }
    }


    private async void UpdateGame() {
        gameSettings.GameDTO = await apiManager.GameGetAsync(gameSettings.GameDTO.id, cts.Token);
        if (!gameSettings.AllPlayersReady) {
            if (gameSettings.GameDTO.gamePlayers[0].ready) {
                Player1Ready.SetActive(true);
            }
            if (gameSettings.GameDTO.gamePlayers[0].ready && gameSettings.GameDTO.gamePlayers[1].ready) {
                gameSettings.AllPlayersReady = true;
            }
        }
        if(gameSettings.GameDTO.gameStart != null) {
            gameSettings.GameOnline = true;
            SceneManager.LoadScene("BoardKM");
        }
    }

}
