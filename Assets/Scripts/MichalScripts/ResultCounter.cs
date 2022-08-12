using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ResultCounter
{
    public int blackResult { get; set; }
    public int redResult { get; set; }
    public int komi { get; set; }
    public BoardManager manager = BoardManager.Instance;
    private BoardLogic logic = BoardLogic.boardLogicInstance;
    private List<HashSet<Vector3>> checkList;
    Token baseToken;
    private int[,] resultTokens;
    void Start()
    {
        blackResult = komi;
        redResult = 0;
    }


    public void ActualResultCheck()
    {
        TokensClone();
        CheckListReset();
        ColorClean(true);
        DestroyingTokens();
        CheckListReset();
        ColorClean(false);
        DestroyingTokens();
        CheckListReset();
        ColorScoreCheck(true);
        Debug.Log("Black");
        Debug.Log(CountResult());
        CheckListReset();
        ColorScoreCheck(false);
        Debug.Log("Red");
        Debug.Log(CountResult());
        CheckListReset();
        BoardRespawn();

    }
    private void ColorClean(bool color)
    {
        TokenSearch(color);
        for (int i = 0; i < manager.boardsize + 1; i++)
        {
            for (int j = 0; j < manager.boardsize + 1; j++)
            {
                if (manager.Tokens[i, j] != null)
                {
                    if (manager.Tokens[i, j].isBlack == color)
                    {
                        DeadNeighbours(manager.Tokens[i, j]);
                    }
                }
            }
        }
    }

    private void ColorScoreCheck(bool color)
    {
        TokenSearch(color);
        for (int i = 0; i < manager.boardsize + 1; i++)
        {
            for (int j = 0; j < manager.boardsize + 1; j++)
            {
                if (manager.Tokens[i, j] != null)
                {
                    if (manager.Tokens[i, j].isBlack == color)
                    {
                        BlankNeighbours(manager.Tokens[i, j]);
                    }
                }
            }
        }
    }
    private void CheckListReset()
    {
        checkList = new List<HashSet<Vector3>>();

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

    private void TokensClone()
    {
        resultTokens = new int[logic.boardsize + 1, logic.boardsize + 1];
        for (int i = 0; i < logic.boardsize + 1; i++)
        {
            for (int j = 0; j < logic.boardsize + 1; j++)
            {
                if (manager.Tokens[i, j] != null)
                {
                    if (manager.Tokens[i, j].isBlack)
                    {
                        resultTokens[i, j] = 0;
                    }
                    else
                    {
                        resultTokens[i, j] = 1;
                    }
                }
                else
                {
                    resultTokens[i, j] = -1;
                }
            }

        }
    }
    private void BoardClean()
    {
        for (int i = 0; i < logic.boardsize + 1; i++)
        {
            for (int j = 0; j < logic.boardsize + 1; j++)
            {
                logic.RemoveToken(i, j);

            }
        }
    }
    private void BoardRespawn()
    {
        BoardClean();
        for (int i = 0; i < logic.boardsize + 1; i++)
        {
            for (int j = 0; j < logic.boardsize + 1; j++)
            {
                if (resultTokens[i, j] != -1)
                {
                    manager.SpawnToken(resultTokens[i, j], i, j);
                }

            }
        }
    }

    private void TokenSearch(bool color)
    {

        for (int i = 0; i < manager.boardsize + 1; i++)
        {
            for (int j = 0; j < manager.boardsize + 1; j++)
            {
                if (manager.Tokens[i, j] != null)
                {
                    if (manager.Tokens[i, j].isBlack == color)
                    {
                        baseToken = manager.Tokens[i, j];
                        return;
                    }
                }
            }
        }

    }
    private void DeadNeighbours(Token token)
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
                    else
                    {
                        HashSet<Vector3> set = new HashSet<Vector3>();
                        set.Add(new Vector3(token.CurrentX + i, 0, token.CurrentY + j));
                        checkList.Add(set);
                    }
                }
            }
        }
        ListCheck();
    }

    private void BlankNeighbours(Token token)
    {

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (token.CurrentX + i >= 0 && token.CurrentY + j >= 0 && token.CurrentX + i < logic.boardsize + 1 && token.CurrentY + j < logic.boardsize + 1)
                {
                    if (manager.Tokens[(int)token.CurrentX + i, (int)token.CurrentY + j] == null)
                    {
                        HashSet<Vector3> set = new HashSet<Vector3>();
                        set.Add(new Vector3(token.CurrentX + i, 0, token.CurrentY + j));
                        checkList.Add(set);
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
            //Debug.Log("Out");
            if (verticalFlag[0] == 1 && verticalFlag[1] == 1 && horizontalFlag[0] == 1 && horizontalFlag[1] == 1)
            {
                checkList.Remove(checkList[checkListCounter]);
                checkListCounter--;
                //Debug.Log("Listcheck deleting!");
                continue;
            }

        }
        DeleteListDuplicates();
        CrossBoardCheck();
    }
    private void CrossBoardCheck()
    {
        for (int checkListCounter = 0; checkListCounter < checkList.Count; checkListCounter++)
        {

            if (EdgeCheck(checkList[checkListCounter]) == 3)
            {
                for (int InnercheckListCounter = checkListCounter + 1; InnercheckListCounter < checkList.Count; InnercheckListCounter++)
                {


                    if (EdgeCheck(checkList[InnercheckListCounter]) == 3)
                    {
                        if (checkList[InnercheckListCounter].Count > checkList[checkListCounter].Count)
                        {
                            checkList.Remove(checkList[InnercheckListCounter]);
                        }
                        else
                        {
                            checkList.Remove(checkList[checkListCounter]);
                        }

                    }
                }
            }


        }
    }
    private int EdgeCheck(HashSet<Vector3> set)
    {
        int edges = 0;
        int[] verticalFlag = { 0, 0 };
        int[] horizontalFlag = { 0, 0 };

        foreach (Vector3 vector in set)
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
        for (int i = 0; i < 2; i++)
        {
            edges = edges + horizontalFlag[i] + verticalFlag[i];
        }
        return edges;
    }
    private void DeleteListDuplicates()
    {
        HashSet<Vector3> copyset;
        for (int checkListCounter = 0; checkListCounter < checkList.Count - 1; checkListCounter++)
        {

            foreach (Vector3 vector in checkList[checkListCounter])
            {
                for (int InnercheckListCounter = checkListCounter + 1; InnercheckListCounter < checkList.Count; InnercheckListCounter++)
                {
                    copyset = new HashSet<Vector3>(checkList[checkListCounter]);
                    copyset.IntersectWith(checkList[InnercheckListCounter]);
                    if (copyset.Count != 0)
                    {
                        checkList.Remove(checkList[InnercheckListCounter]);
                    }
                }
            }

        }
    }
    private void SetCheck()
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
                            Vector3 vectorCheck = new Vector3(vector.x + i, 0, vector.z);
                            if (!checkList[checkListCounter].Contains(vectorCheck))
                            {
                                copyset.Add(vectorCheck);
                            }

                        }

                    }
                }
                //VERTICAL

                for (int i = -1; i < 2; i++)
                {
                    if (vector.z + i >= 0 && (int)(vector.z + i) < logic.boardsize + 1)
                    {
                        //Debug.Log((int)vector.z);
                        //Debug.Log("Bounds:"+(int)vector.x+"bound:"+(int)(vector.z+i));
                        //Debug.Log("Token:"+manager.Tokens.Length+"token:");
                        if (manager.Tokens[(int)vector.x, (int)(vector.z + i)] != null)
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
                            Vector3 vectorCheck = new Vector3(vector.x, 0, vector.z + i);
                            if (!checkList[checkListCounter].Contains(vectorCheck))
                            {
                                copyset.Add(vectorCheck);
                            }
                        }

                    }
                }

                /*for(int i = -1; i<2 ;i++){
                    for(int j = -1; j<2 ;j++){
                        if(vector.x+i>=0&&vector.z+j>=0&&vector.x+i<=manager.boardsize&&vector.z+j<=manager.boardsize){
                            if(manager.Tokens[(int)vector.x+i,(int)vector.z+j].isBlack!=baseToken.isBlack){
                                eachSet.Add(new Vector3(token.CurrentX+i,0,token.CurrentY+j));
                                checkList.Add(set);
                            }
                        }
                    }
                }*/
            }
            if (!removingflag)
            {
                copyList.Add(copyset);
            }

        }
        checkList = copyList;
        setUnioning();
    }
    private void BlankSetCheck()
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

                        if (manager.Tokens[(int)(vector.x + i), (int)vector.z] == null)
                        {
                            Vector3 vectorCheck = new Vector3(vector.x + i, 0, vector.z);
                            if (!checkList[checkListCounter].Contains(vectorCheck))
                            {
                                copyset.Add(vectorCheck);
                            }

                        }

                    }
                }
                //VERTICAL

                for (int i = -1; i < 2; i++)
                {
                    if (vector.z + i >= 0 && (int)(vector.z + i) < logic.boardsize + 1)
                    {
                        if (manager.Tokens[(int)(vector.x + i), (int)vector.z] == null)
                        {
                            Vector3 vectorCheck = new Vector3(vector.x, 0, vector.z + i);
                            if (!checkList[checkListCounter].Contains(vectorCheck))
                            {
                                copyset.Add(vectorCheck);
                            }
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
    private int CountResult()
    {
        setUnioning();
        int res = 0;
        foreach (HashSet<Vector3> set in checkList)
        {
            res = res + set.Count;
        }
        return res;
    }

}

