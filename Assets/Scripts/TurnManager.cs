using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : Singleton<TurnManager>
{

    //private void Start()
    //{
        
    //}

    public int NumberOfPlayers = 2;
    public int CurrentPlayerId = 0;
    public GameObject result;
    
    public bool IsDoneClicking = false;
    public bool HasPassed = false;
    private int PassCounter = 0;
    PlayerTurnDisplay playerTurnDisplay;

    public int PlayerId
    {
        get
        {
            return CurrentPlayerId;
        }
    }

    public void NewTurn()
    {
        IsDoneClicking = false;
        HasPassed = false;

        playerTurnDisplay = GameObject.FindObjectOfType<PlayerTurnDisplay>();
        CurrentPlayerId = (CurrentPlayerId +1) % NumberOfPlayers;
        playerTurnDisplay.Switcher();
    }

    public void Surrender()
    {
        result.SetActive(true);
        return;
    }



    // Update is called once per frame
    void Update()
    {
        if(IsDoneClicking)
        {
            NewTurn();
            PassCounter = 0;
            return;
        }
        else if (HasPassed)
        {
            NewTurn();
            PassCounter++;
            if (PassCounter == 2)
            {
                result.SetActive(true);
            }
            return;
        }
    }
}
