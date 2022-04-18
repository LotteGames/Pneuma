using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FragFloat : MonoBehaviour
{
    public Vector3 startPos;
    public float floatTime = 0.3f, distance = 1.5f,stillTime=0;
    public Ease ease;
    private void Start()
    {
        startPos = transform.position;
        StartCoroutine(enumerator());
    }

    IEnumerator enumerator()
    {
        transform.DOMoveY(startPos.y + distance, floatTime).SetEase(ease);
        yield return new WaitForSeconds(floatTime);

        yield return new WaitForSeconds(stillTime);

        transform.DOMoveY(startPos.y - distance, floatTime).SetEase(ease);
        yield return new WaitForSeconds(floatTime);

        yield return new WaitForSeconds(stillTime);

        StartCoroutine(enumerator());
    }
}
