using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitRate : MonoBehaviour
{
    public ParticleSystem particle;

    public List<RateWaitTime> rateWaitTimes;
    [System.Serializable]
    public struct RateWaitTime
    {
        public float waitTime;

        public float rate;
    }
    void Start()
    {
        StartCoroutine(enumerator());
    }

    IEnumerator enumerator()
    {
        for (int i = 0; i < rateWaitTimes.Count; i++)
        {
            yield return new WaitForSeconds(rateWaitTimes[i].waitTime);
            var emission = particle.emission;
            emission.rateOverTime = rateWaitTimes[i].rate;
        }
    }
}
