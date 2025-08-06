﻿using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Input Fields")]
    [Space(5)]
    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputAction move;
    private TetherManager tether;



    [Header("Movement and Jumping")]
    [Space(5)]
    [SerializeField]
    private float maxSpeed;
    private bool isSprinting = false;
    [SerializeField]
    private float sprintMultiplier;
    private Vector3 forceDirection = Vector3.zero;
    [SerializeField]
    private float moveForce;
    [SerializeField]
    private float rotationSpeed = 5f;

    [Header("Slope Handling")]
    [Space(5)]

    private RaycastHit slopeHit;
    public float maxSlopeAngle;


    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private Transform cameraTransform;
    public bool isGrounded;
    public float playerHeight;
    public LayerMask layer;

    [Header ("Shooting Mechanic")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float bulletHitMiss = 25f;
    [SerializeField]
    private Transform bulletParent;

    [Header("Aiming")]
    [SerializeField]
    private CinemachineFreeLook freeLookCam;
    [SerializeField]
    private float aimingRange = 5f;
    [SerializeField]
    private Image reticleImage;
    private bool isAiming = false;
    public Color original;
    public Color targeted;



    [SerializeField]
    private int priorityBoostAmount = 10;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        inputAsset = GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");
        tether = GetComponent<TetherManager>();
        Cursor.lockState = CursorLockMode.Locked;
        reticleImage.enabled = false;

    }

    private void OnEnable()
    {
        player.FindAction("Jump").performed += Jump;
        player.FindAction("Shoot").performed += Shoot;
        player.FindAction("Sprint").performed += Speed;
        player.FindAction("Sprint").canceled += LimitSpeed;
        player.FindAction("Aim").performed += StartAim;
        player.FindAction("Aim").canceled += CancelAim;
        move = player.FindAction("Move");
        player.Enable();
    }

    private void StartAim(InputAction.CallbackContext context)
    {
        reticleImage.enabled = true;
        freeLookCam.Priority += priorityBoostAmount;
        isAiming = true;
  
        //combatFreelook.SetActive(true);
        //normalFreelook.SetActive(false);


    }

    private void CancelAim(InputAction.CallbackContext context)
    {
        reticleImage.enabled = false;
        freeLookCam.Priority -= priorityBoostAmount;
        isAiming = false;
        //combatFreelook.SetActive(false);
        //normalFreelook.SetActive(true);
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        BulletControl bulletControl = projectile.GetComponent<BulletControl>();
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            
            bulletControl.target = hit.point;
            bulletControl.hit = true;

            if (hit.collider.CompareTag("Dragonfly"))
            {
                Debug.Log("Hit Dragonfly!");

                // Heal the player
                Health playerHealth = GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(10f); 
                    Debug.Log("Healed player for 10 health");
                }

                
            }
            if (hit.collider.CompareTag("Bee"))
            {
                Debug.Log("Hit Bee!");

                // Heal the player
                Health playerHealth = GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(10f);
                    Debug.Log("Healed player for 10 health");
                }


            }

        }
        else
        {
            bulletControl.target = cameraTransform.position + cameraTransform.forward * bulletHitMiss;
            bulletControl.hit = false;
        }

        
        
    }

   

    private void Speed(InputAction.CallbackContext context)
    {
        isSprinting = true;
    }

    private void LimitSpeed(InputAction.CallbackContext context)
    {
        isSprinting = false;
    }

    private void OnDisable()
    {
        player.FindAction("Jump").performed -= Jump;
        player.FindAction("Shoot").performed -= Shoot;
        player.FindAction("Sprint").performed -= ctx => isSprinting = true;
        player.FindAction("Sprint").canceled -= ctx => isSprinting = false;
        player.Disable();
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, layer);

        // Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        // transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if (isAiming)
        {
            UpdateReticle();
        }
        else
        {
            // Reset color if not aiming
            reticleImage.color = Color.white;
        }

    }

    private void FixedUpdate()
    {
        Vector2 input = move.ReadValue<Vector2>();
        float currentSpeed = isSprinting ? moveForce * sprintMultiplier : moveForce;
        Vector3 rawMove = input.x * GetCameraRight(playerCamera) + input.y * GetCameraForward(playerCamera);
        rawMove *= currentSpeed;

        // Apply force normally
        forceDirection += rawMove;
        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if (OnSlope())
        {
            rb.AddForce(GetSlopeDirection() * currentSpeed * 20, ForceMode.Impulse);
        }

        // Extra gravity
        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        // Clamp max horizontal speed
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

        LookAt();

        rb.useGravity = !OnSlope();

        if (isAiming)
        {
            Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        

        
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }

    private Vector3 GetCameraForward(Camera cam)
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera cam)
    {
        Vector3 right = cam.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            //forceDirection += Vector3.up * jumpForce;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
           
            
            animator.SetTrigger("jumping");
        }
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeDirection()
    {
        return Vector3.ProjectOnPlane(forceDirection, slopeHit.normal).normalized;
    }

    private void UpdateReticle()
    {
        print("Change color");
        
        RaycastHit hit;
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * aimingRange, Color.red, 2f);
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, aimingRange))
        {
            Debug.Log("Hit: " + hit.collider.name);
            if (hit.collider.CompareTag("Dragonfly"))
            {
                
                reticleImage.color = Color.red; 
            }
            else if (hit.collider.CompareTag("Bee"))
            {

                reticleImage.color = Color.red;
            }
            else
            {
                reticleImage.color = Color.white;
            }
        }
        else
        {
            Debug.Log("Nothing hit");
            reticleImage.color = Color.white; 
        }

       

        
        


        
    }



}
