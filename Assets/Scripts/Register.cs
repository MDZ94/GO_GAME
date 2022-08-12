using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Assets.Scripts.WebApi;

public class Register : MonoBehaviour
{
    public InputField PassInput;
    public InputField PassRepeatInput;
    public InputField LoginInput;
    public InputField Username;
    public GameObject ErrorMenu;
    public GameObject NextMenu;
    public GameObject CurrentMenu;
    private ErrorHandler errorHandler;

    private WebApiManager apiManager;
    private static CancellationTokenSource cts = new CancellationTokenSource();

    string password;
    string passwordRepeat;
    string login;
    string username;

    void Start()
    {
        apiManager = FindObjectOfType<WebApiManager>();
        errorHandler = FindObjectOfType<ErrorHandler>();
    }

    public async void Registration()
    {
        login = LoginInput.text;
        password = PassInput.text;
        username = Username.text;
        passwordRepeat = PassRepeatInput.text;

        if (password == passwordRepeat)
        {
            try
            {
                await apiManager.RegisterAsync(login, password, username, cts.Token);
                login = "";
                password = "";
                username = "";
                passwordRepeat = "";
                NextMenu.SetActive(true);
                CurrentMenu.SetActive(false);
            }
            catch (System.Exception em)
            {
                errorHandler.ErrorShow(em.Message);
            }


        }
        else
        {
            errorHandler.ErrorShow("Podane has³a ró¿ni¹ siê");
        }
        
    }
}
