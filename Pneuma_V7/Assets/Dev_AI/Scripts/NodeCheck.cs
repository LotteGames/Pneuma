using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class NodeCheck : MonoBehaviour
{
    public NodeManager nodeManager;
    public Node currentNode;

    public UnityEvent event_NodeChaged;
    void Start()
    {
        currentNode = nodeManager.GetNode(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Node node = nodeManager.GetNode(transform.position);

        if (node != currentNode)
        {
            currentNode = node;
            event_NodeChaged.Invoke();
        }
    }
}
