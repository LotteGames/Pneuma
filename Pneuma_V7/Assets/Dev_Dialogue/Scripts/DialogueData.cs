using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
[System.Serializable]
public class DialogueData
{
    [SerializeField]
    CharacterType charactertype;
    public CharacterType CHARACTERTYPE { get { return charactertype; } set { charactertype = value; } }

    [SerializeField]
    int area;
    public int Area { get { return area; } set { area = value; } }

    [SerializeField]
    TxtTypes txttypes;
    public TxtTypes TXTTYPES { get { return txttypes; } set { txttypes = value; } }

    [SerializeField]
    int puzzle;
    public int Puzzle { get { return puzzle; } set { puzzle = value; } }

    [SerializeField]
    int sequence;
    public int Sequence { get { return sequence; } set { sequence = value; } }

    [SerializeField]
    string ch_charactername;
    public string Ch_Charactername { get { return ch_charactername; } set { ch_charactername = value; } }

    [SerializeField]
    string ch_sentence;
    public string Ch_Sentence { get { return ch_sentence; } set { ch_sentence = value; } }

    [SerializeField]
    string eng_charactername;
    public string Eng_Charactername { get { return eng_charactername; } set { eng_charactername = value; } }

    [SerializeField]
    string eng_sentence;
    public string Eng_Sentence { get { return eng_sentence; } set { eng_sentence = value; } }

}
