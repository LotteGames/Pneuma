using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

[ExecuteAlways]
public class CameraArea : MonoBehaviour
{

    public bool isActivate = false;

    private CinemachineVirtualCamera virtualCamera;

    public CinemachineVirtualCamera VirtualCamera
    {
        get
        {
            return virtualCamera;
        }
    }

    public E_CameraType cameraType;

    public PolygonCollider2D limit, area;

    public int areaNum = 0;

    private void Start()
    {
        #region--GetComponent--

        if (virtualCamera == null)
        {
            virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }

        #endregion
    }
    private void OnValidate()
    {
        if (cameraType == E_CameraType.ConstantCamera)
        {
            if (virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>() != null)
            {
                virtualCamera.DestroyCinemachineComponent<CinemachineFramingTransposer>();
            }
            virtualCamera.transform.localPosition = new Vector3(0, 0, -10);

        }
        else
        {
            virtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();

            virtualCamera.Follow = FindObjectOfType<CatContrl>().transform;
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
    }
}
public enum E_CameraType
{
    ConstantCamera,
    LimitedspaceFreeCamera,
}