using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FunkyCode;
public class LightControll : MonoBehaviour
{
    public GameObject light_Fore,light_TreeNear,light_Cha;
    public float waitTime_Cha = 0.1f, waitTime_TreeNear=0.4f, waitTime_Fore=0.4f;
    void Start()
    {
        StartCoroutine(enumerator(light_Cha, waitTime_Cha));
        StartCoroutine(enumerator(light_TreeNear, waitTime_TreeNear));
        StartCoroutine(enumerator(light_Fore, waitTime_Fore));
    }

    IEnumerator enumerator(GameObject light ,float duration)
    {
        LightSprite2D lightSprite2D = light.GetComponent<LightSprite2D>();

        Color originColor = lightSprite2D.color;

        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);

            lightSprite2D.color = Color.Lerp(originColor, new Color(0, 0, 0, 0), time / duration);
        }

        light_Cha.SetActive(false);
    }

}
