using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    public List<RoleBubble> RoleBubbles;

    public DialogueChapter chapter;

    [SerializeField, HideInInspector]
    private DialogueChapter lastChapter;

    [HideInInspector]
    public DialogueData[] dialogueDatas;

    private void OnValidate()
    {
        if (chapter != null && lastChapter != null)
        {
            if (lastChapter != chapter)
            {
                ClearData();

                dialogueDatas = chapter.dataArray;

                //Debug.Log(chapter.dataArray.Length);
                //Debug.Log(dialogueDatas.Length);

                SeparateAreas();

                SeparatePuzzle();

                lastChapter = chapter;
            }
        }
        else if (chapter != null && lastChapter == null)
        {
            lastChapter = chapter;
        }
    }

    [HideInInspector]
    public List<int> areaNums = new List<int>();

    public Dictionary<int, List<int>> Area_To_ExcelIndex = new Dictionary<int, List<int>>();

    public void SeparateAreas()
    {
        int lastNum = dialogueDatas[0].Area;

        areaNums.Add(lastNum);

        List<int> dialogues = new List<int>();

        Area_To_ExcelIndex.Add(lastNum, dialogues);
        Area_To_ExcelIndex[lastNum].Add(0);


        for (int i = 1; i < dialogueDatas.Length; i++)
        {
            int num = dialogueDatas[i].Area;

            if (num == lastNum)
            {
                Area_To_ExcelIndex[lastNum].Add(i);
            }
            else
            {
                dialogues = new List<int>();

                Area_To_ExcelIndex.Add(num, dialogues);

                Area_To_ExcelIndex[num].Add(i);

                areaNums.Add(num);
            }
            lastNum = num;
        }
    }

    public void LogAreaIndex()
    {
        for (int i = 0; i < areaNums.Count; i++)
        {
            int num = areaNums[i];

            List<int> indexs = Area_To_ExcelIndex[num];

            string logIndexs = string.Empty;

            for (int j = 0; j < indexs.Count; j++)
            {
                logIndexs += indexs[j] + ",";
            }

            Debug.Log(num + " : " + logIndexs);
        }
    }

    //=========================================
    public List<PlaceSpeaks> struct_PlaceSpeaks = new List<PlaceSpeaks>();

    public void SeparatePuzzle()
    {
        for (int i = 0; i < areaNums.Count; i++)
        {
            int areaNum = areaNums[i];

            List<int> excelIndexs = Area_To_ExcelIndex[areaNum];

            int last_PuzzleNum = dialogueDatas[excelIndexs[0]].Puzzle;

            List<Speak> struct_Speaks = new List<Speak>();
            struct_Speaks.Add(new Speak(dialogueDatas[excelIndexs[0]].CHARACTERTYPE, dialogueDatas[excelIndexs[0]].Ch_Sentence));

            for (int j = 1; j < excelIndexs.Count; j++)
            {
                int current_PuzzleNum = dialogueDatas[excelIndexs[j]].Puzzle;

                if (last_PuzzleNum == current_PuzzleNum)
                {
                    //�W�[Speak
                    struct_Speaks.Add(new Speak(dialogueDatas[excelIndexs[j]].CHARACTERTYPE, dialogueDatas[excelIndexs[j]].Ch_Sentence));
                }
                else
                {
                    //�W�[PlaceSpeak
                    struct_PlaceSpeaks.Add(new PlaceSpeaks(areaNum, last_PuzzleNum, struct_Speaks));

                    //Debug.Log(last_PuzzleNum + " , " + current_PuzzleNum);

                    //�W�[�sArea��Speak
                    struct_Speaks = new List<Speak>();
                    struct_Speaks.Add(new Speak(dialogueDatas[excelIndexs[j]].CHARACTERTYPE, dialogueDatas[excelIndexs[j]].Ch_Sentence));
                }

                last_PuzzleNum = current_PuzzleNum;
            }

            struct_PlaceSpeaks.Add(new PlaceSpeaks(areaNum, last_PuzzleNum, struct_Speaks));
            //�W�[��Area�̫�@�Ӫ����
        }
    }

    public void ClearData()
    {
        Area_To_ExcelIndex.Clear();

        areaNums.Clear();

        struct_PlaceSpeaks.Clear();
    }

    //============================

    private void Start()
    {
        for (int i = 0; i < RoleBubbles.Count; i++)
        {
            RoleBubbles[i].SetBubbleActive(false);
        }
    }

    public CameraActivate cameraActivate;
    public PlaceInfo placeInfo;

    //public PlaceSpeaks Find_PlaceSpeak()
    //{
    //    int cam_AreaNum = cameraActivate.CurrentCam_AreaNum;
    //    int puzzleNum = placeInfo.puzzleNum;


    //    PlaceSpeaks struct_PlaceSpeak = new PlaceSpeaks();

    //    for (int i = 0; i < struct_PlaceSpeaks.Count; i++)
    //    {
    //        if (struct_PlaceSpeaks[i].areaNum == cam_AreaNum)
    //        {
    //            if (struct_PlaceSpeaks[i].puzzleNum == puzzleNum)
    //            {
    //                struct_PlaceSpeak = struct_PlaceSpeaks[i];
    //                break;
    //            }
    //        }
    //    }
    //    return struct_PlaceSpeak;
    //}
    public PlaceSpeaks Find_PlaceSpeak(int puzzleNum = -1)
    {
        int cam_AreaNum = cameraActivate.CurrentCam_AreaNum;

        Debug.LogError("camAreaNum : " + cam_AreaNum);

        PlaceSpeaks struct_PlaceSpeak = new PlaceSpeaks();
        Debug.LogError("puzzleNum : " + puzzleNum);
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
    PuzzleState puzzleState;

    public void StartDialogue()
    {
        puzzleState = placeInfo.puzzleState;

        Debug.LogError("StartDialogue");
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ieum_StartDialogue(placeInfo.puzzleNum));
    }
    Coroutine coroutine;

    public float oneCharduration = 0.04f;
    IEnumerator ieum_StartDialogue(int puzzleNum)
    {
        PlaceSpeaks placeSpeaks = Find_PlaceSpeak(puzzleNum);

        if (placeSpeaks.puzzleState != null)
        {
            if (placeSpeaks.puzzleState.isCompelete)
            {
                Debug.LogError(-1);
                placeSpeaks = Find_PlaceSpeak(-1);
            }
        }



        int i = 0; bool finDialogue = false, finSentence = true;

        yield return new WaitForEndOfFrame();

        if (placeSpeaks.speaks == null)
        {
            Debug.LogError("��Ƹ̨S���b�o��Area�BPuzzleNum�����");
        }
        else
        {
            while (i < placeSpeaks.speaks.Count)
            {
                if (!finDialogue)
                {
                    Speak speak = placeSpeaks.speaks[i];

                    RoleBubble roleBubble = FindText(speak.character);

                    if (finSentence)
                    {
                        if (roleBubble == null)
                        {
                            Debug.LogError("Dialogue Manager �S���������ܹ�H����ƻP����Ѧ�");
                        }
                        else
                        {
                            roleBubble.SetBubbleActive(true);
                            roleBubble.dialogueBubble.localScale = Vector3.zero;

                            roleBubble.text.text = "";
                            //roleBubble.SetCanvasOrder(i);

                            string text = speak.txt;
                            roleBubble.text.SetText(text);
                            roleBubble.text.ForceMeshUpdate();
                            Vector2 textSize = roleBubble.text.GetRenderedValues(false);

                            Debug.LogError(textSize.ToString());

                            Vector2 padding = new Vector2(2, 2);

                            if (textSize.x < textSize.y)
                            {
                                textSize = new Vector2(textSize.y, textSize.x);
                            }

                            roleBubble.dialogueBubble.sizeDelta = textSize + padding;

                            roleBubble.dialogueBubble.DOScale(Vector3.one, scaleUp);

                            roleBubble.text.SetText("");
                            yield return new WaitForSeconds(scaleUp);
                            //Debug.LogError(text.Length);

                            for (int j = 0; j < text.Length; j++)
                            {
                                //Debug.LogError(j);
                                roleBubble.text.text = speak.txt.Substring(0, j + 1) +
                                    "<color=#00000000>" + speak.txt.Substring(j + 1) + "</color>";

                                float timepass = 0, duration = oneCharduration;
                                while (timepass < duration)
                                {
                                    if (Input.GetKeyDown(KeyCode.C))
                                    {
                                        roleBubble.text.text = text;
                                        j = text.Length;
                                    }

                                    timepass += Time.deltaTime;
                                    yield return new WaitForSeconds(Time.deltaTime);
                                }
                            }
                        }

                        finSentence = false;
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.C) && i != placeSpeaks.speaks.Count - 1)
                        {
                            //roleBubble.SetBubbleActive(false);
                            i++;
                            finSentence = true;

                            roleBubble.text.text = "";
                            roleBubble.dialogueBubble.DOScale(Vector3.zero, scaleDown);
                            yield return new WaitForSeconds(scaleDown);
                        }
                        else if (Input.GetKeyDown(KeyCode.C) && i == placeSpeaks.speaks.Count - 1)
                        {
                            finDialogue = true;
                            roleBubble.text.text = "";
                            roleBubble.dialogueBubble.DOScale(Vector3.zero, scaleDown);
                            yield return new WaitForSeconds(scaleDown);
                        }
                        yield return null;
                    }
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);

                    for (int k = 0; k < RoleBubbles.Count; k++)
                    {
                        RoleBubbles[k].SetBubbleActive(false);
                        //RoleBubbles[k].SetCanvasOrder(0);
                    }
                    i++;
                }
            }
        }
        InvokeEvent_TalkFin();

        if (puzzleState != null)
        {
            puzzleState.InvokeEvent();
            puzzleState = null;
        }
    }

    public RoleBubble FindText(Role role)
    //�O�o�ˬdpuzzleState�A�p�G�w�g�����N������
    {
        for (int i = 0; i < RoleBubbles.Count; i++)
        {
            if (RoleBubbles[i].role == role)
            {
                return RoleBubbles[i];
            }
        }

        return null;
    }
    public float scaleUp = 0.2f, scaleDown = 0.2f;

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
