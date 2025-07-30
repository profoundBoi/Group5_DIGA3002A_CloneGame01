using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 5f;


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Dragonfly"))
        {
            health--;
        }
        else if (hit.gameObject.CompareTag("Snake"))
        {
            health--;
        }
        else if (hit.gameObject.CompareTag("Rock"))
        {
            health--;
        }

        
    }

    
  
}
