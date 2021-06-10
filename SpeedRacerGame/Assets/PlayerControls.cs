using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private PlayerStats playerStats;
    public Rigidbody rb;

    [Header("Movement")]
    [Range(0f, 8f)]
    public float forwardAccel = 4f;
    [Range(0f, 8f)]
    public float reverseAccel = 4f;
    [Range(0f, 16f)]
    public float gravity = 10f;
    public float maxSpeed = 50f, turnStrength = 180f;
    public float physicsModifier = 1000;
    private float horizontalInput, verticalInput, rawVerticalInput;

    [Header("Airborne")]
    public LayerMask groundLayer;
    public float raycastLength;
    public Transform rayOrigin;
    private bool isGrounded;

    [Header("Keybinds")]
    public KeyCode useItem;

    void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.transform.parent = null;

        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        isGrounded = false;
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, -transform.up, out hit, raycastLength, groundLayer))
        {
            isGrounded = true;
        }

        if (isGrounded)
        {
            Vector3 rotationOffset = new Vector3(0f, horizontalInput * turnStrength * rawVerticalInput * Time.deltaTime, 0f);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationOffset);
        }
        else
        {
            Vector3 rotationOffset = new Vector3(0f, horizontalInput * (turnStrength / 4) * rawVerticalInput * Time.deltaTime, 0f);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationOffset);
        }
        transform.position = rb.position;

        if(Mathf.Abs(verticalInput) > 0)
        {
            rb.AddForce(transform.forward * verticalInput);
        }

        if(!isGrounded)
        {
            rb.AddForce(-Vector3.up * gravity * 50);
        }
    }

    private void HandleInput()
    {
        horizontalInput = verticalInput = 0;

        switch(playerStats.playerID)
        {
            case 0:
                rawVerticalInput = Input.GetAxis("P1Vertical");
                if (Input.GetAxis("P1Vertical") > 0)
                {
                    verticalInput = Input.GetAxis("P1Vertical") * forwardAccel * physicsModifier;
                }
                else if(Input.GetAxis("P1Vertical") < 0)
                {
                    verticalInput = Input.GetAxis("P1Vertical") * reverseAccel * physicsModifier;
                }
                horizontalInput = Input.GetAxis("P1Horizontal");
                break;
            case 1:
                rawVerticalInput = Input.GetAxis("P2Vertical");
                if (Input.GetAxis("P2Vertical") > 0)
                {
                    verticalInput = Input.GetAxis("P2Vertical") * forwardAccel * physicsModifier;
                }
                else if (Input.GetAxis("P2Vertical") < 0)
                {
                    verticalInput = Input.GetAxis("P2Vertical") * reverseAccel * physicsModifier;
                }
                horizontalInput = Input.GetAxis("P2Horizontal");
                break;
        }

        if (Input.GetKeyDown(useItem))
        {
            if (playerStats.heldItem != null)
            {
                playerStats.heldItem.UseItem();
                playerStats.heldItem = null;
            }
        }
    }
}
