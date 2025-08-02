using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] public FPSController playerFPSController;
    [SerializeField] public GameObject inventory;
    float lookSpeed;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        //Store player variables
        lookSpeed = playerFPSController.lookSpeed;
        //Cursor Lock
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        //Player lock on
        playerFPSController.lookSpeed = 0;
    }
    private void OnDisable()
    {
        //Cursor Lock
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Player lock off
        playerFPSController.lookSpeed = lookSpeed;
    }
}
