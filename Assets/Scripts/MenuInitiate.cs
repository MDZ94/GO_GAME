using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInitiate : MonoBehaviour
{
    public GameObject PlayMenu;
    public GameObject OfflineMenu;
    public GameObject OnlineMenu;
    public GameObject OptionsMenu;
    public GameObject LoginHostMenu;
    public GameObject LoginJoinMenu;
    public GameObject HostMenu;
    public GameObject SearchMenu;
    public GameObject WaitHostMenu;
    public GameObject WaitJoinMenu;
    public GameObject ErrorMenu;
    public GameObject RegisterMenu;
    





    private void Start()
    {

        PlayMenu.SetActive(true);
        OfflineMenu.SetActive(false);
        OnlineMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        LoginHostMenu.SetActive(false);
        LoginJoinMenu.SetActive(false);
        HostMenu.SetActive(false);
        SearchMenu.SetActive(false);
        WaitHostMenu.SetActive(false);
        WaitJoinMenu.SetActive(false);
        ErrorMenu.SetActive(false);
        RegisterMenu.SetActive(false);
    }


    


}
