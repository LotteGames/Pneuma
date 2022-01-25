using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Sentence
{
    public Role speaker;

    [TextArea(3,10)]
    public string text;
}
