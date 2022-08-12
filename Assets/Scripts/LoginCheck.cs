using Assets.Scripts.WebApi;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoginCheck : MonoBehaviour
{
    string login;
    string password;
    public InputField LoginInput;
    public InputField PassInput;
    public GameObject NextMenu;
    public GameObject CurrentMenu;
    public GameObject ErrorMenu;

    private static CancellationTokenSource cts = new CancellationTokenSource();
    private WebApiManager apiManager;
    private ErrorHandler errorHandler;

    void Start()
    {
        apiManager = FindObjectOfType<WebApiManager>();
        errorHandler = FindObjectOfType<ErrorHandler>();
    }

    public async void Login() {
        login = LoginInput.text;
        password = PassInput.text;
        try {
            await apiManager.LoginAsync(login, password, cts.Token);
            NextMenu.SetActive(true);
            CurrentMenu.SetActive(false);
        }
        catch (System.Exception em) {
            errorHandler.ErrorShow(em.Message);
        }
    }
    
    //public void test()
    //{
    //    login = LoginInput.text;
    //    password = PassInput.text;
    //    if (login == "" & password == "")
    //    {
    //        NextMenu.SetActive(true);
    //        CurrentMenu.SetActive(false);
    //    }
    //    else
    //    {
    //        ErrorMenu.SetActive(true);
    //    }
    //    return;

    //}

}
