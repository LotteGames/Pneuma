using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentAea : MonoBehaviour
{
    CameraArea[] cameraAreas;

    private void Awake()
    {
        cameraAreas = FindObjectsOfType<CameraArea>();
    }

    public int GetCurrentArea(Vector3 position)
    {
        int areaNum = 0;
        float distance = float.PositiveInfinity;

        for (int i = 0; i < cameraAreas.Length; i++)
        {
            float dis = (cameraAreas[i].transform.position - position).magnitude;

            if (dis < distance)
            {
                distance = dis;
                areaNum = cameraAreas[i].areaNum;
            }
        }
        return areaNum;
    }
}
