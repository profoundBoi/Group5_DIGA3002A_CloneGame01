using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private float maxSpeed = 5f; //Remember to always keep the max Speed here the same as the one in PlayerMovement...
    private PlayerMovement playerMovement;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.isGrounded)
        {
            animator.SetFloat("speed", rb.velocity.magnitude / maxSpeed);
        }
        
    }
}
