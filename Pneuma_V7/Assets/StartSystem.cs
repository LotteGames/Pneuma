using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSystem : MonoBehaviour
{
    public GameObject Menu;
    public GameObject MenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            //SceneManager.LoadScene(1);
            Menu.SetActive(true);
            MenuUI.SetActive(true);
            GetComponent<Animator>().enabled = true;
        }
    }

    public void ToMenu()
    {
        gameObject.SetActive(false);
    }
}
