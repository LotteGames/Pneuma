using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Struct_Speak
{
    public CharacterType character;

    public string txt;

    public Struct_Speak(CharacterType characterType, string txt)
    {
        this.character = characterType;
        this.txt = txt;
    }
}
[Serializable]
public struct Struct_PlaceSpeak
{
    public int areaNum, puzzleNum;

    public PuzzleState puzzleState;

    public List<Struct_Speak> struct_Speaks;

    public Struct_PlaceSpeak(int areaNum, int puzzleNum, List<Struct_Speak> struct_Speaks)
    {
        this.areaNum = areaNum;
        this.puzzleNum = puzzleNum;
        this.struct_Speaks = struct_Speaks;

        puzzleState = null;
    }

   
}
