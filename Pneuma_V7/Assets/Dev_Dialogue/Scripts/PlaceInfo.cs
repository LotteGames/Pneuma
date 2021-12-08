using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceInfo : MonoBehaviour
{
    public int puzzleNum = -1;
    public PuzzleState puzzleState;

    public void SetPuzzleNum(int value)
    {
        puzzleNum = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PuzzleRange"))
        {
            Debug.LogError("Execute");
            puzzleState = collision.GetComponentInParent<PuzzleState>();
            puzzleNum = puzzleState.puzzleNum;

            if (puzzleNum == -2)
            {
                if (puzzleState.isCompelete == false)
                {
                    FindObjectOfType<DialogueManager>().InvokeEvent_Talk();

                    puzzleState.SetCompelete(true);
                    puzzleNum = -1;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PuzzleRange"))
        {
            puzzleState = null;
            puzzleNum = -1;
        }
    }
}
