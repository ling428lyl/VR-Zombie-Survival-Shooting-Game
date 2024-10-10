using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FixedWatchPosition : MonoBehaviour
{
    public XRGrabInteractable watchGrabInteractable;
    void WatchFixed()
    {
        watchGrabInteractable.enabled = false;
    }
}
