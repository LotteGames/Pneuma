using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CameraActivate : MonoBehaviour
{
    private CameraArea[] cameraAreas;

    private void Start()
    {
        cameraAreas = FindObjectsOfType<CameraArea>();

        for (int i = 0; i < cameraAreas.Length; i++)
        {


            cameraAreas[i].SetDisplayClear();

            cameraAreas[i].SetCameraActivate(false);
            //cameraAreas[i].index = i;

        }
    }

    //public int lastOne = -1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CameraArea cameraArea = collision.GetComponentInParent<CameraArea>();

        if (cameraArea != null)
        {

            //if (lastOne == -1)
            //{
            //    lastOne = cameraArea.index;
            //}
            cameraArea.SetCameraActivate(true);

            StartCoroutine(DelaySave());

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        CameraArea cameraArea = collision.GetComponentInParent<CameraArea>();

        if (cameraArea != null)
        {
            //lastOne = cameraArea.index;

            cameraArea.SetCameraActivate(false);
        }
    }

    public int CurrentCam_AreaNum
    {
        get
        {
            return CurrentCamArea.areaNum;
        }
    }
    public CameraArea CurrentCamArea
    {
        get
        {
            int activateOne = -1;
            for (int i = 0; i < cameraAreas.Length; i++)
            {
                if (cameraAreas[i].isActivate)
                {
                    activateOne = i;
                    break;
                }
            }
            return cameraAreas[activateOne];
        }
    }
    public CameraShake CurrentCamArea_Shake
    {
        get
        {
            return CurrentCamArea.cameraShake;
        }
    }
    public IEnumerator DelaySave()
    {
        yield return new WaitForSeconds(0.13f);
        PlayerPrefs.SetFloat("CatPos_X", transform.position.x);
        PlayerPrefs.SetFloat("CatPos_Y", transform.position.y);
        GetComponent<CatContrl>().StopCat(1f);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        // Debug.Log(PlayerPrefs.GetFloat("CatPos_X") + "," + PlayerPrefs.GetFloat("CatPos_Y"));
    }
}
