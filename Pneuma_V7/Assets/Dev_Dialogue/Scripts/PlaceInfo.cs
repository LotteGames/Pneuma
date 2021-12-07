using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceInfo : MonoBehaviour
{
    public int puzzleNum = -1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PuzzleRange"))
        {
            PuzzleState puzzleState = collision.GetComponentInParent<PuzzleState>();
            puzzleNum = puzzleState.puzzleNum;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PuzzleRange"))
        {
            puzzleNum = -1;
        }
    }
}
