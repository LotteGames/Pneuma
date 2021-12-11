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

    public GameObject startTalkBtn ;

    private void Update()
    {
        if (IsTalkable && IsFin)
        {
            startTalkBtn.SetActive(true);

            if (Input.GetKeyDown(KeyCode.C))
            {
                startTalkBtn.SetActive(false);
                FindObjectOfType<DialogueManager>().InvokeEvent_Talk();
            }
        }
        else { startTalkBtn.SetActive(false); }
    }

    public bool IsFin = true;

    public void SetIsFin(bool value)
    {
        Debug.LogError("IsFin");
        IsFin = value;
    }


}
