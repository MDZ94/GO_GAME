using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTurnDisplay : MonoBehaviour
{
    BoardLogic boardLogic;
    private GameSettings gameSettings;
    // Start is called before the first frame update
    void Start()
    {
        theTurnManager = GameObject.FindObjectOfType<TurnManager>();
        myText = GetComponent<TextMeshProUGUI>();
        gameSettings = FindObjectOfType<GameSettings>();
        myText.text = "Tura gracza: Czarny";
    }

    TurnManager theTurnManager;
    TextMeshProUGUI myText;

    int turn = 0;

    string[] numberWords = { "Czarny", "Bialy" };
    public void Change()
    {
        turn = (turn + 1) % 2;
        Switcher();
    }

    public void Switcher()
    {
        if (gameSettings.GameOnline)
        {
            myText.text = "Tura gracza: " + numberWords[theTurnManager.CurrentPlayerId];
        }
        else if (turn == 0)
        {
            myText.text = "Tura gracza: " + numberWords[0];
        }
        else
        {
            myText.text = "Tura gracza: " + numberWords[1];
        }
    }
    
    
}
