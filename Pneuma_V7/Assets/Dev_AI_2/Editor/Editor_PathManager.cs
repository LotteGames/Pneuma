using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(PathManager))]
public class Editor_PathManager : Editor
{
    public override void OnInspectorGUI()
    {
        PathManager pathManager = (PathManager)target;

        base.OnInspectorGUI();

        pathManager.points = FindObjectsOfType<Point>();


        if (GUILayout.Button("Set Name"))
        {
            for (int i = 0; i < pathManager.points.Length; i++)
            {
                pathManager.points[i].SetName();
            }

            Debug.Log("Name Setting Done");
        }


        if (GUILayout.Button("Calculate Distance"))
        {
            for (int i = 0; i < pathManager.points.Length; i++)
            {
                for (int j = 0; j < pathManager.points[i].neighbors.Count; j++)
                {
                    if (pathManager.points[i].neighbors[j].point != null)
                    {
                        pathManager.points[i].GetInfo(pathManager.points[i].neighbors[j]);
                    }
                    else
                    {
                        Debug.Log(pathManager.points[i].gameObject.name + "'s " + j + " neighbor's point field is null");
                    }
                }
            }

            Debug.Log("Calculate Done");
        }

        #region--需再研究，包含Serilize的部分--

        for (int i = 0; i < pathManager.graphs.Count; i++)
        {
            if (pathManager.graphs[i] != null)
            {
                for (int j = 0; j < pathManager.graphs[i].points.Count; j++)
                {
                    EditorUtility.SetDirty(pathManager.graphs[i].points[j]);
                }
            }
        }

        #endregion
    }
}
