using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TargetMove : MonoBehaviour
{
    public void SetPosition(Transform transform)
    {
        this.transform.position = transform.position;
    }

    public void WaitMove(Transform transform)
    {
        StartCoroutine(WaitThenMove(transform));
    }

    IEnumerator WaitThenMove(Transform transform)
    {
        Behaviour behaviour = FindObjectOfType<Behaviour>();

        while (Mathf.Abs(this.transform.position.x - behaviour.transform.position.x) > 1)
        {
            yield return new WaitForEndOfFrame();
        }

        SetPosition(transform);
    }
}
