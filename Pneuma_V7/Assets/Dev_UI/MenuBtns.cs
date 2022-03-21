using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuBtns : MonoBehaviour
{
    public void LoadGameScene(int index) 
    {
        SceneManager.LoadScene(index);
    }
}
