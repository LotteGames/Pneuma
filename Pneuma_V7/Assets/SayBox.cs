using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SayBox : MonoBehaviour
{
    [System.Serializable]
    public class sayBox
    {
        [Header("說了甚麼")]
        [Multiline(3)]
        public string[] SayWhat;
        [Header("等待幾秒換下一句對話")]
        public float WaitTime;
    }

    [Header("死亡後說到哪ㄌ")]
    public int ToDieToWhere;
    [Header("死亡後說的話")]
    public sayBox[] ToDie;

    [Header("說了甚麼")]
    public List<string> SayWhat;
    [Header("等待幾秒換下一句對話")]
    public float WaitTime;
    float T;
    [Header("等待幾秒換下一句對話")]
    public Text SayText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        T += Time.deltaTime;
        if (SayWhat.Count > 0)
        {
            if(T >= WaitTime)
            {
                GetComponent<Animator>().SetBool("Open", false);
            }
        }
        else
        {
            if (T >= WaitTime)
            {
                GetComponent<Animator>().SetBool("Open", false);
            }
        }

    }

    public void NewText()
    {
        if (SayWhat.Count > 0)
        {
            GetComponent<Animator>().SetBool("Open", true);

            SayText.text = SayWhat[0];
            SayWhat.RemoveAt(0);
            T = 0;
        }
        else
        {

        }
    }

    public void CatSayNew(bool IsClear)
    {
        if(IsClear == true)
        {
            SayWhat.Clear();
        }

        for (int i = 0; i < ToDie[ToDieToWhere].SayWhat.Length; i++)
        {
            SayWhat.Add(ToDie[ToDieToWhere].SayWhat[i]);
        }

        ToDieToWhere++;
        if (ToDieToWhere >= ToDie.Length - 1)
        {
            ToDieToWhere = ToDie.Length - 1;
        }
        NewText();
    }
}
