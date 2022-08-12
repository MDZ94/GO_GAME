using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorHandler : MonoBehaviour
{
    public GameObject ErrorMenu;
    public Text ErrorDescText;


    void Start()
    {
        
    }

    public void ErrorShow(string errorMSG)
    {
        ErrorDescText.text = errorMSG;
        ErrorMenu.SetActive(true);
    }

}
