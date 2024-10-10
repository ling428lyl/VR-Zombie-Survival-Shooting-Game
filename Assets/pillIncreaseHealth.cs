using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillIncreaseHealth : MonoBehaviour
{
    public PlayerController playerController;

    private AudioSource pillAudioSource;
    void Start()
    {
        pillAudioSource = GetComponent<AudioSource>();
    }
        void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pill")
        {
            playerController.RestoreHealth(50);
            pillAudioSource.Play();
            ChangePillToTrash(other.gameObject, "Trash");
        }
    }

    void ChangePillToTrash(GameObject pill, string newTag)
    {
        pill.tag = newTag;
        foreach (Transform child in pill.transform)
        {
            if (child.gameObject.tag == "Capsules")
            {
                Destroy(child.gameObject);
            }
            else
            {
                ChangePillToTrash(child.gameObject, newTag);
            }
            
        }
    }

}
