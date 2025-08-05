using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private bool toggle;

    [Header("UI Elements")]
    [Space(5)]
    public GameObject controlUIPanel;
    public GameObject[] menuUIElements;

    private void Start()
    {
       
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }


    public void ControlPanel()
    {
        toggle = !toggle;

        if (toggle == false)
        {
            controlUIPanel.SetActive(false);
            foreach(GameObject elem in menuUIElements)
            {
                elem.SetActive(true);
            }
            

        }

        if (toggle)
        {
            controlUIPanel.SetActive(true);
            foreach (GameObject elem in menuUIElements)
            {
                elem.SetActive(false);
            }


        }


    }

    public void LoadLevelNumber(int _index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_index);
    }

}
