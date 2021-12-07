using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAni : MonoBehaviour
{
    [Header("要生成的動畫")]
    public GameObject AniObject;

    public void Create()
    {
        Instantiate(AniObject);
    }
    public void Create(Vector3 CreatePos)
    {
        Instantiate(AniObject, CreatePos, Quaternion.Euler(0, 0, 0));
    }
    public void Create(Vector3 CreatePos,float DestroyTime)
    {
        GameObject A = Instantiate(AniObject, CreatePos, Quaternion.Euler(0, 0, 0));
        Destroy(A, DestroyTime);
    }
}
