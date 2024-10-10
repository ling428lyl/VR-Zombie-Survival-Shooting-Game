using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabMagazine : MonoBehaviour
{
    public HandData rightHandPose;//pose of grabbing hand
    public HandData leftHandPose;//pose of grabbing hand

    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;

    private Vector3 startingHandScale;
    private Vector3 finalHandScale;

    private Quaternion finalHandRotation;
    private Quaternion startingHandRotation;

    
    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        //Debug.Log(grabInteractable);
        grabInteractable.selectEntered.AddListener(SetupPose);//when the user grabs the gun, set the hand pose as the grabbing one
        grabInteractable.selectExited.AddListener(UnSetPose);//when the user lets go the gun, set the hand pose as the original one
        rightHandPose.gameObject.SetActive(false);//not display in the scene
        leftHandPose.gameObject.SetActive(false);//not display in the scene
    }
    public void SetupPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRRayInteractor)//when the ray interactor detects the gun
        {
            HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();//the original hand model
            //Debug.Log(rightHandPose);
            //handData.gameObject.SetActive(false);
            //handData.animator.enabled = false;//freeze the original hand's animation
            //Debug.Log("11111");
            if (handData.handType == HandData.HandModeType.Right)
            {
                SetHandDataValues(handData, rightHandPose);//set the transform values
            }
            if (handData.handType == HandData.HandModeType.Left)
            {
                SetHandDataValues(handData, leftHandPose);//set the transform values
            }

            SetHandData(handData, finalHandPosition, finalHandRotation, finalHandScale);
        }
    }

    public void SetHandDataValues(HandData h1, HandData h2)//set the starting and final pose of the hands
    {
        /*
        startingHandPosition = new Vector3(h1.root.localPosition.x / h1.root.localScale.x, h1.root.localPosition.y / h1.root.localScale.y,
            h1.root.localPosition.z / h1.root.localScale.z);
        finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x, h2.root.localPosition.y / h2.root.localScale.y,
            h2.root.localPosition.z / h2.root.localScale.z);
        */
        startingHandPosition = h1.root.localPosition;
        finalHandPosition = h2.root.localPosition;

        startingHandRotation = h1.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        startingHandScale = h1.root.localScale;
        finalHandScale = h2.root.localScale;

        

    }

    //set the original hand's transform as the grabbing one
    public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
    {

        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;
        h.root.localScale = newScale;
        
    }


    public void UnSetPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRRayInteractor)
        {
            HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();

            //handData.gameObject.SetActive(true);
            handData.animator.enabled = true;

            SetHandData(handData, startingHandPosition, startingHandRotation, startingHandScale);
        }
    }
}
