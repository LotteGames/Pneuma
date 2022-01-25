using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerProgress : MonoBehaviour
{
    public List<Task> tasks;
    //public List<Task> compeleteTasks;
    private void OnValidate()
    {
        if (Application.isPlaying == false)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].index = i;
            }
        }
    }

    public void SetTask(int index)
    {
        tasks[index].SetCompelete();
    }

    public bool GetIsCompelete(int index)
    {
        return tasks[index].isCompelete;
    }
}

[System.Serializable]
public class Task
{
    public string name;

    public int index;

    public bool isCompelete = false;

    public void SetCompelete()
    {
        isCompelete = true;
    }
}


