using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerKM : MonoBehaviour
{
    public TextMeshProUGUI TurnText;
    public TextMeshProUGUI Player1Text;
    public TextMeshProUGUI Player1ColorText;
    public TextMeshProUGUI Player2Text;
    public TextMeshProUGUI Player2ColorText;
    public GameObject ResultGo;
    public Text ResultText;

    private GameSettings gameSettings;

    void Start()
    {
        gameSettings = FindObjectOfType<GameSettings>();

        if (gameSettings.GameOnline) {
            Player1Text.text = gameSettings.GameDTO.gamePlayers[0].apiUser.nick;
            Player1ColorText.text = gameSettings.GameDTO.gamePlayers[0].blackColor ? "Czarny" : "Bia³y";

            Player2Text.text = gameSettings.GameDTO.gamePlayers[1].apiUser.nick;
            Player2ColorText.text = gameSettings.GameDTO.gamePlayers[1].blackColor ? "Czarny" : "Bia³y";
        }
    }

    public void ShowResult(string results) {
        ResultGo.SetActive(true);
        ResultText.text = results;
    }

    public void HideResult() {
        ResultGo.SetActive(false);
    }

    public void SetTurnText(string text) {
        TurnText.text = text;
    }

    public void SetPlayer1Text(string text) {
        Player1Text.text = text;
    }

    public void SetPlayer2Text(string text) {
        Player2Text.text = text;
    }

    public void SetPlayer1ColorText(string text) {
        Player1ColorText.text = text;
    }

    public void SetPlayer2ColorText(string text) {
        Player2ColorText.text = text;
    }
}
