using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crab : MonoBehaviour
{
    public GameObject interactPrompt;  
    public GameObject winPanel;       
    public float interactDistance = 2f;

    private GameObject nearestCrab;
    private TetherManager tetherManager;
    private bool hasWon = false;

    void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        tetherManager = GetComponent<TetherManager>();
    }

    void Update()
    {
        if (hasWon) return;

        FindNearestCrab();

        if (nearestCrab != null)
        {
            float distance = Vector3.Distance(transform.position, nearestCrab.transform.position);
            if (distance <= interactDistance)
            {
                interactPrompt.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactPrompt.SetActive(false);

                    if (tetherManager != null)
                        tetherManager.enabled = false;

                    StartCoroutine(ShowWinPanelAfterDelay());
                    hasWon = true;
                }
            }
            else
            {
                interactPrompt.SetActive(false);
            }
        }
        else
        {
            interactPrompt.SetActive(false);
        }
    }

    void FindNearestCrab()
    {
        GameObject[] crabs = GameObject.FindGameObjectsWithTag("Crab");
        float minDistance = Mathf.Infinity;
        nearestCrab = null;

        foreach (GameObject crab in crabs)
        {
            float dist = Vector3.Distance(transform.position, crab.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearestCrab = crab;
            }
        }
    }

    IEnumerator ShowWinPanelAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        if (winPanel != null)
            winPanel.SetActive(true);
    }
}

