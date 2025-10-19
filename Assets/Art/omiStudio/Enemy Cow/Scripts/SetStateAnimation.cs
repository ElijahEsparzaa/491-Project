using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetStateAnimation : MonoBehaviour
{
    public GameObject cow1, cow2;
    public Button btn1;

    public void setIdle()
    {
        cow1.GetComponentInChildren<Animator>().SetInteger("State", 0);
        cow2.GetComponentInChildren<Animator>().SetInteger("State", 0);
    }

    public void setWalk()
    {
        cow1.GetComponentInChildren<Animator>().SetInteger("State", 1);
        cow2.GetComponentInChildren<Animator>().SetInteger("State", 1);
    }

    public void setRun()
    {
        cow1.GetComponentInChildren<Animator>().SetInteger("State", 2);
        cow2.GetComponentInChildren<Animator>().SetInteger("State", 2);
    }

    void Start()
    {
        //optional: set default state here if needed
        //setIdle();
    }

    void Update()
    {
        //optional: logic to auto-trigger based on distance or input
    }
}