using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PuzzleState : MonoBehaviour
{
    public bool isCompelete = false;

    public int puzzleNum;

    public void SetCompelete(bool value)
    {
        isCompelete = value;
    }

    public UnityEvent finishEvent;

    public void InvokeEvent()
    {
        finishEvent.Invoke();
    }

    public void SetPuzzleNum(int value)
    {
        puzzleNum = value;
    }
}

