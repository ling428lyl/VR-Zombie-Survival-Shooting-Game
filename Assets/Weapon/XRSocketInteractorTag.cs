using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static GunTutorialManager;

public class XRSocketInteractorTag : XRSocketInteractor
{
    public string[] targetTags;
    public AudioClip objectInsertSound; // Assign via Inspector
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) // Ensure there's an AudioSource component
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource if not present
        }
        audioSource.playOnAwake = false;  // Ensure it doesn't start playing automatically
    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        // Call the base method and check if the object has a valid tag
        return base.CanSelect(interactable) && HasValidTag(interactable.gameObject);
    }

    private bool HasValidTag(GameObject obj)
    {
        foreach (var tag in targetTags)
        {
            if (obj.CompareTag(tag))
                return true;
        }
        return false;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (HasValidTag(args.interactableObject.transform.gameObject))
        {
            // Set the audio source to start from 0.5 seconds
            audioSource.time = 0.1f;

            // Play sound when the correct object is added to the socket
            if (objectInsertSound != null)
            {
                audioSource.clip = objectInsertSound;
                audioSource.Play();
            }
            if (GunTutorialManager.Instance.currentStep == TutorialStep.PlaceObjectInSocket)
            {
                GunTutorialManager.Instance.ObjectPlacedInSocket();
            }
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (HasValidTag(args.interactableObject.transform.gameObject))
        {
            // Set the audio source to start from 0.5 seconds
            audioSource.time = 0.1f;

            // Play sound when the correct object is removed from the socket
            if (objectInsertSound != null)
            {
                audioSource.clip = objectInsertSound;
                audioSource.Play();
            }
        }
    }
}