using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAiFollow : MonoBehaviour
{
public  Behaviour aiBehaviour;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FollowArea followArea = collision.GetComponent<FollowArea>();
        if (followArea != null)
        {
            aiBehaviour.SetIsFollow(followArea.isFollow);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FollowArea followArea = collision.GetComponent<FollowArea>();
        if (followArea != null)
        {
            aiBehaviour.SetIsFollow(false); ;
        }
    }
}
