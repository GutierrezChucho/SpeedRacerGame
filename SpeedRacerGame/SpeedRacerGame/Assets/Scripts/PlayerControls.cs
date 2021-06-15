using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private PlayerStats playerStats;
    public Rigidbody rb;
    public GameObject itemSpawn;

    [Header("Movement")]
    [Range(0f, 8f)]
    public float forwardAccel = 4f;
    [Range(0f, 8f)]
    public float reverseAccel = 4f;
    [Range(0f, 16f)]
    public float gravity = 10f;
    public float maxSpeed = 50f, turnStrength = 180f;
    public float physicsModifier = 1000;
    public float speedModifier = 1;
    private float horizontalInput, verticalInput, rawVerticalInput;
    public bool forceStopMovement = false;

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

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Banana":
                SetSpeedModifer(0.5f);
                break;
            case "Killzone":
                StartCoroutine(DisableMovement());
                SetSpeedModifer(0);

                //Move player. (This code is probably more complex than needs be. Has to move both mesh and rigidbody separately)
                gameObject.transform.position = GameManager.Instance.respawnPoints[0].transform.position + Vector3.up;
                rb.velocity = Vector3.zero;
                rb.transform.position = GameManager.Instance.respawnPoints[0].transform.position + Vector3.up;
                break;
        }
    }

    void FixedUpdate()
    {
        isGrounded = false;
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, -transform.up, out hit, raycastLength, groundLayer))
        {
            isGrounded = true;
        }

        //Player turning (turning speed is quartered mid-air)
        if (!forceStopMovement)
        {
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
            //Update transform to the players rigidbody (sphere)
            transform.position = rb.position;

            if (Mathf.Abs(verticalInput) > 0)
            {
                rb.AddForce(transform.forward * verticalInput);
            }
        }

        //Simulate Gravity
        if (!isGrounded)
        {
            rb.AddForce(-Vector3.up * gravity * 50);
        }
    }

    public void SetSpeedModifer(float mod)
    {
        speedModifier = mod;
        StartCoroutine(ResetSpeedModifier(1));
    }

    //Resets speed modifier to neutral after duration
    IEnumerator ResetSpeedModifier(float duration)
    {
        yield return new WaitForSeconds(duration);
        speedModifier = 1;
    }

    //Disable player movement (at start of game and on respawn from killzone
    IEnumerator DisableMovement()
    {
        forceStopMovement = true;
        yield return new WaitForSeconds(3);
        forceStopMovement = false;
    }


    private void HandleInput()
    {
        //Reset input each frame
        horizontalInput = verticalInput = 0;

        //WSAS / Arrow keys. Two separate cases for both players
        switch (playerStats.playerID)
        {
            case 0:
                rawVerticalInput = Input.GetAxis("P1Vertical");
                if (Input.GetAxis("P1Vertical") > 0)
                {
                    verticalInput = Input.GetAxis("P1Vertical") * forwardAccel * physicsModifier * speedModifier;
                }
                else if (Input.GetAxis("P1Vertical") < 0)
                {
                    verticalInput = Input.GetAxis("P1Vertical") * reverseAccel * physicsModifier * speedModifier;
                }
                horizontalInput = Input.GetAxis("P1Horizontal");
                break;
            case 1:
                rawVerticalInput = Input.GetAxis("P2Vertical");
                if (Input.GetAxis("P2Vertical") > 0)
                {
                    verticalInput = Input.GetAxis("P2Vertical") * forwardAccel * physicsModifier * speedModifier;
                }
                else if (Input.GetAxis("P2Vertical") < 0)
                {
                    verticalInput = Input.GetAxis("P2Vertical") * reverseAccel * physicsModifier * speedModifier;
                }
                horizontalInput = Input.GetAxis("P2Horizontal");
                break;
        }

        //Use held item (if available)
        if (Input.GetKeyDown(useItem))
        {
            if (playerStats.heldItem != null)
            {
                playerStats.heldItem.UseItem(gameObject);
                playerStats.heldItem = null;
            }
        }
    }
}
