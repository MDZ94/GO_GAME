using Assets.Scripts.WebApi;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WaitJoinCheck : MonoBehaviour
{
    public GameObject Player2ReadyText;
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
            Player2ReadyText.SetActive(true);
            ReadyBtn.SetActive(false);
        }
        catch (System.Exception em) {
            errorHandler.ErrorShow(em.Message);
        }
    }

}
