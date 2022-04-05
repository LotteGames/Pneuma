using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : MonoBehaviour
{
    [Header("音效盒")]
    public GameObject MusicBox;
    [Header("沖出時的粒子特效")]
    public GameObject RockAni;
    [Header("粒子特效出現的點")]
    public GameObject Pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MusicStart()
    {
        MusicBox.GetComponent<AudioSource>().Play();
        GameObject Ani = Instantiate(RockAni, Pos.transform.position, Quaternion.Euler(-90, 0, 0));
        Ani.GetComponent<AudioSource>().enabled = true;
        Destroy(Ani, 5);
    }
    public void MusicStart_No()
    {
        GameObject Ani = Instantiate(RockAni, Pos.transform.position, Quaternion.Euler(-90, 0, 0));
        Destroy(Ani, 5);
    }
    public void MusicStart_NoAni()
    {
        MusicBox.GetComponent<AudioSource>().Play();
    }

    public void MusicOver()
    {
        MusicBox.GetComponent<AudioSource>().Stop();
    }
}
