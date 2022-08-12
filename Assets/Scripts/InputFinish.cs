using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFinish : MonoBehaviour
{
    public Button button;
   
    public void Finish()
    {
        button.onClick.Invoke();
    }
}
