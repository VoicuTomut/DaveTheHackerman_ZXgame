using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public float OnInit()
    {
        animator.Play("LevelStartTransition");
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
    public float OnStart()
    {
        animator.Play("BeginLevelTransition");
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
    public float OnClear()
    {
        animator.Play("LevelClearTransition");
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
    public float OnSwitch()
    {
        animator.Play("SwitchLevelTransition");
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
    
    void OnEnable()
    {
        if(!animator)
        {
            animator = GetComponent<Animator>();
        }
    }
 
   
}
