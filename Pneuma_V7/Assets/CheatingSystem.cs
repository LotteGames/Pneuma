using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatingSystem : MonoBehaviour
{
    [Header("貓迷程式碼")]
    public CatContrl Cat;

    [Header("各個區域")]
    public GameObject[] pos_;

    // Start is called before the first frame update
    void Start()
    {
        Cat = GameObject.FindObjectOfType<CatContrl>();
    }

    public void CatDie()
    {
        Cat.GetCatDie();
    }
    public void GoScense(int Where)
    {
        SceneManager.LoadScene(Where);
    }
    public void GoToPos(int Where)
    {
        Cat.GetCatDie();
        PlayerPrefs.SetFloat("CatPos_X", pos_[Where].transform.position.x);
        PlayerPrefs.SetFloat("CatPos_Y", pos_[Where].transform.position.y);
    }
}
