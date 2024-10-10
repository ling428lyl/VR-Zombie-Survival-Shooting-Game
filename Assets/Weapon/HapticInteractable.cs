using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticInteractable : MonoBehaviour
{
    [Range(0, 1)]
    public float intensity;
    public float duration;

    private Magazine magazine; // Reference to the current Magazine component
    [SerializeField] private SimpleShoot simpleShoot; // Reference to the SimpleShoot script

    void Awake()
    {
        // Initialize the magazine reference on awake
        UpdateMagazineReference();
    }

    private void Start()
    {
        // Listen to activated event on the XR interactable
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        if (interactable == null)
        {
            Debug.LogError("XRBaseInteractable component not found on " + gameObject.name);
        }
        else
        {
            interactable.activated.AddListener(TriggerHaptic);
        }
    }

    void UpdateMagazineReference()
    {
        // Attempt to find a Magazine component in the child objects
        magazine = GetComponentInChildren<Magazine>();
        if (magazine == null)
        {
            Debug.LogWarning("Magazine component not found on " + gameObject.name);
        }
    }

    public void TriggerHaptic(ActivateEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor &&
            magazine != null &&
            magazine.numberOfBullets > 0 &&
            simpleShoot != null &&
            simpleShoot.hasSlide) // Check hasSlide is true
        {
            TriggerHaptic(controllerInteractor.xrController);
        }
    }

    public void TriggerHaptic(XRBaseController controller)
    {
        if (intensity > 0 && magazine != null && magazine.numberOfBullets > 0)
        {
            controller.SendHapticImpulse(intensity, duration);
        }
    }

    // Call these from wherever magazines are changed
    public void OnMagazineInserted(Magazine newMagazine)
    {
        magazine = newMagazine;
    }

    public void OnMagazineRemoved()
    {
        magazine = null;
    }
}
