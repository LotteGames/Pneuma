using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerCounter : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public int countFps = 30;
    public float duration = 1f;



    public bool isRunning = true;

    public float timeSinceLoad = 0;

    /// <summary>
    /// 暫停或遊戲結束時時就執行這個
    /// </summary>
    public void PauseTimer()
    {
        isRunning = false;
    }
    public void ResumeTimer()
    {
        isRunning = true;
    }
    public void Counting()
    {
        if (isRunning)
        {
            timeSinceLoad += Time.deltaTime;
        }
    }
    private void Update()
    {
        Counting();
    }


    public void UpdateText()
    {
        if (CountingCoroutine != null)
        {
            StopCoroutine(CountingCoroutine);
        }

        CountingCoroutine = StartCoroutine(CountText());
    }
    public float previousValue = 0;
    private IEnumerator CountText()
    {
        WaitForSeconds wait = new WaitForSeconds(1f / countFps);
        previousValue = 0;
        float stepAmount = (timeSinceLoad - previousValue) / (countFps * duration);

        Debug.Log((int)(countFps * duration));

        int i = 0;
        while ((int)(countFps * duration) != i)//Mathf.FloorToInt(previousValue) != Mathf.FloorToInt(timeSinceLoad))
        {
            previousValue += stepAmount; // (-20 - 0) / (30 * 1) = -0.66667 -> -1              0 + -1 = -1

            int sec = (int)previousValue % 60, min = (int)previousValue / 60;


            Text.SetText(((min < 10) ? "0" : "") + min.ToString() + " : " + ((sec < 10) ? "0" : "") + sec.ToString());

            i++;
            yield return wait;
        }
    }

    private Coroutine CountingCoroutine;


}
