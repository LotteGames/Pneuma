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

#if (UNITY_EDITOR != true)
    cameraAreas[i].SetDisplayClear();
#endif
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
            StartCoroutine(DelaySave());
            GetComponent<CatContrl>().StopCat(1f);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

            cameraArea.SetCameraActivate(true);
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
            int activateOne = -1;
            for (int i = 0; i < cameraAreas.Length; i++)
            {
                if (cameraAreas[i].isActivate)
                {
                    activateOne = i;
                    break;
                }
            }
            return cameraAreas[activateOne].areaNum;
        }
    }
    public IEnumerator DelaySave()
    {
        yield return new WaitForSeconds(0.13f);
        PlayerPrefs.SetFloat("CatPos_X", transform.position.x);
        PlayerPrefs.SetFloat("CatPos_Y", transform.position.y);
        // Debug.Log(PlayerPrefs.GetFloat("CatPos_X") + "," + PlayerPrefs.GetFloat("CatPos_Y"));
    }
}
