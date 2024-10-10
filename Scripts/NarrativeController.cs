using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeController : MonoBehaviour
{
    //public GameObject narrativeController;
    public AudioSource[] audioSources;
    public Animator animatorSmith;
    private BoxCollider narrativeCollider;

    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        narrativeCollider = GetComponent<BoxCollider>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainCamera")
        {
            StartCoroutine(PlayNarratives());
            
        }
    }

    IEnumerator PlayNarratives()
    {
        animatorSmith.SetBool("isWavingRight", true);
        audioSources[0].Play();
        yield return new WaitForSeconds(7.8f); //might need to play walk animation, so longer.
        animatorSmith.SetBool("isWavingRight", false);
        audioSources[1].Play();
        animatorSmith.SetBool("isWalkInCircle", true);
        yield return new WaitForSeconds(11);
        audioSources[2].Play(); 
        // while(animatorSmith.GetCurrentAnimatorStateInfo(0).IsName("WalkInCircle"))
        // {
        //     yield return null;
        // }
        yield return new WaitForSeconds(7.8f);
        animatorSmith.SetBool("isWalkInCircle", false);
        animatorSmith.SetBool("rightTurn", true);
        yield return new WaitForSeconds(11.75f);
        animatorSmith.SetBool("rightTurn", false);
        audioSources[3].Play();
        yield return new WaitForSeconds(17f);
        animatorSmith.SetBool("isTurn110", true);
        audioSources[4].Play();
        animatorSmith.SetBool("isFastWalk", true);
        yield return new WaitForSeconds(5f);
        animatorSmith.SetBool("isTurn110", false);
        narrativeCollider.enabled = false;
    }
//     IEnumerator WalkInCirclePtOne()
//     {
//         animatorSmith.SetBool("isWalkinCircle", true);
//         yield return new WaitForSeconds(22.1f); //might need to play walk animation, so longer.
// a       nimatorSmith.SetBool("isWalkinCircle", false);
//     }

    // Update is called once per frame
}
