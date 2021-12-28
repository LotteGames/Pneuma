using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public bool isCompelete = false;

    public bool directTrigger = false;

    public List<int> taskConditions;

    [Space(15)]
    public List<Sentence> sentences;

}
