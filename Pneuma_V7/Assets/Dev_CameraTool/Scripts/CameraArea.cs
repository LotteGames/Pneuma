using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

[ExecuteAlways]
public class CameraArea : MonoBehaviour
{
    public bool isActivate = false;

    public Transform player;

    private CinemachineVirtualCamera virtualCamera;

    public CinemachineVirtualCamera VirtualCamera
    {
        get
        {
            return virtualCamera;
        }
    }

    private CinemachineConfiner cinemachineConfiner;

    private CameraDisplay cameraDisplay;

    private LimitedSpace limitedSpace;

    public E_CameraType cameraType;

    [Space(20)]
    public bool isDisplay;
    public Color color_Camera = Color.blue, color_Space = Color.yellow;

    public int areaNum = 0;


    public CameraShake cameraShake;
    private void OnValidate()
    {
        #region--GetComponent--

        if (virtualCamera == null)
        {
            virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }

        if (cinemachineConfiner == null)
        {
            cinemachineConfiner = virtualCamera.GetComponentInChildren<CinemachineConfiner>();
        }

        if (cameraDisplay == null)
        {
            cameraDisplay = GetComponentInChildren<CameraDisplay>();
        }

        if (limitedSpace == null)
        {
            limitedSpace = GetComponentInChildren<LimitedSpace>();
        }

        #endregion


        if (player != null)
        {
            if (cameraType == E_CameraType.Area_Equal_Camera)
            {
                virtualCamera.Follow = null;

                virtualCamera.transform.localPosition = new Vector3(0, 0, -10);


                cinemachineConfiner.enabled = false;

                cameraDisplay.transform.localPosition = new Vector3(0, 0, 0);

                cameraDisplay.SetCollider(true);


                limitedSpace.enabled = false;

                limitedSpace.SetCollider(false);


                if (isDisplay)
                {
                    cameraDisplay.SetColor(color_Camera);

                    limitedSpace.SetColor(Color.clear);
                }
                else
                {
                    cameraDisplay.SetColor(Color.clear);

                    limitedSpace.SetColor(Color.clear);
                }
            }
            else if (cameraType == E_CameraType.LimitedSpace_Free_Camera)
            {
                virtualCamera.Follow = player;


                cinemachineConfiner.enabled = true;


                cameraDisplay.SetCollider(false);


                limitedSpace.enabled = true;

                limitedSpace.SetCollider(true);


                if (isDisplay)
                {
                    cameraDisplay.SetColor(color_Camera);

                    limitedSpace.SetColor(color_Space);
                }
                else
                {
                    cameraDisplay.SetColor(Color.clear);

                    limitedSpace.SetColor(Color.clear);
                }
            }
        }
    }

    public void SetDisplayClear()
    {
        cameraDisplay = GetComponentInChildren<CameraDisplay>();

        if (cameraDisplay != null)
        {
            cameraDisplay.SetColor(Color.clear);
        }


        limitedSpace = GetComponentInChildren<LimitedSpace>();

        if (limitedSpace != null)
        {
            limitedSpace.SetColor(Color.clear);
        }
    }


    public void SetCameraActivate(bool value)
    {
        isActivate = value;

        if (virtualCamera == null)
        {
            virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }
        virtualCamera.enabled = value;



        if (cameraType == E_CameraType.Area_Equal_Camera)
        {
            if (cameraDisplay == null)
            {
                cameraDisplay = GetComponentInChildren<CameraDisplay>();
            }

            cameraDisplay.enabled = value;

        }
        else if (cameraType == E_CameraType.LimitedSpace_Free_Camera)
        {
            if (cameraDisplay == null)
            {
                cameraDisplay = GetComponentInChildren<CameraDisplay>();
            }

            cameraDisplay.enabled = value;



            if (limitedSpace == null)
            {
                limitedSpace = GetComponentInChildren<LimitedSpace>();
            }

            limitedSpace.enabled = value;
        }
    }
}
public enum E_CameraType
{
    Area_Equal_Camera,
    LimitedSpace_Free_Camera,
}