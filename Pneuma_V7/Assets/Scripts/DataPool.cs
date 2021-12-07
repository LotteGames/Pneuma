using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPool : MonoBehaviour
{
    public static Dialogue_Ch1 dialogue_Ch1;
    public static void InitData() 
    {
        dialogue_Ch1 = Resources.Load<Dialogue_Ch1>("Dialogue_Ch1");
    }
}
