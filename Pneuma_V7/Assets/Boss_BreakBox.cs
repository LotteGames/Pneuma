using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_BreakBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    public void SetStart()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        
    }
}
