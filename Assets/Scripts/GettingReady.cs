using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GettingReady : MonoBehaviour
{
    public GameObject Player1Ready;
    public GameObject Player2Ready;
    public Button myButton;
    public Button rdyButton;
    private Text txt;

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //PrevMenu.SetActive(false);
        //NextMenu.SetActive(true);
        myButton.enabled = false;
        txt = rdyButton.GetComponentInChildren<Text>();
        Player1Ready.SetActive(false);
        Player2Ready.SetActive(false);
        rdyButton.onClick.AddListener(Show);
    }


    void Show()
    {
        bool btnStatus = false;
        if (!myButton.enabled)
        {
            btnStatus = true;
        }
        myButton.enabled = btnStatus;
        Player1Ready.SetActive(btnStatus);
        return;
    }


}