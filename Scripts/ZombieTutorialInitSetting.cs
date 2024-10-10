using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSetting : MonoBehaviour
{
    
    // input zombie prefab
    [SerializeField]
    public GameObject zombiePrefab;
    public GameObject bossPrefab;
    public Transform goal;
    UnityEngine.AI.NavMeshAgent agent;


    Animator animator;
    GameObject zombie1;
    GameObject zombie2;
    GameObject boss;
    GameObject player;
    GameObject respawnPoint;
    bool isActivated = false;
    void Start()
    {
        player = GameObject.FindWithTag("MainCamera");
        respawnPoint = GameObject.FindWithTag("Respawn");
        // create two zombies based on the prefab
        // zombie1 = Instantiate(zombiePrefab, new Vector3(8, 0, -9), Quaternion.identity);

        // zombie2 = Instantiate(zombiePrefab, new Vector3(0, 0, -9), Quaternion.identity);
        // create a boss based on the prefab
        // boss = Instantiate(bossPrefab, new Vector3(4, 0, -9), Quaternion.identity);
        // stop the animation of the boss
        Instantiate(zombiePrefab, new Vector3(-34.3f, 0.085f, 15.752f), Quaternion.identity);
        Instantiate(zombiePrefab, new Vector3(-24.84f, 0.085f, 22.98f), Quaternion.identity);
        Instantiate(zombiePrefab, new Vector3(-31.17f, 0.085f, 32.03f), Quaternion.identity);
        Instantiate(zombiePrefab, new Vector3(-16.89f, 0.085f, 18.2f), Quaternion.identity);
        Instantiate(zombiePrefab, new Vector3(-0.49f, 0.085f, 15.34f), Quaternion.identity);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceWithDesinatedPoint = Vector3.Distance(respawnPoint.transform.position, player.transform.position);
        if (distanceWithDesinatedPoint <= 8f && !isActivated)
        {

            // zombie1 = Instantiate(zombiePrefab, new Vector3(8, 0, -9), Quaternion.identity);

            zombie2 = Instantiate(zombiePrefab, new Vector3(0, 0, -9), Quaternion.identity);
            // create a boss based on the prefab
            boss = Instantiate(bossPrefab, new Vector3(4, 0, -9), Quaternion.identity);
            /*boss.GetComponent<Animator>().enabled = true;
            boss.GetComponent<Animator>().SetBool("isMoving", true);
            Boss bossScript = boss.GetComponent<Boss>();
            bossScript.agent.speed = 1.5f;
            bossScript.ChangeState(Boss.ZombieState.Walking);

            zombie1.GetComponent<Animator>().enabled = true;
            zombie1.GetComponent<Animator>().SetBool("isMoving", true);
            Zombie zombie1Script = zombie1.GetComponent<Zombie>();
            zombie1Script.agent.speed = 0.3f;
            zombie1Script.ChangeState(Zombie.ZombieState.Walking);

            zombie2.GetComponent<Animator>().enabled = true;
            zombie2.GetComponent<Animator>().SetBool("isMoving", true);
            Zombie zombie2Script = zombie2.GetComponent<Zombie>();
            zombie2Script.agent.speed = 0.3f;
            zombie2Script.ChangeState(Zombie.ZombieState.Walking); */
            isActivated = true;
        }
        
    }
}
