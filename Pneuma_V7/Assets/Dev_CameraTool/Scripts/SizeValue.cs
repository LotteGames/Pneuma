using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

/// <summary>
/// SizeValue，用來取得並顯示攝影機Size的數值大小
/// </summary>
[ExecuteAlways]
public class SizeValue : MonoBehaviour
{
    private CameraArea cameraArea;

    [Space(20)]
    [SerializeField]
    private ValueEvent m_OnValueChanged = new ValueEvent();

    private float oldValue, currentValue;
    public float Size
    {
        get
        {
            return currentValue;
        }
    }

    private void Awake()
    {
        cameraArea = GetComponentInParent<CameraArea>();
    }
    private void Start()
    {
        if (Camera.main.orthographic)
        {
            oldValue = cameraArea.VirtualCamera.m_Lens.OrthographicSize;
            currentValue = oldValue;
        }
    }
    private void Update()
    {
        if (Camera.main.orthographic)
        {
            currentValue = cameraArea.VirtualCamera.m_Lens.OrthographicSize;

            if (currentValue != oldValue)
            {
                m_OnValueChanged.Invoke();

                oldValue = currentValue;
            }
        }
    }
}
[System.Serializable]
public class ValueEvent : UnityEvent { }


