using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleCheck
{
    private List<HashSet<Vector3>> checkList;
    private Token baseToken = null;
    private Token referenceToken = null;
    private BoardLogic logic = BoardLogic.boardLogicInstance;
    private BoardManager manager = BoardLogic.boardLogicInstance.manager;
    private int[,] koTokens;

    private List<int[,]> koMemory = new List<int[,]>();
    public bool koFlag { get; set; }

    public void Checking(Token token)
    {
        koFlag = false;
        baseToken = token;
        referenceToken = token;
        checkList = new List<HashSet<Vector3>>();
        CheckingAllTokensSameColor();
    }

    public void KoChecking()
    {
        KoHandle();
        Remember();
    }
    private void CheckingAllTokensSameColor()
    {
        for (int i = 0; i < logic.boardsize + 1; i++)
        {
            for (int j = 0; j < logic.boardsize + 1; j++)
            {
                if (manager.Tokens[i, j] != null)
                {
                    if (manager.Tokens[i, j].isBlack == referenceToken.isBlack)
                    {
                        baseToken = manager.Tokens[i, j];
                        Neighbours(baseToken);
                        if (checkList.Count != 0)
                        {
                            Debug.Log(checkList);
                            DestroyingTokens();
                        }
                    }
                }
            }
        }
    }
    private void Remember()
    {
        TokensClone();
        koMemory.Add(koTokens);
    }
    private void TokensClone()
    {
        koTokens = new int[logic.boardsize + 1, logic.boardsize + 1];
        for (int i = 0; i < logic.boardsize + 1; i++)
        {
            for (int j = 0; j < logic.boardsize + 1; j++)
            {
                if (manager.Tokens[i, j] != null)
                {
                    if (manager.Tokens[i, j].isBlack)
                    {
                        koTokens[i, j] = 0;
                    }
                    else
                    {
                        koTokens[i, j] = 1;
                    }
                }
                else
                {
                    koTokens[i, j] = -1;
                }
            }

        }
    }
    private void KoHandle()
    {
        if (KoCheck())
        {
            KOBoardCleaning();
            KOBoardRespawn();
            koFlag = true;
            Debug.Log("KOOO");
        }
    }

    private bool KoCheck()
    {
        foreach (int[,] boardplace in koMemory)
        {
            if (TokenArrayEqualityCheck(manager.Tokens, boardplace))
            {
                return true;
            }
        }
        return false;
    }

    private bool TokenArrayEqualityCheck(Token[,] array1, int[,] array2)
    {
        for (int i = 0; i < logic.boardsize + 1; i++)
        {
            for (int j = 0; j < logic.boardsize + 1; j++)
            {
                if (array1[i, j] == null)
                {
                    if (array2[i, j] != -1)
                    {
                        return false;
                    }
                }
                else if ((array1[i, j].isBlack && array2[i, j] != 0) ||
               (!array1[i, j].isBlack && array2[i, j] != 1))
                {
                    return false;
                }


            }
        }
        return true;
    }
    private void KOBoardCleaning()
    {
        for (int i = 0; i < logic.boardsize + 1; i++)
        {
            for (int j = 0; j < logic.boardsize + 1; j++)
            {
                if (manager.Tokens[i, j] != null)
                {
                    logic.RemoveToken(i, j);
                }

            }
        }
    }
    private void KOBoardRespawn()
    {
        for (int i = 0; i < logic.boardsize + 1; i++)
        {
            for (int j = 0; j < logic.boardsize + 1; j++)
            {
                if (koTokens[i, j] != -1)
                {
                    manager.SpawnToken(koTokens[i, j], i, j);
                }

            }
        }
    }

    private void DestroyingTokens()
    {
        foreach (HashSet<Vector3> eachSet in checkList)
        {
            foreach (Vector3 vector in eachSet)
            {
                logic.RemoveToken((int)vector.x, (int)vector.z);
            }
        }
    }
    public void Neighbours(Token token)
    {

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (token.CurrentX + i >= 0 && token.CurrentY + j >= 0 && token.CurrentX + i < logic.boardsize + 1 && token.CurrentY + j < logic.boardsize + 1)
                {
                    if (manager.Tokens[(int)token.CurrentX + i, (int)token.CurrentY + j] != null)
                    {
                        if (manager.Tokens[token.CurrentX + i, token.CurrentY + j].isBlack != token.isBlack)
                        {
                            HashSet<Vector3> set = new HashSet<Vector3>();
                            set.Add(new Vector3(token.CurrentX + i, 0, token.CurrentY + j));
                            checkList.Add(set);
                        }
                    }
                }
            }
        }
        ListCheck();
    }

    private void ListCheck()
    {
        for (int i = 0; i < 10; i++)
        {
            SetCheck();
        }
        for (int checkListCounter = 0; checkListCounter < checkList.Count; checkListCounter++)
        {

            int[] verticalFlag = { 0, 0 };
            int[] horizontalFlag = { 0, 0 };

            foreach (Vector3 vector in checkList[checkListCounter])
            {
                if (vector.z == 0)
                {
                    verticalFlag[0] = 1;
                }
                if (vector.z == logic.boardsize)
                {
                    verticalFlag[1] = 1;
                }
                if (vector.x == 0)
                {
                    horizontalFlag[0] = 1;
                }
                if (vector.x == logic.boardsize)
                {
                    horizontalFlag[1] = 1;
                }
            }
            if (verticalFlag[0] == 1 && verticalFlag[1] == 1 && horizontalFlag[0] == 1 && horizontalFlag[1] == 1)
            {
                checkList.Remove(checkList[checkListCounter]);
                checkListCounter--;
                continue;
            }
        }
    }
    public void SetCheck()
    {
        List<HashSet<Vector3>> copyList = new List<HashSet<Vector3>>();
        HashSet<Vector3> copyset;

        for (int checkListCounter = 0; checkListCounter < checkList.Count; checkListCounter++)
        {
            bool removingflag = false;

            copyset = new HashSet<Vector3>(checkList[checkListCounter]);
            foreach (Vector3 vector in checkList[checkListCounter])
            {
                //HORIZONTAL
                for (int i = -1; i < 2; i++)
                {
                    if (vector.x + i >= 0 && vector.x + i < logic.boardsize + 1)
                    {
                        if (manager.Tokens[(int)(vector.x + i), (int)vector.z] != null)
                        {
                            if (manager.Tokens[(int)vector.x + i, (int)vector.z].isBlack != baseToken.isBlack)
                            {
                                Vector3 vectorCheck = new Vector3(vector.x + i, 0, vector.z);
                                if (!checkList[checkListCounter].Contains(vectorCheck))
                                {
                                    copyset.Add(vectorCheck);
                                }

                            }
                        }
                        else
                        {
                            removingflag = true;
                        }
                    }
                }
                //VERTICAL

                for (int i = -1; i < 2; i++)
                {
                    if (vector.z + i >= 0 && (int)(vector.z + i) < logic.boardsize + 1)
                    {
                        if (manager.Tokens[(int)(vector.x), (int)(vector.z + i)] != null)
                        {
                            if (manager.Tokens[(int)vector.x, (int)(vector.z + i)].isBlack != baseToken.isBlack)
                            {
                                Vector3 vectorCheck = new Vector3(vector.x, 0, vector.z + i);
                                if (!checkList[checkListCounter].Contains(vectorCheck))
                                {
                                    copyset.Add(vectorCheck);
                                }
                            }
                        }
                        else
                        {
                            removingflag = true;
                        }
                    }
                }
            }
            if (!removingflag)
            {
                copyList.Add(copyset);
            }

        }
        checkList = copyList;
        setUnioning();
    }

    private void setUnioning()
    {
        HashSet<Vector3> copyset;
        for (int checkListCounter = 0; checkListCounter < checkList.Count; checkListCounter++)
        {
            copyset = new HashSet<Vector3>(checkList[checkListCounter]);
            if (checkListCounter + 1 < checkList.Count)
            {
                for (int insidecheckListCounter = checkListCounter + 1; checkListCounter < checkList.Count; checkListCounter++)
                {
                    copyset.IntersectWith(checkList[insidecheckListCounter]);
                    if (copyset.Count != 0)
                    {
                        checkList[checkListCounter].UnionWith(checkList[insidecheckListCounter]);
                        checkList.Remove(checkList[insidecheckListCounter]);
                    }
                }
            }
        }
    }

}
