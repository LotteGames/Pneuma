using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// �]�t�H�P��
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
/// �]�t�a�ϡB���D�s����Speaks
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
