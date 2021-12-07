using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Ai_Decision : MonoBehaviour
{
    public float jumpVelocity = 0f;

    private Ai_PhysicSetting physicSetting;
    private void Awake()
    {
        physicSetting = GetComponent<AiBehaviour>().physicSetting;
        physicSetting.CalculateValues();
    }

    public float GetAngle(Vector3 vectorToNext)
    {
        float value = 90 - Vector2.Angle(vectorToNext, Vector2.up);

        return value;
    }

    public DicisionAndTime Teleporation(Vector3 vectorToNext, Vector3 targetPos)
    {
        DicisionAndTime dicisionAndTime = new DicisionAndTime();

        dicisionAndTime.SetTeleporation(0.05f, vectorToNext, targetPos);

        return dicisionAndTime;
    }
    public DicisionAndTime Walk(float duration, Vector3 vectorToNext, float speed, Vector3 targetPos)
    {
        DicisionAndTime dicisionAndTime = new DicisionAndTime();

        dicisionAndTime.SetWalk(duration, vectorToNext, speed, targetPos);

        return dicisionAndTime;
    }
    public DicisionAndTime Jump(float duration, Vector3 vectorToNext, float jumpVelocity, float speed, Vector3 targetPos)
    {
        DicisionAndTime dicisionAndTime = new DicisionAndTime();

        dicisionAndTime.SetJump(duration, vectorToNext, jumpVelocity, speed, targetPos);

        return dicisionAndTime;
    }

    public List<DicisionAndTime> GetDicisions(List<PathNode> pathNodes)
    {
        List<DicisionAndTime> dicisionAndTimes = new List<DicisionAndTime>();

        //dicisionAndTimes.Add(Teleporation(pathNodes[0].node.Position - transform.position, pathNodes[0].node.Position));

        //������Ĥ@��Node����m





        //�@�}�l�]�\��������
        //�ӬO�o�X��|��
        //�η�e��m���|���ĤG���I�h���p��
        //�o�ˤ~��q���Ӧ�m�}�l����


        for (int i = 0; i < pathNodes.Count - 1; i++)
        {
            float angle;
            bool isGround;
            Vector3 vectorToNext;

            if (i == 0)
            {
                angle = GetAngle(pathNodes[1].node.Position - transform.position);

                isGround = Ai_Detect.GetIsGround(transform.position, pathNodes[1].node);

                vectorToNext = pathNodes[1].node.Position - transform.position;
            }
            else
            {
                angle = GetAngle(pathNodes[i].vectorToNext);

                isGround = Ai_Detect.GetIsGround(pathNodes[i].node, pathNodes[i + 1].node);

                vectorToNext = pathNodes[i].vectorToNext;
            }

            Debug.LogWarning("Path Node " + pathNodes[i].node.NodeNum + " to Path Node " + pathNodes[i + 1].node.NodeNum + " , angle : " + angle + " , IsGround : " + isGround);

            if (vectorToNext.y >= 0)
            {
                if (isGround)
                {
                    if (angle <= physicSetting.maxSlopeAngle)
                    {
                        //�n�ݩY�׬O�_�D0��
                        //�p�G�O���ܡA�n���p��Ө��פU�A��e�t�ת�������q�򫫪����q

                        dicisionAndTimes.Add(Walk(Mathf.Abs(vectorToNext.x) / physicSetting.speed, vectorToNext, ((vectorToNext.x > 0) ? 1 : (vectorToNext.x < 0) ? -1 : 0) * physicSetting.speed, pathNodes[i + 1].node.Position));
                    }
                    else//�Y�פj��i���Y��
                    {
                        //O=origin���I,H=highest�̰��I,G=Goal���I
                        float verticleDistance_OtoH = vectorToNext.y + 1;//1�u�O�H�N�q���Ψ���Ϥ�����

                        float time_OtoH = Mathf.Sqrt(verticleDistance_OtoH / (physicSetting.gravity / 2));

                        float time_HtoG = Mathf.Sqrt(1 / (physicSetting.gravity / 2));

                        float duration = time_OtoH + time_HtoG;

                        float jumpVelocity = (time_OtoH * physicSetting.gravity);


                        if (vectorToNext.y - 1 <= physicSetting.maxJumpHeight &&//-1�O�]���W���]�w�̰��I�|�OGoal+1�A�T�OGoal�|�O�̰��I
                            Math.Abs(vectorToNext.x) <= physicSetting.speed * duration)
                        {
                            dicisionAndTimes.Add(Jump(duration, vectorToNext, jumpVelocity, Math.Abs(vectorToNext.x) / duration, pathNodes[i + 1].node.Position));
                        }
                        else
                        {
                            dicisionAndTimes.Add(Teleporation(pathNodes[i].vectorToNext, pathNodes[i + 1].node.Position));
                        }
                    }
                }
                else
                {

                    float verticleDistance_HtoG = Mathf.Abs(vectorToNext.y) + 1f;

                    float time_GtoH = Mathf.Sqrt(verticleDistance_HtoG / (physicSetting.gravity / 2f));

                    float time_HtoO = Mathf.Sqrt(1f / (physicSetting.gravity / 2f));

                    float duration = time_GtoH + time_HtoO;

                    float jumpV = (time_GtoH * physicSetting.gravity);


                    if (verticleDistance_HtoG < physicSetting.maxJumpHeight &&
                        Math.Abs(vectorToNext.x) < physicSetting.speed * duration)
                    {
                        dicisionAndTimes.Add(Jump(duration, vectorToNext, jumpV, Math.Abs(vectorToNext.x) / duration, pathNodes[i + 1].node.Position));

                    }
                    else
                    {
                        dicisionAndTimes.Add(Teleporation(pathNodes[i].vectorToNext, pathNodes[i + 1].node.Position));
                    }
                }
            }
            else if (vectorToNext.y < 0)
            {
                if (isGround)
                {
                    if (angle <= physicSetting.maxSlopeAngle)
                    {
                        //Debug.Log(346 + " , " + Mathf.Abs(vectorToNext.x) / physicSetting.speed);
                        dicisionAndTimes.Add(Walk(Mathf.Abs(vectorToNext.x) / physicSetting.speed, vectorToNext, ((vectorToNext.x > 0) ? 1 : (vectorToNext.x < 0) ? -1 : 0) * physicSetting.speed, pathNodes[i + 1].node.Position));
                    }
                    else
                    {
                        dicisionAndTimes.Add(Teleporation(pathNodes[i].vectorToNext, pathNodes[i + 1].node.Position));
                    }
                }
                else
                {
                    float time_WalktoEdge = 1f / physicSetting.speed;

                    float time_OtoG_y = Mathf.Sqrt(Mathf.Abs(vectorToNext.y) / (physicSetting.gravity / 2));
                    //������ʸ��U�һݮɶ�

                    Vector3 fallPos = pathNodes[i].node.Position + new Vector3(time_OtoG_y * ((vectorToNext.x > 0) ? 1 : (vectorToNext.x < 0) ? -1 : 0) * physicSetting.speed, vectorToNext.y);

                    bool isPosGround = Ai_Detect.GetIsGround(fallPos, Color.white, 3);

                    bool isGroundConnect = Ai_Detect.GetIsGround((fallPos + pathNodes[i + 1].node.Position) / 2, Color.yellow, 3);


                    //Debug.Log("IsGround : " + isPosGround + " , " + isGroundConnect);


                    float time_OtoG_x = (Mathf.Abs(vectorToNext.x) / physicSetting.speed);


                    //Debug.Log("time_OtoG_y : " + time_OtoG_y + " , time_OtoG_x : " + time_OtoG_x);

                    if (Mathf.Abs(vectorToNext.x) <= time_OtoG_y * physicSetting.speed ||
                        (isPosGround && isGroundConnect))
                    {
                        float speed_OtoG = vectorToNext.x / (time_OtoG_y + time_WalktoEdge);
                        float speed_withDir = ((vectorToNext.x > 0) ? 1 : (vectorToNext.x < 0) ? -1 : 0) * physicSetting.speed;


                        dicisionAndTimes.Add(Walk(time_OtoG_y + time_WalktoEdge, vectorToNext, speed_OtoG, pathNodes[i + 1].node.Position));
                    }
                    else
                    {
                        dicisionAndTimes.Add(Teleporation(pathNodes[i].vectorToNext, pathNodes[i + 1].node.Position));
                    }

                }
            }
        }
        return dicisionAndTimes;
    }

}

