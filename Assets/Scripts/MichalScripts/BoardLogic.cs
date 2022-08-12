using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLogic
{
    public static BoardLogic boardLogicInstance { set; get; }
    public BoardManager manager = BoardManager.Instance;
    public int boardsize = BoardManager.Instance.boardsize;
    private int redScore;
    private int blackScore;
    private RuleCheck ruleCheck;
    private bool[,] allowedMoves { set; get; }
    private bool[,] bannedMoves { set; get; }
    public bool isBlackTurn = true;
    public ResultCounter resultCounter;
    PlayerTurnDisplay playerTurnDisplay;

    void Start()
    {
        playerTurnDisplay = GameObject.FindObjectOfType<PlayerTurnDisplay>();
    }
    public void LogicInit()
    {
        boardLogicInstance = this;
        redScore = 0;
        blackScore = 0;
        allowedMoves = new bool[boardsize + 1, boardsize + 1];
        bannedMoves = new bool[boardsize + 1, boardsize + 1];
        ruleCheck = new RuleCheck();
        resultCounter = new ResultCounter();
    }

    // Update is called once per frame

    public void LogicChecking(Vector3 point)
    {

        if (AllowedMovesCheck(point))
        {
            if (isBlackTurn)
            {
                SpawnToken(0, (int)(point.x), (int)(point.z));
                if (!ruleCheck.koFlag)
                {
                    isBlackTurn = false;
                    Start();
                    playerTurnDisplay.Change();

                }

            }
            else if (!isBlackTurn)
            {
                SpawnToken(1, (int)(point.x), (int)(point.z));
                if (!ruleCheck.koFlag)
                {
                    isBlackTurn = true;
                    Start();
                    playerTurnDisplay.Change();
                }
            }

        }
    }

    private bool AllowedMovesCheck(Vector3 point)
    {
        return allowedMoves[(int)(point.x), (int)(point.z)];

    }
    public void AllowedMovesChecking()
    {
        for (int i = 0; i < boardsize + 1; i++)
        {
            for (int j = 0; j < boardsize + 1; j++)
            {
                if (manager.Tokens[i, j] == null)
                {
                    allowedMoves[i, j] = true;
                }
                else
                {
                    allowedMoves[i, j] = false;
                }
            }
        }

    }

    private void BannedMovesInit()
    {
        for (int i = 0; i < boardsize + 1; i++)
        {
            for (int j = 0; j < boardsize + 1; j++)
            {
                bannedMoves[i, j] = true;
            }
        }

    }
    public void RemoveToken(int x, int y)
    {
        manager.RemoveToken(x, y);
        bannedMoves[x, y] = false;
    }
    private void SpawnToken(int index, int x, int y)
    {

        manager.SpawnToken(index, x, y);
        CheckToken(x, y);

    }
    public void CheckToken(int x, int y)
    {
        ruleCheck.Checking(manager.Tokens[x, y]);
        ruleCheck.Checking(manager.Tokens[x, y]);
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (manager.Tokens[x, y] != null)
                {

                    if (manager.Tokens[x, y].CurrentX + i >= 0 && manager.Tokens[x, y].CurrentY + j >= 0
                    && manager.Tokens[x, y].CurrentX + i < boardsize + 1 && manager.Tokens[x, y].CurrentY + j < boardsize + 1)
                    {
                        if (manager.Tokens[(int)manager.Tokens[x, y].CurrentX + i, (int)manager.Tokens[x, y].CurrentY + j] != null)
                        {
                            if (manager.Tokens[manager.Tokens[x, y].CurrentX + i, manager.Tokens[x, y].CurrentY + j].isBlack != manager.Tokens[x, y].isBlack)
                            {
                                ruleCheck.Checking(manager.Tokens[x + i, y + j]);
                            }
                        }
                    }
                }
            }

        }
        ruleCheck.KoChecking();
        resultCounter.ActualResultCheck();
    }
}
