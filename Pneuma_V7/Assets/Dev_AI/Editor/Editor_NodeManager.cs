using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(NodeManager))]
public class Editor_NodeManager : Editor
{
    public override void OnInspectorGUI()
    { 
        NodeManager nodeManager = (NodeManager)target;

        if (GUILayout.Button("Get Nodes On Scene"))
        {
            Debug.Log(4);
            nodeManager.Nodes = FindObjectsOfType<Node>();

            for (int i = 0; i < nodeManager.Nodes.Length; i++)
            {
                nodeManager.Nodes[i].NodeNum = i;
                nodeManager.Nodes[i].SetName();
            }
            Debug.Log("Get Nodes");
        }

        base.OnInspectorGUI();


        if (GUILayout.Button("Calculate Distance"))
        {
            for (int i = 0; i < nodeManager.Nodes.Length; i++)
            {
                nodeManager.Nodes[i].GetNeighborsDistance();
            }
            Debug.Log("Finish Calculate");
        }

        if (GUILayout.Button("Get Path"))
        {
            nodeManager.GetPath(nodeManager.node_Start, nodeManager.node_End);
        }

        if (GUILayout.Button("Clear Message"))
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
        #region--需再研究，包含Serilize的部分--
        foreach (Node node in nodeManager.Nodes)
        {
            EditorUtility.SetDirty(node);
        }
        #endregion
    }
}
