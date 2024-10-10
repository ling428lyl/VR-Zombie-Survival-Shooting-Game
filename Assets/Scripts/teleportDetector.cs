using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Assertions;

public class teleportDetector : MonoBehaviour
{
    // Start is called before the first frame update

    public TeleportationProvider teleportationProvider;
    public LocomotionSystem locomotion;
    
    public WatchEntered watchSocketEntered;

    private bool isCooldown = true;
    public float cooldownTime; // Set your desired cooldown time (in seconds)
    public AudioSource audioSource;
    public TeleportationArea teleportationArea;
    private float cooldownStartTime;

    public float remainingCooldownTime;
    //private bool shouldProcessTeleportRequest = true;

    

    void Start()
    {
        GameObject asylum = GameObject.Find("Asylum");
        if (asylum != null)
        {
            Transform dayRoomfloor = asylum.transform.Find("1stFloor/DayRoom/DayRoom_floor");
            if (dayRoomfloor != null)
            {
                teleportationArea = dayRoomfloor.gameObject.GetComponent<TeleportationArea>();
            }
            else
            {
                Debug.Log("dayroom not found");
            }
        }
        else
        {
            Debug.Log("Asylum not found");
        }

        teleportationProvider = GetComponent<TeleportationProvider>();
        audioSource = GetComponent<AudioSource>();
        locomotion = GetComponent<LocomotionSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(watchSocketEntered.watchTriggerEntered == true)
        {
            teleportationProvider.enabled = true;
            if (isCooldown)
            {
                if (teleportationProvider.locomotionPhase == LocomotionPhase.Done) // Check if the teleportation is done, and if the current request is not null, then play the sound and start the cooldown.
                {
                    audioSource.Play();
                    Debug.Log("Teleportation is done");
                    teleportationProvider.enabled = false;
                    locomotion.enabled = false;
                    teleportationArea.enabled = false;

                    isCooldown = false;

                    //shouldProcessTeleportRequest = false;
                    StartCoroutine(StartCooldown());

                }
            }
            else
            {
                remainingCooldownTime = cooldownStartTime + cooldownTime - Time.time; // Calculate the remaining cooldown time
                Debug.Log("Teleportation is on cooldown, cd: " + remainingCooldownTime.ToString("F1") + "seonds remaining");
                // here should have a sound where teleport failed.
                //teleportationProvider.QueueTeleportRequest(null); //

            }
        }
        if(isCooldown)
        {
           if(teleportationProvider.locomotionPhase == LocomotionPhase.Done) // Check if the teleportation is done, and if the current request is not null, then play the sound and start the cooldown.
            {
                audioSource.Play();
                Debug.Log("Teleportation is done");
                teleportationProvider.enabled = false;
                locomotion.enabled = false;
                teleportationArea.enabled = false;
                
                isCooldown = false;

                
                //shouldProcessTeleportRequest = false;
                StartCoroutine(StartCooldown());
                
            }
        }
        else
        {
            remainingCooldownTime = cooldownStartTime + cooldownTime - Time.time; // Calculate the remaining cooldown time
            //Debug.Log("Teleportation is on cooldown, cd: " + remainingCooldownTime.ToString("F1") + "seonds remaining");
            // here should have a sound where teleport failed.
            //teleportationProvider.QueueTeleportRequest(null); //
            
        }
    }

     IEnumerator StartCooldown() 
    {
        // Set the cooldown flag to prevent further actions during cooldown
        
        cooldownStartTime = Time.time;
        //locomotion.SetActive(false);
        //teleportationProvider.GetComponent<TeleportationProvider>().enabled = false; // Disable the action, so it can't be called again.

        // Wait for the specified cooldown time
        yield return new WaitForSeconds(cooldownTime); // Wait for the specified cooldown time, then continue with the rest of the method.
        //locomotion.SetActive(true);
        // Cooldown is over, reset the flag
        locomotion.enabled = true;
        teleportationProvider.enabled = true; // Enable the action, so it can be called again.
        teleportationArea.enabled = true;

        isCooldown = true;
        //shouldProcessTeleportRequest = false;
    }


}
