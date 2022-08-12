using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    
    void Start()
    {
        
    }
    private float x;
    private float y;
    private Vector3 rotateValue;

    void Update()
    {
        if (Input.GetKey("space")) {
            y = Input.GetAxis("Mouse X");
            x = Input.GetAxis("Mouse Y");
            Debug.Log(x + ":" + y);
            rotateValue = new Vector3(x, y * -1, 0);
            transform.eulerAngles = transform.eulerAngles - rotateValue;
            return;
        }

    }
}
