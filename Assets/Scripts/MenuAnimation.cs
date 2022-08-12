using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimation : MonoBehaviour
{
    public GameObject panel;
    
    public void Animate()
    {

        Animator animator = panel.GetComponent<Animator>();
        if(animator != null)
        {
            animator.Play("Base Layer.CloseMenu",0,0);
            //menu.SetActive(false);
        }               
           
        
    }

    public void Animate2()
    {
        Animator animator = panel.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("Base Layer.CloseMenu 1", 0, 0);
            //menu.SetActive(false);
        }
    }

    public void Animate3()
    {
        Animator animator = panel.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("Base Layer.CloseMenu 2", 0, 0);
            //menu.SetActive(false);
        }
    }
}
