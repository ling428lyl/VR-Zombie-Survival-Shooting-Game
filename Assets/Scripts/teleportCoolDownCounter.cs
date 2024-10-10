using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class teleportCoolDownCounter : MonoBehaviour
{
    public TextMeshProUGUI _text;
    public teleportDetector _teleportDetector;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        // Assuming the GameObject with the teleportDetector script is named "TeleportDetectorObject"
        GameObject detectorObject = GameObject.Find("XR Origin (XR Rig)/Locomotion System");
        if (detectorObject != null)
        {
            _teleportDetector = detectorObject.GetComponent<teleportDetector>();
        }
        else{
            Debug.LogError("TeleportDetector not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_teleportDetector != null)
        {
            _text.text = _teleportDetector.remainingCooldownTime.ToString("F1");
        }
    }

    // private void UpdateText(float remainingTime)
    // {
    //     _text.text = remainingTime.ToString("F1");
    // }
}
