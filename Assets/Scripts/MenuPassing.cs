using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPassing : MonoBehaviour
{
    public GameObject CurMenu;
    public GameObject NextMenu1;
    public GameObject NextMenu2;
    public GameObject NextMenu3;
    Canvas canvas;
    Animator animator;
    public GameObject panel;


    
    public void MoveOn1()
    {

        canvas = CurMenu.GetComponent<Canvas>();
        canvas.sortingOrder = 0;
        NextMenu1.SetActive(true);
        Animator animator = panel.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("Base Layer.CleanUp", 0, 0);
            //menu.SetActive(false);
        }

    }
    public void MoveOn2()
    {

        canvas = CurMenu.GetComponent<Canvas>();
        canvas.sortingOrder = 0;
        NextMenu2.SetActive(true);
        Animator animator = panel.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("Base Layer.CleanUp", 0, 0);
            //menu.SetActive(false);
        }

    }

    public void MoveOn3()
    {

        canvas = CurMenu.GetComponent<Canvas>();
        canvas.sortingOrder = 0;
        NextMenu3.SetActive(true);
        Animator animator = panel.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("Base Layer.CleanUp", 0, 0);
            //menu.SetActive(false);
        }

    }


}
