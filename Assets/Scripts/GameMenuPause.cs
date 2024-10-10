using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameMenuPause : MonoBehaviour
{
    // Start is called before the first frame update
    public InputActionReference pauseAction;
    //private bool lastsecondaryButtonState = false;
    public GameObject pauseMenu;
    //public gameobject mainCamera;
    public Transform xrRig; // Assign the XR Rig transform in the inspector
    public XRRayInteractor leftRayInteractor;
    public LineRenderer lineRenderer;
    public XRRayInteractor rightRayInteractor;
    public LineRenderer rightLineRenderer;
    void Start()
    {
        //StartCoroutine(WaitAndInitialize());
    }

    // IEnumerator WaitAndInitialize()
    // {
    //     int attempts = 0;
    //     while (attempts < 10)
    //     {
    //         List<InputDevice> devices = new List<InputDevice>();
    //         InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, devices);
    //         if (devices.Count > 0)
    //         {
    //             leftHandDevice = devices[0];
    //             Debug.Log("Left hand controller found.");
    //             yield break;
    //         }
    //         yield return new WaitForSeconds(1); // Wait for 1 second before trying again
    //         attempts++;
    //     }
    //     Debug.LogWarning("Left hand controller not found after several attempts.");
    // }

    // Update is called once per frame
    void Update()
    {
        if(pauseAction.action.WasPerformedThisFrame()) 
        {
            // if (pauseMenu.activeSelf) // 如果暂停菜单已经激活 
            // {
                if (Time.timeScale == 1)
                {
                Debug.Log("Pause Menu Activated");
                Time.timeScale = 0; // 暂停游戏时间
                leftRayInteractor.enabled = false; 
                lineRenderer.enabled = false; 
                rightLineRenderer.enabled = false;
                rightRayInteractor.enabled = false;
                //pauseMenu.transform.position = xrRig.position + xrRig.forward * 1 + xrRig.up * 1; // 1 meter in front of the player
                


                Transform mainCamera = xrRig.GetComponentInChildren<Camera>().transform;
                pauseMenu.transform.position = mainCamera.position + mainCamera.forward * 1; 
                pauseMenu.transform.rotation = Quaternion.LookRotation(pauseMenu.transform.position - mainCamera.position, Vector3.up);
                pauseMenu.SetActive(true);
                }
            else
            {
                Time.timeScale = 1; // Resume the game time
                leftRayInteractor.enabled = true;
                lineRenderer.enabled = true;
                rightLineRenderer.enabled = true;
                rightRayInteractor.enabled = true;
                pauseMenu.SetActive(false); 
            }
            //}
        }
    }
}
