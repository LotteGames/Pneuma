using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBehav : MonoBehaviour
{
    public Transform playerPos;

    public bool IsTalkable
    {
        get
        {
            float dist = (playerPos.position - transform.position).magnitude;

            return dist < 1.5f ? true : false;
        }
    }

    public GameObject talkBtn;

    private void Update()
    {
        if (IsTalkable && IsFin)
        {
            talkBtn.SetActive(true);

            if (Input.GetKeyDown(KeyCode.C))
            {
                talkBtn.SetActive(false);
                FindObjectOfType<DialogueManager>().InvokeEvent_Talk();
            }
        }
        else { talkBtn.SetActive(false); }
    }

    public bool IsFin = true;

    public void SetIsFin(bool value)
    {
        IsFin = value;
    }
}
