using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DialogueChapter : ScriptableObject
{
    [HideInInspector]
    [SerializeField]
    public string SheetName = "";

    [HideInInspector]
    [SerializeField]
    public string WorksheetName = "";

    // Note: initialize in OnEnable() not here.
    [SerializeField] public DialogueData[] dataArray;
    [SerializeField] public List<DialogueData> dataList;//為甚麼沒有顯示在Inspector上

    void OnEnable()
    {
        //#if UNITY_EDITOR
        //hideFlags = HideFlags.DontSave;
        //#endif
        // Important:
        //    It should be checked an initialization of any collection data before it is initialized.
        //    Without this check, the array collection which already has its data get to be null 
        //    because OnEnable is called whenever Unity builds.
        // 		
        if (dataArray == null)
            dataArray = new DialogueData[0];
        dataList = new List<DialogueData>(dataArray);

    }

    //
    // Highly recommand to use LINQ to query the data sources.
    //

}
