using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cleaner : MonoBehaviour
{
    string clear;
    public InputField LoginInput;
    public InputField PassInput;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Clean()
    {
        clear = "";
        LoginInput.text = clear;
        PassInput.text = clear;
        return;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
