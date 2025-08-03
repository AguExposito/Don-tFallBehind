using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera playerCamera;
    [SerializeField] SettingsMenu settingsMenu;


    [Space]
    [Header("Inputs")]
    [SerializeField] InputActionReference runInput;
    [SerializeField] InputActionReference jumpInput;
    [SerializeField] InputActionReference menuInput;
    [SerializeField] InputActionReference inventoryInput;
    [SerializeField] InputActionReference itemChange;

    [Space]
    [Header("Movement Variables")]
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    [SerializeField] public float jumpPower;
    [SerializeField] float gravity;
    [SerializeField] public bool alteredMovement;

    [Space]
    [Header("Camera Variables")]
    [SerializeField] public float lookSpeed;
    [SerializeField] float lookXLimit;

    [Space]
    [Header("State Variables")]
    [SerializeField] bool canMove = true;

    [Space]
    [Header("Read Only Variables"), ReadOnly]
    [SerializeField] Vector3 moveDirection = Vector3.zero;
    [SerializeField] float rotationX = 0;
    [SerializeField] CharacterController characterController;
    [SerializeField] HudController hudController;
    [SerializeField] MenuController screenUI;

    void Start()
    {
        hudController = transform.Find("CanvasHUD").GetComponent<HudController>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        screenUI = transform.GetComponentInChildren<MenuController>(true);
    }

    void Update()
    {
        #region Handles UI events
            #region Menu
        if (menuInput.action.WasPerformedThisFrame()) {
            if (screenUI.gameObject.activeInHierarchy)
            {
                if (settingsMenu.gameObject.activeInHierarchy)
                {
                    settingsMenu.gameObject.SetActive(false); //Deactivates menu on input
                }
                else
                {
                    screenUI.gameObject.SetActive(false); //Deactivates menu on input
                    screenUI.transform.GetChild(0).Find("Menu").gameObject.SetActive(false);
                    screenUI.transform.GetChild(0).Find("Inventory").gameObject.SetActive(false); //Also deactivates inventory ui
                }
            }
            else
            {
                screenUI.gameObject.SetActive(true); //Activates menu on input
                screenUI.transform.GetChild(0).Find("Menu").gameObject.SetActive(true);
            }
        }
        #endregion
            #region Inventory
        if (inventoryInput.action.WasPerformedThisFrame() && !screenUI.transform.GetChild(0).Find("Menu").gameObject.activeInHierarchy) //Checks that menu is not active 
        {
            if (screenUI.gameObject.activeInHierarchy)
            {
                screenUI.gameObject.SetActive(false); //Deactivates inventory on input
                screenUI.transform.GetChild(0).Find("Inventory").gameObject.SetActive(false);
            }
            else
            {
                screenUI.gameObject.SetActive(true); //Activates inventory on input
                screenUI.transform.GetChild(0).Find("Inventory").gameObject.SetActive(true);
            }
        }
        #endregion
        #endregion

        #region Handles Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = runInput.action.inProgress;
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        #endregion

        #region Handles Jumping
        if (jumpInput.action.inProgress && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        #endregion

        #region Handles Weapon Change
        float scrollValue = itemChange.action.ReadValue<float>();
        if (Mathf.Abs(scrollValue)>0.1f) {
            
            if (scrollValue > 0)
            {
               // Cicla hacia adelante
            }
            else
            {
               // Cicla hacia atras
            }

            OnItemChange(); // Llamamos la funci�n para actualizar el cambio
        }
        
        #endregion
    }
    public void ChangeMovementVariables(float walkSpeed, float runSpeed, float jumpPower, bool alteredMovement) { 
        this.walkSpeed = walkSpeed;
        this.runSpeed = runSpeed;
        this.jumpPower = jumpPower;
        this.alteredMovement = alteredMovement;
    }
    void OnItemChange() {
        hudController.UpdateHudValues();
    }
    private void OnEnable()
    {
        jumpInput.action.Enable();
        runInput.action.Enable();
        menuInput.action.Enable();
        inventoryInput.action.Enable();
        itemChange.action.Enable();
    }
    private void OnDisable()
    {
        jumpInput.action.Disable();
        runInput.action.Disable();
        menuInput.action.Disable();
        inventoryInput.action.Disable();
        itemChange.action.Disable();
    }
}