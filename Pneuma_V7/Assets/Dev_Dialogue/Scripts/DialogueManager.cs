using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public List<CharacterType_and_Text> characterType_And_Texts;

    public DialogueChapter dialogue_Chapter;

    [SerializeField, HideInInspector]
    private DialogueChapter last_DialogueChapter;

    [HideInInspector]
    public DialogueData[] dialogueDatas;

    private void OnValidate()
    {

        if (dialogue_Chapter != null && last_DialogueChapter != null)
        {
            if (last_DialogueChapter != dialogue_Chapter)
            {
                ClearData();

                dialogueDatas = dialogue_Chapter.dataArray;

                //Debug.Log(dialogue_Chapter.dataArray.Length);
                //Debug.Log(dialogueDatas.Length);

                SeparateAreas();

                SeparatePuzzle();

                last_DialogueChapter = dialogue_Chapter;
            }
        }
        else if (dialogue_Chapter != null && last_DialogueChapter == null)
        {
            last_DialogueChapter = dialogue_Chapter;
        }
    }

    [HideInInspector]
    public List<int> keys_AreaNum = new List<int>();

    public Dictionary<int, List<int>> dic_Area_ExcelIndexs = new Dictionary<int, List<int>>();

    public void SeparateAreas()
    {
        int lastNum = dialogueDatas[0].Area;

        keys_AreaNum.Add(lastNum);

        List<int> dialogues = new List<int>();

        dic_Area_ExcelIndexs.Add(lastNum, dialogues);
        dic_Area_ExcelIndexs[lastNum].Add(0);


        for (int i = 1; i < dialogueDatas.Length; i++)
        {
            int num = dialogueDatas[i].Area;

            if (num == lastNum)
            {
                dic_Area_ExcelIndexs[lastNum].Add(i);
            }
            else
            {
                dialogues = new List<int>();

                dic_Area_ExcelIndexs.Add(num, dialogues);

                dic_Area_ExcelIndexs[num].Add(i);

                keys_AreaNum.Add(num);
            }
            lastNum = num;
        }
    }

    public void LogAreaIndex()
    {
        for (int i = 0; i < keys_AreaNum.Count; i++)
        {
            int num = keys_AreaNum[i];

            List<int> indexs = dic_Area_ExcelIndexs[num];

            string logIndexs = string.Empty;

            for (int j = 0; j < indexs.Count; j++)
            {
                logIndexs += indexs[j] + ",";
            }

            Debug.Log(num + " : " + logIndexs);
        }
    }

    //=========================================
    public List<Struct_PlaceSpeak> struct_PlaceSpeaks = new List<Struct_PlaceSpeak>();

    public void SeparatePuzzle()
    {
        for (int i = 0; i < keys_AreaNum.Count; i++)
        {
            int areaNum = keys_AreaNum[i];

            List<int> excelIndexs = dic_Area_ExcelIndexs[areaNum];

            int last_PuzzleNum = dialogueDatas[excelIndexs[0]].Puzzle;

            List<Struct_Speak> struct_Speaks = new List<Struct_Speak>();
            struct_Speaks.Add(new Struct_Speak(dialogueDatas[excelIndexs[0]].CHARACTERTYPE, dialogueDatas[excelIndexs[0]].Ch_Sentence));

            for (int j = 1; j < excelIndexs.Count; j++)
            {
                int current_PuzzleNum = dialogueDatas[excelIndexs[j]].Puzzle;

                if (last_PuzzleNum == current_PuzzleNum)
                {
                    //增加Speak
                    struct_Speaks.Add(new Struct_Speak(dialogueDatas[excelIndexs[j]].CHARACTERTYPE, dialogueDatas[excelIndexs[j]].Ch_Sentence));
                }
                else
                {
                    //增加PlaceSpeak
                    struct_PlaceSpeaks.Add(new Struct_PlaceSpeak(areaNum, last_PuzzleNum, struct_Speaks));

                    Debug.Log(last_PuzzleNum + " , " + current_PuzzleNum);

                    //增加新Area的Speak
                    struct_Speaks = new List<Struct_Speak>();
                    struct_Speaks.Add(new Struct_Speak(dialogueDatas[excelIndexs[j]].CHARACTERTYPE, dialogueDatas[excelIndexs[j]].Ch_Sentence));
                }

                last_PuzzleNum = current_PuzzleNum;
            }

            struct_PlaceSpeaks.Add(new Struct_PlaceSpeak(areaNum, last_PuzzleNum, struct_Speaks));
            //增加該Area最後一個的資料
        }
    }

    public void ClearData()
    {
        dic_Area_ExcelIndexs.Clear();

        keys_AreaNum.Clear();

        struct_PlaceSpeaks.Clear();
    }

    //============================

    private void Start()
    {
        for (int i = 0; i < characterType_And_Texts.Count; i++)
        {
            characterType_And_Texts[i].SetDialogueActive(false);
        }
    }

    public CameraActivate cameraActivate;
    public PlaceInfo placeInfo;

    public Struct_PlaceSpeak Find_PlaceSpeak()
    {
        int cam_AreaNum = cameraActivate.CurrentCam_AreaNum;
        int puzzleNum = placeInfo.puzzleNum;


        Struct_PlaceSpeak struct_PlaceSpeak = new Struct_PlaceSpeak();

        for (int i = 0; i < struct_PlaceSpeaks.Count; i++)
        {
            if (struct_PlaceSpeaks[i].areaNum == cam_AreaNum)
            {
                if (struct_PlaceSpeaks[i].puzzleNum == puzzleNum)
                {
                    struct_PlaceSpeak = struct_PlaceSpeaks[i];
                    break;
                }
            }
        }
        return struct_PlaceSpeak;
    }
    public Struct_PlaceSpeak Find_PlaceSpeak(int puzzleNum = -1)
    {
        int cam_AreaNum = cameraActivate.CurrentCam_AreaNum;

        Struct_PlaceSpeak struct_PlaceSpeak = new Struct_PlaceSpeak();

        for (int i = 0; i < struct_PlaceSpeaks.Count; i++)
        {
            if (struct_PlaceSpeaks[i].areaNum == cam_AreaNum)
            {
                if (struct_PlaceSpeaks[i].puzzleNum == puzzleNum)
                {
                    struct_PlaceSpeak = struct_PlaceSpeaks[i];
                    break;
                }
            }
        }
        return struct_PlaceSpeak;
    }
    public void StartDialogue()
    {
        StartCoroutine(iEnum_StartDialogue());
    }
    IEnumerator iEnum_StartDialogue()
    {
        Struct_PlaceSpeak struct_PlaceSpeak = Find_PlaceSpeak();

        if (struct_PlaceSpeak.puzzleState != null)
        {
            if (struct_PlaceSpeak.puzzleState.isCompelete)
            {
                struct_PlaceSpeak = Find_PlaceSpeak(-1);
            }
        }


        for (int i = 0; i < struct_PlaceSpeak.struct_Speaks.Count; i++)
        {
            Struct_Speak struct_Speak = struct_PlaceSpeak.struct_Speaks[i];

            CharacterType_and_Text characterType_And_Text = FindText(struct_Speak.character);


            if (characterType_And_Text == null)
            {
                Debug.LogError("Dialogue Manager 沒有對應說話對象的資料與物件參考");
            }
            else
            {
                characterType_And_Text.dialogueObj.SetActive(true);
                characterType_And_Text.tmp_Text.text = "";
                characterType_And_Text.SetCanvasOrder(i);

                string text = struct_Speak.txt;

                for (int j = 0; j < text.Length; j++)
                {
                    characterType_And_Text.tmp_Text.text += text[j];
                    yield return new WaitForSeconds(0.13f);
                }
            }
        }

        yield return new WaitForSeconds(0.5f);

        for (int k = 0; k < characterType_And_Texts.Count; k++)
        {
            characterType_And_Texts[k].SetDialogueActive(false);
            characterType_And_Texts[k].SetCanvasOrder(0);
        }

        InvokeEvent_TalkFin();
    }

    public CharacterType_and_Text FindText(CharacterType characterType)
    //記得檢查puzzleState，如果已經完成就講閒話
    {
        for (int i = 0; i < characterType_And_Texts.Count; i++)
        {
            if (characterType_And_Texts[i].characterType == characterType)
            {
                return characterType_And_Texts[i];
            }
        }

        return null;
    }


    public UnityEvent event_Talk, event_Fin;

    public void InvokeEvent_Talk()
    {
        event_Talk.Invoke();
    }

    public void InvokeEvent_TalkFin()
    {
        event_Fin.Invoke();
    }

}
