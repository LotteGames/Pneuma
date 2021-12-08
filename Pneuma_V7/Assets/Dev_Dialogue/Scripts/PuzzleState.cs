using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PuzzleState : MonoBehaviour
{
    public bool isCompelete = false;

    public int puzzleNum;

    public void SetCompelete(bool value) 
    {
        isCompelete = value;
    }

}

