using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchEntered : MonoBehaviour
{
    public bool watchTriggerEntered { get; private set; }

    void OnTriggerEnter(Collider other)
    {
        watchTriggerEntered = true;
    }

    // void OnTriggerExit(Collider other)
    // {
    //     IsTriggerEntered = false;
    // }
}
