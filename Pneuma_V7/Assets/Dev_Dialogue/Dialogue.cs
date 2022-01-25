using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    public bool isCompelete = false;

    public bool directTrigger = false;

    public List<Condition> taskConditions;

    [Space(15)]
    public List<Sentence> sentences;

    public UnityEvent finishEvent;

    public void InvokeFinishEvent() 
    {
        finishEvent.Invoke();
    }

}
