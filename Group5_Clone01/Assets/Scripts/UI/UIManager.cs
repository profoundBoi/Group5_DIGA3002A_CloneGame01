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
    public GameObject menuUI;


    [Header("Level UI Elements")]
    [Space(5)]

    public GameObject levelPanel;
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
            menuUI.SetActive(false);
            

        }

        if (toggle)
        {
            menuUI.SetActive(true);
            


        }


    }

    public void LoadLevelNumber(int _index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_index);
    }

}
