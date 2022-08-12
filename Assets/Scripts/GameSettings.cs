using Assets.Scripts.WebApi.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public bool GameOnline { get; set; } = false;
    public GameDTO GameDTO { get; set; }
    public bool WaitingForPlayer { get; set; } = false;
    public bool AllPlayersReady { get; set; } = false;

    public void Clear() {
        GameDTO = null;
        WaitingForPlayer = false;
        AllPlayersReady = false;
    }
}
