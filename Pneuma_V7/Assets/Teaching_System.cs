using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teaching_System : MonoBehaviour
{
    [Header("貓咪")]
    public GameObject Cat;

    [Header("兩個眼睛")]
    public GameObject[] eyes;

    [Header("最後眼睛的顏色")]
    public Color[] OverEyesColor;


    [Header("每個階段的教學")]
    public GameObject[] AllTeachUI;

    [Header("每個階段的眼睛顏色")]
    public Color[] Eyes_Colors;

    public int Level;
    public bool Look;
    // Start is called before the first frame update
    void Start()
    {
        Cat = GameObject.FindObjectOfType<CatContrl>().gameObject;
        Look = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Level < Eyes_Colors.Length)
        {
            for (int i = 0; i < eyes.Length; i++)
            {
                eyes[i].GetComponent<SpriteRenderer>().color = Color.Lerp(eyes[i].GetComponent<SpriteRenderer>().color, Eyes_Colors[Level], 0.06f);
            }
        }
        else
        {
            for (int i = 0; i < eyes.Length; i++)
            {
                eyes[i].GetComponent<SpriteRenderer>().color = Color.Lerp(eyes[i].GetComponent<SpriteRenderer>().color, OverEyesColor[i], 0.06f);
            }
        }

        if (Level < AllTeachUI.Length)
        {
            for (int i = 0; i < eyes.Length; i++)
            {
                if(Level == i)
                {
                    AllTeachUI[i].SetActive(true);
                }
                else
                {
                    AllTeachUI[i].SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < eyes.Length; i++)
            {
                AllTeachUI[i].SetActive(false);
            }
        }

    }

    public void NextLevel()
    {
        Level++;
        //if (Level >= Eyes_Colors.Length)
        //{
        //    for (int i = 0; i < eyes.Length; i++)
        //    {
        //        eyes[i].SetActive(false);
        //    }
        //    for (int i = 0; i < AllEyes.Length; i++)
        //    {
        //        AllEyes[i].SetActive(true);
        //    }
        //}
        StartCoroutine(Delay());
        for (int i = 0; i < eyes.Length; i++)
        {
            eyes[i].GetComponent<Animator>().Play("Base Layer.CatGodEyes");
        }
    }

    public IEnumerator Delay()
    {
        Cat.GetComponent<CatContrl>().NowCatAct = CatContrl.CatAct.CatStop;
        Cat.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(1);
        Cat.GetComponent<CatContrl>().NowCatAct = CatContrl.CatAct.Idle;
    }
}
