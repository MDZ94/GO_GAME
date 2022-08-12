using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTurn : MonoBehaviour
{

    int passCounter =0;

    
    void Start()
    {
        turnManager = GameObject.FindObjectOfType<TurnManager>();
    }
    TurnManager turnManager;

    public void PassingTurn()
    {
        turnManager = GameObject.FindObjectOfType<TurnManager>();
        turnManager.HasPassed = true;
    }

}
