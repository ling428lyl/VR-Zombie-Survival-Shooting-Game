using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VacBoost : MonoBehaviour
{
    public teleportDetector teleportDetector;
    public PlayerController playerController;

    private AudioSource injectAudioSource;
    private GameObject vacs;
    
    void Start()
    {
        injectAudioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Vac")
        {
            vacs = other.gameObject;
            teleportDetector.cooldownTime -= 1.5f;
            playerController.IncreaseHealth(20);
            injectAudioSource.Play();
            ChangeVacToTrash(vacs.gameObject, "Trash");
            //StartCoroutine(VacInjecting());
        }
    }

    // IEnumerator VacInjecting()
    // {
    //     injectAudioSource.Play();
    //     ChangeVacToTrash(vacs.gameObject, "Trash");
    //     //yield return new WaitForSeconds(2.2f); //it was wanted to close XRGrabInteractable after 2.2 seconds. 
    // }

    void ChangeVacToTrash(GameObject vac, string newTag)
    {
        vac.tag = newTag;
        foreach (Transform child in vac.transform)
        {
            ChangeVacToTrash(child.gameObject, newTag);
        }
    }
}
