using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ani_Contrl : MonoBehaviour
{
    public void OpenBool(string What)
    {
        GetComponent<Animator>().SetBool(What, true);
    }
    public void CloseBool(string What)
    {
        GetComponent<Animator>().SetBool(What, false);
    }
}
