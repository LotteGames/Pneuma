using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShake : MonoBehaviour
{

    public CinemachineBasicMultiChannelPerlin channelPerlin;

    private void Awake()
    {
        channelPerlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float amplitude, float frequency)
    {
        channelPerlin.m_AmplitudeGain = amplitude;
        channelPerlin.m_FrequencyGain = frequency;
    }
    public void Shake(float amplitude, float frequency, float time)
    {
        if (waitRoutine != null)
        {
            StopCoroutine(waitRoutine);
        }
        waitRoutine = StartCoroutine(Wait(amplitude, frequency, time));
    }
    Coroutine waitRoutine;
    IEnumerator Wait(float amplitude, float frequency, float time)
    {
        channelPerlin.m_AmplitudeGain = amplitude;
        channelPerlin.m_FrequencyGain = frequency;

        yield return new WaitForSeconds(time);

        channelPerlin.m_AmplitudeGain = 0;
        channelPerlin.m_FrequencyGain = 0;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            Shake(1, 1, 1);
        }
    }

}
