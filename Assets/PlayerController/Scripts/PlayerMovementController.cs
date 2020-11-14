using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovementController : MonoBehaviour
{
    // Start is called before the first frame update
   [Header("Movement Settings")]
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpForce;
    public SprintType sprintType;
    public float gravityScale = 1;
    public bool isInAir = false;
    //other Variables
    CharacterController charControl;
    [Tooltip("The transform which the movement is relative to, for typical fps movement select main camera. But it can be anything")]
    public Transform mainCamera;
    Vector2 pInput;

    public PlayerInputController playerInputController;
    float movementSpeed;
    float gravity;
    bool isSprinting = false;
    
    
   
    void Start()
    {
        charControl = GetComponent<CharacterController>();
        
        //playerInputController = FindObjectOfType<PlayerInputController>();
    }




    // Update is called once per frame
    private void Update()
    {

        #region Jump
        if (isInAir == false)
        {
            if (playerInputController.inputActions.Player.Jump.triggered)
            { 
            gravity = jumpForce * Time.deltaTime;
            isInAir = true;
            }
        }
        #endregion
        
        #region sprinting
        if (isSprinting)
        {
            movementSpeed = sprintSpeed;
        }
        else
        {
            movementSpeed = walkSpeed;
        }
        switch(sprintType)
        {
            case SprintType.clickToSprint:
                if(playerInputController.inputActions.Player.Sprint.triggered)
                isSprinting = !isSprinting;    
                break;
            case SprintType.holdToSprint:
                playerInputController.inputActions.Player.Sprint.performed += sprint => isSprinting = true;
                playerInputController.inputActions.Player.Sprint.canceled += sprint => isSprinting = false;
               
                break;

        }
       
        #endregion
        
        //set y rotation = camera y rotation
        transform.rotation = Quaternion.Euler(new Vector3(0, mainCamera.eulerAngles.y, 0));
       
        pInput = playerInputController.inputActions.Player.Move.ReadValue<Vector2>();
        
        charControl.Move(transform.right * pInput.x * movementSpeed * Time.deltaTime + transform.forward * pInput.y * movementSpeed * Time.deltaTime + gravity * transform.up * Time.deltaTime);
    }
    private void FixedUpdate()
    {
       
        if (charControl.isGrounded)
        {
            isInAir = false;
        }
        else
        {
            isInAir = true;
            gravity += gravityScale*Physics.gravity.y*Time.deltaTime;
        }
      
    }
    public enum SprintType
    {
        clickToSprint,
        holdToSprint
    }
  
}
