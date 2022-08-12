using Assets.Scripts.WebApi;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitHostCheck : MonoBehaviour
{

    public GameObject Player1ReadyText;
    public GameObject ReadyBtn;

    public GameObject CurrentMenu;
    public GameObject ErrorMenu;

    private static CancellationTokenSource cts = new CancellationTokenSource();
    private WebApiManager apiManager;
    private GameSettings gameSettings;

    private ErrorHandler errorHandler;


    void Start() {
        apiManager = FindObjectOfType<WebApiManager>();
        gameSettings = FindObjectOfType<GameSettings>();
    }

    public async void SetReady() {
        try {
            await apiManager.GameSetReadyAsync(gameSettings.GameDTO.id, cts.Token);
            Player1ReadyText.SetActive(true);
            ReadyBtn.SetActive(false);
        }
        catch (System.Exception em) {
            errorHandler.ErrorShow(em.Message);
        }
    }

    public async void StartGame() {
        try {
            await apiManager.GameStartAsync(gameSettings.GameDTO.id, cts.Token);
            gameSettings.GameOnline = true;
            SceneManager.LoadScene("BoardKM");
        }
        catch (System.Exception me) {
            errorHandler.ErrorShow(me.Message);
        }
    }

}
