using System.Collections;
using System.Collections.Generic;
using System.Threading;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class FlashTeleported : MonoBehaviour
{
    public InputActionReference teleportActivated;
    public AudioSource audioSource;
    //public TeleportationProvider teleportationProvider;
    public GameObject locomotion;
    public float cooldownTime; // Set your desired cooldown time (in seconds)
    private bool isCooldown = false; 

    //public TeleportationProvider teleportationProvider;
    //public InputActionProperty testActionProperty;
    //public InputActionMap testActionMap;
    //public InputActionAsset testActionAsset;

    private InputAction teleportActivatedAction; // This will store the action from the reference.
    void Start()
    {
        audioSource = GetComponent<AudioSource>();


       //teleportationProvider = GetComponent<TeleportationProvider>();

        teleportActivatedAction = teleportActivated.action; // Get the action from the reference.
        teleportActivatedAction.canceled += ActivateBehavior; // Add a listener to the action, which will call ActivateBehavior when the action is performed.
                                                              //teleportActivatedAction.performed += ActivateEvents;

    }


    private void ActivateBehavior(InputAction.CallbackContext context) // This is the method that will be called when the action is performed.
    {

        if (isCooldown == false)
        {
            
            audioSource.Play();

        }
        StartCoroutine(StartCooldown()); 

        //isCooldown = true; teleportActivatedAction = null; // Set the cooldown flag to prevent further actions during cooldown, and set the action to null to prevent it from being called again.


    }

    IEnumerator StartCooldown() 
    {
        // Set the cooldown flag to prevent further actions during cooldown
        isCooldown = true;

        locomotion.SetActive(false);
        //teleportationProvider.GetComponent<TeleportationProvider>().enabled = false; // Disable the action

        // Wait for the specified cooldown time
        yield return new WaitForSeconds(cooldownTime); // Wait for the specified cooldown time, then continue with the rest of the method.
        locomotion.SetActive(true);
        // Cooldown is over, reset the flag
        //teleportationProvider.GetComponent<TeleportationProvider>().enabled = true; // Enable the action, so it can be called again.
        isCooldown = false;
        

    }
}
