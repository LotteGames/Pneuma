using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NumberCounter : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public int countFps = 30;
    public float duration = 1f;
    private int value;

    public int Value
    {
        get
        {
            return this.value;
        }
        set
        {
            UpdateText(value);
            this.value = value;
        }
    }

    public void UpdateText(int newValue)
    {
        if (CountingCoroutine != null)
        {
            StopCoroutine(CountingCoroutine);
        }

        CountingCoroutine = StartCoroutine(CountText(newValue));
    }

    private IEnumerator CountText(int newValue)
    {
        WaitForSeconds wait = new WaitForSeconds(1f/countFps * duration);
        int previousValue = value;
        int stepAmount  = Mathf.CeilToInt((newValue - previousValue) / (countFps * duration));

        while (previousValue != newValue)
        {
            previousValue += stepAmount; // (-20 - 0) / (30 * 1) = -0.66667 -> -1              0 + -1 = -1

            Text.SetText(previousValue.ToString());

            yield return wait;
        }
    }

    private Coroutine CountingCoroutine;

}
