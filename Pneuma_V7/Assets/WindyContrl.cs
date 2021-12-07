using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindyContrl : MonoBehaviour
{
    [Header("風的物件")]
    public GameObject WindCube;
    [Header("風的粒子特效")]
    public GameObject WindAni;
    [Header("風關閉的時間有多久")]
    public float WindCloseTime;
    [Header("風開啟的時間有多久")]
    public float WindOpenTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Open());
    }

    public IEnumerator Open()
    {
        WindAni.GetComponent<ParticleSystem>().loop = true;
        WindAni.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1.2f);
        WindCube.SetActive(true);
        yield return new WaitForSeconds(WindOpenTime);
        StartCoroutine(Close());
    }
    public IEnumerator Close()
    {
        WindAni.GetComponent<ParticleSystem>().loop = false;
        yield return new WaitForSeconds(0.8f);
        WindCube.SetActive(false);
        yield return new WaitForSeconds(WindCloseTime);
        StartCoroutine(Open());
    }
}
