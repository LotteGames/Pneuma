using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class DialoguesManager : MonoBehaviour
{
    public List<AreaDialogues> areaDialogues;

    public CameraActivate cameraActivate;
    public int CurrentAreaNum
    {
        get
        {
            return cameraActivate.CurrentCam_AreaNum;
        }
    }

    public AreaDialogues GetAreaDialogues()
    {
        for (int i = 0; i < areaDialogues.Count; i++)
        {
            if (areaDialogues[i].areaNum == CurrentAreaNum)
            {
                return areaDialogues[i];
            }
        }
        return null;
    }


    public PlayerProgress playerProgress;

    public bool IsConditionsCompelete(List<int> conditions)
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            if (playerProgress.GetIsCompelete(conditions[i]) == false)
            {
                return false;
            }
        }
        return true;
    }


    public List<Role> GetRoles(Dialogue dialogue)
    {
        List<Role> roles = new List<Role>();

        for (int i = 0; i < dialogue.sentences.Count; i++)
        {
            if (!roles.Contains(dialogue.sentences[i].speaker))
            {
                roles.Add(dialogue.sentences[i].speaker);
            }
        }

        return roles;
    }

    public List<RoleObjs> roleObjs;

    public RoleObjs GetRoleObjs(Role role)
    {
        for (int i = 0; i < roleObjs.Count; i++)
        {
            if (roleObjs[i].role == role)
            {
                return roleObjs[i];
            }
        }
        return null;
    }

    public bool RoleInSameArea(Dialogue dialogue)
    {
        List<Role> roles = GetRoles(dialogue);

        CurrentAea currentAea = FindObjectOfType<CurrentAea>();

        List<int> areaNums = new List<int>();


        for (int i = 0; i < roles.Count; i++)
        {
            areaNums.Add(
                currentAea.GetCurrentArea(
                    GetRoleObjs(roles[i]).Position)
                );
        }


        int value = -1;

        for (int i = 0; i < areaNums.Count; i++)
        {
            if (value == -1)
            {
                value = areaNums[i];
            }
            else
            {
                if (value != areaNums[i])
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void StartDialogue()
    {
        if (isCompelete)
        {
            AreaDialogues areaDialogues = GetAreaDialogues();

            if (areaDialogues != null)
            {
                Dialogue dialogue = areaDialogues.GetDirectTriggerOne();

                if (dialogue != null)//有要直接觸發的
                {
                    if (IsConditionsCompelete(dialogue.taskConditions) && RoleInSameArea(dialogue) && dialogue.isCompelete == false)
                    {

                        StartCoroutine(TextWriter(dialogue));
                        return;
                    }
                }

                if (Input.GetKeyDown(KeyCode.C) && IsConditionsCompelete(dialogue.taskConditions) && RoleInSameArea(dialogue) && dialogue.isCompelete == false)
                {
                    StartCoroutine(TextWriter(dialogue));
                    return;
                }

            }
        }
    }
    bool isCompelete = true;
    IEnumerator TextWriter(Dialogue dialogue)
    {
        Debug.LogError(132);

        isCompelete = false;

        bool sentenceFin = true;

        int i = 0;
        SetCamActive(true);
        SetCamGroup(GetRoles(dialogue));

        RoleObjs roleObjs = GetRoleObjs(dialogue.sentences[i].speaker);



        while (i < dialogue.sentences.Count)
        {
            if (sentenceFin)
            {
                roleObjs.SetBubble(true);
                roleObjs.BubbleScaleUp("<color=#00000000>"+dialogue.sentences[i].text + "</color>");

                yield return new WaitForSeconds(0.2f);

                for (int j = 0; j < dialogue.sentences[i].text.Length; j++)
                {
                    string txt = dialogue.sentences[i].text.Substring(0, j + 1) +
                        "<color=#00000000>" + dialogue.sentences[i].text.Substring(j + 1) + "</color>";

                    roleObjs.SetText(txt);

                    float timepass = 0, duration = 0.04f;
                    while (timepass < duration)
                    {
                        if (Input.GetKeyDown(KeyCode.C))
                        {
                            roleObjs.SetText(dialogue.sentences[i].text);
                            j = dialogue.sentences[i].text.Length;
                        }

                        timepass += Time.deltaTime;
                        yield return new WaitForSeconds(Time.deltaTime);
                    }
                }
                roleObjs.SetContinueBtn(true);
                sentenceFin = false;
            }
            else
            {

                if (Input.GetKeyDown(KeyCode.C) && i< dialogue.sentences.Count-1 )
                {
                    sentenceFin = true;

                    roleObjs.SetContinueBtn(false);
                    i++;

                    roleObjs.BubbleScaleDown();
                    yield return new WaitForSeconds(0.2f);
                }
                else if (Input.GetKeyDown(KeyCode.C) && i >= dialogue.sentences.Count-1 )
                {
                    roleObjs.SetContinueBtn(false);

                    roleObjs.BubbleScaleDown();
                    SetCamActive(false);
                    FindObjectOfType<CatContrl>().NowCatAct = CatContrl.CatAct.Idle;
                    yield return new WaitForSeconds(0.2f);
                    i++;
                    dialogue.isCompelete = true; isCompelete = true;
                }

            }
            yield return null;
        }
    }



    public void SetCat()
    {
        CatContrl catContrl = FindObjectOfType<CatContrl>();
        catContrl.NowCatAct = CatContrl.CatAct.CatStop;

        Rigidbody2D rb2d = catContrl.GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);


    }

    public GameObject dialogueCam, targetGroup;

    public void SetCamActive(bool value)
    {
        dialogueCam.SetActive(value);
    }
    public void SetCamGroup(List<Role> roles)
    {
        CinemachineTargetGroup cinemachineTargetGroup = targetGroup.GetComponent<CinemachineTargetGroup>();
        cinemachineTargetGroup.m_Targets = new CinemachineTargetGroup.Target[roles.Count + 1];

        for (int i = 0; i < roles.Count; i++)
        {
            cinemachineTargetGroup.m_Targets.SetValue(GetRoleObjs(roles[i]).target, i);
        }
        cinemachineTargetGroup.m_Targets.SetValue(target, roles.Count);
    }
    public Cinemachine.CinemachineTargetGroup.Target target;

}
[System.Serializable]
public class AreaDialogues
{
    public int areaNum;

    public List<Dialogue> dialogues;

    public Dialogue GetDirectTriggerOne()
    {
        for (int i = 0; i < dialogues.Count; i++)
        {
            if (dialogues[i].directTrigger)
            {
                return dialogues[i];
            }
        }
        return null;
    }
}