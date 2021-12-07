using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public List<Dialogue_Ch1Data> dialogue_Ch1Datas;

    private void Start()
    {
        DataPool.InitData();

        //dialogue_Ch1Datas = DataPool.dialogue_Ch1.dataList;

        if (dialogue_Ch1Datas != null)
        {
            Debug.LogError("Get Data Ch1 !");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Dialogue_Ch1Data dialogue_Ch1Data = FindData(area, sequence);

            switch (e_Language)//Switch可能不太好，改成Dictionary?
            {
                case E_Language.Chinese:

                    Debug.LogError(dialogue_Ch1Data.Ch_Charactername + " Say : " + dialogue_Ch1Data.Ch_Sentence);
                    break;

                case E_Language.English:
                   
                    Debug.LogError(dialogue_Ch1Data.Eng_Charactername + " Say : " + dialogue_Ch1Data.Eng_Sentence);
                    break;
            }
        }
    }

    public int area = 0, sequence = 0;
    public E_Language e_Language;

    public Dialogue_Ch1Data FindData(int area, int sequence) 
    {
        for (int i = 0; i < dialogue_Ch1Datas.Count; i++) 
        {
            Dialogue_Ch1Data dialogue_Ch1Data = dialogue_Ch1Datas[i];

            if (dialogue_Ch1Data.Area == area && dialogue_Ch1Data.Sequence == sequence) 
            {
                return dialogue_Ch1Data;
            }
        }return null;
    }

}

public enum E_Language 
{
Chinese,
English,
}
//需要畫面動畫，如藤蔓之類的
//