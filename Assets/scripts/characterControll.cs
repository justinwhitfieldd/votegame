using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotateSpeed = 300f;
    public Animator animator;
    public Camera mainCamera;
    private Vector3 movement;
    private float mouseX;
    public bool isBlocking = false;
    public AttributesManager attribute;
    private CharacterController controller;
    public bool isAttacking = false;
    public bool contactMade = false;
    public bool canMove = true;
    public HealthBar player;
    public HealthBar enemy;
    public EnemyController enemyfighter;
    public PauseMenu menu;
    private float fixedYPosition;
    private void Start()
    {
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        fixedYPosition = transform.position.y;
        // Get the CharacterController component attached to this object
        controller = GetComponent<CharacterController>();
    }
    public void cursorOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void cursorOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ResetPlayerAttacking()
    {
        isAttacking = false;
        contactMade = false;
    } 
    public void ToggleIsPunching()
    {
        attribute.PlayerIsPunching = false;
    }
    private void Update()
    {
        if(player.slider.value <= 0)
        {
            enemyfighter.canMove = false;
            animator.SetTrigger("knockout");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            menu.GameOverLose();
        }
        if(enemy.slider.value <= 0)
        {
            enemyfighter.canMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            menu.GameOverWin();
        }
        if(canMove)
        {
            // Get input for movement
            float moveVertical = Input.GetAxis("Vertical");
            float moveHorizontal = Input.GetAxis("Horizontal");
            
            // Calculate movement vector relative to the camera's forward direction
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            movement = cameraForward * moveVertical + cameraRight * moveHorizontal;

            animator.SetBool("walkForward", moveVertical > 0f);
            //animator.SetBool("walkBackward", moveVertical < 0f);
            //animator.SetBool("walkRight", moveHorizontal > 0f);
            //animator.SetBool("walkLeft", moveHorizontal < 0f);
            animator.SetBool("idle", moveVertical == 0f && moveHorizontal == 0f);

            // Move the character using the CharacterController
            controller.Move(movement * moveSpeed * Time.deltaTime);
        Vector3 currentPosition = transform.position;
        currentPosition.y = fixedYPosition;
        transform.position = currentPosition;
            // Get mouse input for rotation
            mouseX = Input.GetAxis("Mouse X") * rotateSpeed;

            // Rotate the player object based on mouse movement
            transform.Rotate(Vector3.up, mouseX * Time.deltaTime);

            // Set the "isMoving" parameter in the animator based on movement input
            // animator.SetBool("isMoving", moveVertical != 0f || moveHorizontal != 0f);

            // Check if the left mouse button is clicked
            if (Input.GetMouseButtonDown(0))
            {
                // Trigger the "attack" animation
                isAttacking = true;
                attribute.PlayerIsPunching = true;
                animator.SetTrigger("Attack");
            }
            if (Input.GetMouseButtonDown(1))
            {
                // Trigger the "attack" animation
                isBlocking = true;
                animator.SetBool("block",true);
            }
            if (Input.GetMouseButtonUp(1)){
                isBlocking = false;
                animator.SetBool("block",false);
            }
        }
    }
}