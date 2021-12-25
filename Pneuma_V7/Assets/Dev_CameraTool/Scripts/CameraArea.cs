using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

[ExecuteAlways]
public class CameraArea : MonoBehaviour
{

    public bool isActivate = false;

    public CinemachineVirtualCamera VirtualCamera;


    public E_CameraType cameraType;

    public PolygonCollider2D limit, area;

    public int areaNum = 0;


    private void Update()
    {
        if (cameraType == E_CameraType.ConstantCamera)
        {
            if (VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>() != null)
            {
                StartCoroutine(ie_Destory());
            }
            VirtualCamera.transform.localPosition = new Vector3(0, 0, -10);

        }
        else
        {
            StartCoroutine(ie_Add());

            VirtualCamera.Follow = FindObjectOfType<CatContrl>().transform;
        }
    }

    IEnumerator ie_Destory()
    {
        yield return new WaitForEndOfFrame();
        VirtualCamera.DestroyCinemachineComponent<CinemachineFramingTransposer>();
    }
    IEnumerator ie_Add()
    {
        yield return new WaitForEndOfFrame();
        VirtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
    }
    public void SetCameraActivate(bool value)
    {
        isActivate = value;

        if (VirtualCamera == null)
        {
            VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }
        VirtualCamera.enabled = value;
    }
}
public enum E_CameraType
{
    ConstantCamera,
    LimitedspaceFreeCamera,
}