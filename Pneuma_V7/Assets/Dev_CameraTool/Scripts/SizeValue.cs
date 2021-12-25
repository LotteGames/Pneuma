using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

[ExecuteAlways]
public class SizeValue : MonoBehaviour
{
    public CameraArea cameraArea;

    public PolygonCollider2D limit//������v���ಾ�ʽd��
        , area//�����v���������ʽd��
        ;

    private Vector2[] oldValue, currentValue
        , limitOrigin = new Vector2[] { new Vector2(-0.01f, 0.01f), new Vector2(-0.01f, -0.01f), new Vector2(0.01f, -0.01f), new Vector2(0.01f, 0.01f) };
    //FOV���ɦ�Size
    //Size�A�h������v�����e��
    //�M�������Area

    private float oldFOV, currentFOV;

    private void Update()
    {
        if (cameraArea != null && limit != null && area != null)
        {

            currentFOV = cameraArea.VirtualCamera.m_Lens.FieldOfView;
            if (oldFOV != currentFOV)
            {
                oldFOV = currentFOV;

                float halfSize = 10 * Mathf.Tan(currentFOV * 0.5f * Mathf.Deg2Rad);

                Vector2[] newArea = new Vector2[] {
                    new Vector2(-halfSize / 9f * 16, halfSize),
                    new Vector2(-halfSize / 9f * 16, -halfSize),
                    new Vector2(halfSize / 9f * 16, -halfSize),
                    new Vector2(halfSize / 9f * 16, halfSize) };

                area.points = newArea;
            }



            if (cameraArea.cameraType == E_CameraType.LimitedspaceFreeCamera)
            {
                currentValue = limit.points;

                if (currentValue.Length != 4)
                {
                    Debug.LogError(string.Format("�г]�w{0}��v����{1}Collider�d��ѥ|���I�c��", cameraArea.gameObject.name, limit.gameObject.name));
                }
                else
                {
                    if (oldValue != null
                        && oldValue.Length != 0)
                    {
                        Vector2[] newArea = new Vector2[4];

                        for (int i = 0; i < 4; i++)
                        {
                            newArea[i] = area.points[i] + currentValue[i] - oldValue[i];

                            oldValue[i] = currentValue[i];
                        }

                        area.points = newArea;
                    }
                    else
                    {
                        oldValue = new Vector2[4];

                        for (int i = 0; i < 4; i++)
                        {
                            oldValue[i] = currentValue[i];
                        }
                    }
                }
            }
            else if (cameraArea.cameraType == E_CameraType.ConstantCamera)
            {
                limit.points = limitOrigin;
                //�]�\�o��Points[i]�O����ק諸�A�n�����ק�Points

                //area.points = areaOrigin;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 parentPos = transform.parent.position;

      
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (i % 2 == 0)
                {
                    Gizmos.color = Color.red;

                    Gizmos.DrawLine(parentPos + new Vector3(limit.points[j].x, limit.points[j].y), parentPos + new Vector3(limit.points[(j + 1 == limit.points.Length) ? 0 : j + 1].x, limit.points[(j + 1 == limit.points.Length) ? 0 : j + 1].y));

                }
                else
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(parentPos + new Vector3(area.points[j].x, area.points[j].y), parentPos + new Vector3(area.points[(j + 1 == area.points.Length) ? 0 : j + 1].x, area.points[(j + 1 == area.points.Length) ? 0 : j + 1].y));

                }
            }
        }
    }
}


