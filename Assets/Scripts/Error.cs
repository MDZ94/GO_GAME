using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Error : MonoBehaviour
{

    public GameObject ErrorMenu;
    public Button button1;
    public Button button2;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void block()
    {
        button1.interactable = false;
        button2.interactable = false;
    }
    public void unblock()
    {
        button1.interactable = true;
        button2.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (ErrorMenu.activeInHierarchy)
        {
            block();
            return;
        }
        else
        {
            unblock();
            return;
        }
    }
}
