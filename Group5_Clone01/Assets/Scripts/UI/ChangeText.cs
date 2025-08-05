using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeText : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    public TextMeshProUGUI buttonTxt;
    public Color originalColor;
    public Color highlightColor;
    [SerializeField]
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonTxt = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void ChangeColour()
    {
     
        buttonTxt.color = highlightColor;

    }

    public void ChangeColourBack()
    {
        buttonTxt.color = originalColor;
    }

 
    public void OnSelect(BaseEventData eventData)
    {
        buttonTxt.color = highlightColor;
    }

   
    public void OnDeselect(BaseEventData eventData)
    {
        buttonTxt.color = originalColor;
    }
}
