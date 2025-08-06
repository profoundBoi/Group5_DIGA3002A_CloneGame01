using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneName = "Sample Scene"; 
    public float delay = 3f;

    void Start()
    {
        Invoke("LoadScene", delay);
    }

    void LoadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
