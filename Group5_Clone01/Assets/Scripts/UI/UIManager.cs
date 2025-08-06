using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private bool toggle;

    [Header("UI Elements")]
    [Space(5)]
    public GameObject controlUIPanel;
    public GameObject[] menuUIElements;
    public GameObject newFirstSelected;
    public GameObject menuFirstSelected;

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
            foreach (GameObject elem in menuUIElements)
            {
                elem.SetActive(true);
            }

            
            StartCoroutine(SetFirstSelected(menuFirstSelected));
        }

        if (toggle)
        {
            controlUIPanel.SetActive(true);
            foreach (GameObject elem in menuUIElements)
            {
                elem.SetActive(false);
            }

            // Set first selected in control panel
            StartCoroutine(SetFirstSelected(newFirstSelected));
        }
    }

    private IEnumerator SetFirstSelected(GameObject first)
    {
        yield return null; // Wait one frame to ensure UI is active
        EventSystem.current.SetSelectedGameObject(null); // Clear selection
        EventSystem.current.SetSelectedGameObject(first); // Set new selected
    }

    public void LoadLevelNumber(int _index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_index);
    }

}
