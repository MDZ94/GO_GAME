using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public abstract class Token : MonoBehaviour, IToken
{
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }
    public bool isBlack;
    public bool isBlank;

    public bool BlackCheck(){
        return isBlack;
    }

    public void SetPosition(int x, int y)
    {

        CurrentX = x;
        CurrentY = y;

    }

}
