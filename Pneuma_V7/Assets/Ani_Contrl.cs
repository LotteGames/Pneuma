using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ani_Contrl : MonoBehaviour
{
    public GameObject OpenGuy;
    public void OpenBool(string What)
    {
        GetComponent<Animator>().SetBool(What, true);
    }
    public void CloseBool(string What)
    {
        GetComponent<Animator>().SetBool(What, false);
    }

    public void Des()
    {
        Destroy(gameObject);
    }
    public void CatStop()
    {
        GameObject.FindObjectOfType<CatContrl>().NowCatAct = CatContrl.CatAct.CatStop;
    }
    public void CatCanMove()
    {
        GameObject.FindObjectOfType<CatContrl>().NowCatAct = CatContrl.CatAct.Idle;
    }
    public void OpenObject()
    {
        OpenGuy.SetActive(true);
    }
}
