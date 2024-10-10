using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHitDetection : MonoBehaviour
{
    private bool canBeAttacked = true;
    private Image hurtBackgroundImage;
    private GameObject player;
    private int userHealth;
    // Start is called before the first frame update
    void Start()
    {
        hurtBackgroundImage = GameObject.FindWithTag("HurtBackground").GetComponent<Image>();
        player = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        // userHealth = player.GetComponent<PlayerController>().getUserHealth();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieComponentFinger") && canBeAttacked)
        {
            // get the zombie object
            Boss boss = other.transform.root.GetComponent<Boss>();
            // check the zombie status
            if(boss != null){
                if (boss.GetState() == Boss.ZombieState.Attacking)
                {
                    Debug.Log("Zombie is attacking");
                    // decrease the user health by 10
                    var color = hurtBackgroundImage.color;
                    color.a = 0.8f;
                    hurtBackgroundImage.color = color;
                    userHealth = player.GetComponent<PlayerController>().getUserHealth();
                    userHealth -= 20;
                    Debug.Log("User health: " + userHealth);
                    player.GetComponent<PlayerController>().setUserHealth(userHealth);
                    player.GetComponent<AudioSource>().Play();
                    if (userHealth <= 0)
                    {
                        Debug.Log("Game over");
                    }
                }
                StartCoroutine(DisableAttacksForDuration(4f));
            }

            
        }

    }
    private IEnumerator DisableAttacksForDuration(float duration)
    {
        // Prevent further attacks for the specified duration
        canBeAttacked = false;
        yield return new WaitForSeconds(duration);
        canBeAttacked = true;
    }
}
