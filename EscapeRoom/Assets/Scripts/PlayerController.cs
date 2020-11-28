using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    InventoryController inventoryController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private Vector3 moveDirection = Vector3.zero;

    //singleton instance of player
    public static GameObject mainPlayer { get; private set; }

    public bool freezed = false;
    

    void Start()
    {
        mainPlayer = gameObject;
        characterController = GetComponent<CharacterController>();
        inventoryController = GetComponent<InventoryController>();
    }

    void Update()
    {

        if (Input.GetKeyDown("i") && !freezed)
        {
            inventoryController.ToggleInventory();
        }

        if (Input.GetKeyDown("e") && !inventoryController.active)
        {
            inventoryController.Interaction();
        }

        if (Input.GetMouseButtonDown(0) && !inventoryController.active && !freezed)
        {
            inventoryController.Fire();
        }

        if (!inventoryController.active && !freezed)
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

            if (characterController.isGrounded)
            {

                moveDirection = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
                moveDirection.y = 0;
                moveDirection *= speed;

            }

            moveDirection.y -= gravity * Time.deltaTime;

            characterController.Move(moveDirection * Time.deltaTime);
        }


    }
}
