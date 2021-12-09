using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PointCheck : MonoBehaviour
{
    private PathManager pathManager;

    public UnityEvent PointChanged;

    private Point lastPoint;

    public bool isActive = true;

    void Start()
    {
        pathManager = FindObjectOfType<PathManager>();
    }

    void Update()
    {
        Point point = pathManager.GetPoint(transform.position);

        if (point != lastPoint && isActive)
        {
            lastPoint = point;
            PointChanged.Invoke();
        }
    }

    public void SetActive(bool value)
    {
        isActive = value;
    }
}
