using System;
using System.Collections;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

public class JumpState : BaseState
{
    // public IEnumerator Name()
    // {
    //     yield return new WaitForSeconds(5f);
    //     _onStateCompleted();
    // } 
    public override void StateEnter(Action onStateCompleted)
    {
        _onStateCompleted = onStateCompleted;
        Debug.Log("Entered Jump State");
        // StartCoroutine(Name());
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpdate(float deltaTime)
    {

    }
}
