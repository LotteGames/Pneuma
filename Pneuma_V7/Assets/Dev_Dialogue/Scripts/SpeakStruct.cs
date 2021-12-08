using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 包含人與話
/// </summary>
[Serializable]
public struct Speak
{
    public Role character;

    public string txt;

    public Speak(Role characterType, string txt)
    {
        this.character = characterType;
        this.txt = txt;
    }
}

/// <summary>
/// 包含地區、謎題編號跟Speaks
/// </summary>
[Serializable]
public struct PlaceSpeaks
{
    public int areaNum, puzzleNum;

    public PuzzleState puzzleState;

    public List<Speak> speaks;

    public PlaceSpeaks(int areaNum, int puzzleNum, List<Speak> speaks)
    {
        this.areaNum = areaNum;
        this.puzzleNum = puzzleNum;
        this.speaks = speaks;

        puzzleState = null;
    }

   
}
