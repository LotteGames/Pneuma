using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Ai_SetState : MonoBehaviour
{
    public AiBehaviour aiBehaviour;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ai_AreaState ai_AreaState = collision.GetComponent<Ai_AreaState>();
        if (ai_AreaState != null)
        {
            aiBehaviour.SetState(ai_AreaState.state);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Ai_AreaState ai_AreaState = collision.GetComponent<Ai_AreaState>();
        if (ai_AreaState != null)
        {
            aiBehaviour.SetState(eState_AI.Idle);
        }
    }
}
