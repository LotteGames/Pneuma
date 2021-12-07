using System;
using TMPro;
using UnityEngine;

[Serializable]
public class CharacterType_and_Text
{
    public CharacterType characterType;

    public Canvas canvas;

    public TMP_Text tmp_Text;

    public GameObject dialogueObj;


    public void SetCanvasOrder(int order) 
    {
        canvas.sortingOrder = order;
    }
    public void SetDialogueActive(bool value)
    {
  
        dialogueObj.SetActive(value);
    }
}
