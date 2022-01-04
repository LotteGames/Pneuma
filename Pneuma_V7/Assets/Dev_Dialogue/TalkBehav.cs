using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The TalkBehav for NPCs
/// </summary>
public class TalkBehav : MonoBehaviour
{
    public Transform playerPos;

    public bool IsTalkable
    {
        get
        {
            float dist = (playerPos.position - transform.position).magnitude;
           
            return dist < 4.5f ? true : false;
        }
    }
    public bool IsDirectTalkable
    {
        get
        {
            float dist = (playerPos.position - transform.position).magnitude;

            return dist < 8.5f ? true : false;
        }
    }

    public GameObject startTalkBtn;
    public DialoguesManager dialoguesManager;
    public SpriteRenderer spR;
    private void Start()
    {
        if (dialoguesManager == null)
        {
            dialoguesManager = FindObjectOfType<DialoguesManager>();
        }

        if (playerPos == null)
        {
            playerPos = FindObjectOfType<CatContrl>().transform;
        }
    }
    private void Update()
    {
        if (IsDirectTalkable && !IsTalkable)
        {
            startTalkBtn.SetActive(false);
            dialoguesManager.StartDialogue();
        }
        else if (IsTalkable)
        {
            startTalkBtn.SetActive(dialoguesManager.isCompelete);
            dialoguesManager.StartDialogue();
        }
        else
        {
            startTalkBtn.SetActive(false);
        }


        if (dialoguesManager.SpeakingOne != null)
        {
            if (dialoguesManager.SpeakingOne.transform.position.x > transform.position.x)
            {
                spR.flipX = false;
            }
            else if (dialoguesManager.SpeakingOne.transform.position.x < transform.position.x)
            {
                spR.flipX = true;
            }
        }
    }


    public bool IsFin { get { return dialoguesManager.isCompelete; } }

    //public void SetIsFin(bool value)
    //{
    //    IsFin = value;
    //}
}
