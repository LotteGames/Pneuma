using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CounterUpdater : MonoBehaviour
{
    public NumberCounter NumberCounter;

    public TimerCounter timerCounter;

    public ResultTitle resultTitle;
    //public int value;

    //private void OnValidate()
    //{
    //    SetValue(value);
    //}

    public void SetValue(int value)
    {
        NumberCounter.Value = value;
    }

    public void ShowResult() 
    {
        StartCoroutine(ResultPresent());
    }


    IEnumerator ResultPresent()
    {
        resultTitle.ShowTitle_1();
        yield return new WaitForSeconds(resultTitle.duration_1);

        timerCounter.UpdateText();
        yield return new WaitForSeconds(timerCounter.duration);

        NumberCounter.UpdateText(12);
        yield return new WaitForSeconds(NumberCounter.duration);

        resultTitle.SetBtnActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CatContrl>())
        {
            ResultCanvas.GetComponent<Canvas>().enabled=(true);
            ShowResult();
        }
    }

    public GameObject ResultCanvas;
}
