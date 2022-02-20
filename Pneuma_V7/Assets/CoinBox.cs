using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBox : MonoBehaviour
{

    [Header("要生成的金幣")]
    public GameObject Coin;
    [Header("金幣儲存了")]
    public bool CoinSave;

    public void SetStart()
    {
        if (transform.GetChild(0).gameObject.active == false && CoinSave == false)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
