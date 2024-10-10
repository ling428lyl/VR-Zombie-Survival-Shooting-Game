using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private double health = 50;
    private double lowerBodyDamage = 30;
    private double upperBodyDamage = 40;
    private double headDamage = 50;

    float attack_distance = 1.5f;

    // define the state of the zombie
    public enum ZombieState
    {
        Walking,
        Attacking,
        Stand,
        Dead
    }
    public enum DamagePosition
    {
        UpperBody,
        LowerBody,
        Head
    }

    ZombieState currentState = ZombieState.Stand;
    Animator animator;

    // get player object
    public GameObject player;
    public NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        // state = ZombieState.Walking;
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("MainCamera");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 0.3f;
        // agent.speed = 0f;

        StartCoroutine(Routine());
        StartCoroutine(ZombieSound());

    }

    private IEnumerator ZombieSound()
    {
        float randomNumber = Random.Range(8, 12); 
        WaitForSeconds wait = new WaitForSeconds(randomNumber);

        while (true)
        {
            yield return wait;
            GetComponent<AudioSource>().Play();
        }
    }

    private IEnumerator Routine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f); 

        while (true)
        {
            yield return wait;
            checkZombieStatus();
        }
    }

    // Update is called once per frame
    // void Update()
    void checkZombieStatus()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (currentState != ZombieState.Dead)
        {
            // if (distance <= attack_distance && currentState != ZombieState.Attacking)
            if (distance <= attack_distance)
            {
                // animator.SetBool("isMoving", false);
                FaceTarget();
                agent.speed = 0;
                ChangeState(ZombieState.Attacking);
            }
            else if (distance > attack_distance )
            {
                agent.speed = 0.3f;
                ChangeState(ZombieState.Walking);
            }

            // if (distance < attack_distance)
            // {
            //     FaceTarget();
            //     agent.speed = 0;
            // }

            // check if zombie is too close to the player
            if (distance < attack_distance - 0.3f)
            {
                KeepDistance();
                agent.speed = 0;
            }
        }


    }
    public void ChangeState(ZombieState newState)
    {
        // 在状态转换时执行一些额外逻辑
        switch (newState)
        {
            case ZombieState.Walking:
                Walk();
                break;
            case ZombieState.Attacking:
                Attack();
                agent.speed = 0;
                break;
            case ZombieState.Stand:
                // 添加 Stand 状态的逻辑
                break;
            case ZombieState.Dead:
                Die();
                break;
            default:
                break;
        }
        currentState = newState;
    }

    public bool TakeDamage(DamagePosition damagePosition, double damageRatio = 1.0)
    {
        double damageAmount = 0;
        Debug.Log("Zombie took damage of type: " + damagePosition);

        switch (damagePosition)
        {
            case DamagePosition.UpperBody:
                damageAmount = lowerBodyDamage * damageRatio;
                break;
            case DamagePosition.LowerBody:
                damageAmount = upperBodyDamage * damageRatio;
                break;
            case DamagePosition.Head:
                damageAmount = headDamage * damageRatio;
                break;
            default:
                Debug.LogWarning("Invalid damage type!");
                // should do error handling here
                return false;
        }

        health -= damageAmount;
        Debug.Log("Zombie health: " + health);

        if (health <= 0)
        {
            Debug.Log("Zombie died");
            Die();
            return true;
        }
        return false;
    }

    public void Attack()
    {
        if (currentState != ZombieState.Attacking)
        {
            agent.speed = 0;
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttack", true);
            currentState = ZombieState.Attacking;
            // StartCoroutine(WaitAndWalk(2));
        }
    }
    public void Walk()
    {

        animator.SetBool("isAttack", false);
        currentState = ZombieState.Stand;
        animator.SetBool("isMoving", true);
        currentState = ZombieState.Walking;

    }

    public void Stop()
    {
        animator.SetBool("isMoving", false);
        Attack();
    }

    public void Die()
    {
        // state = ZombieState.Dead;
        animator.SetBool("isDie", true);
        currentState = ZombieState.Dead;
        agent.speed = 0;
        // let animation end
        animator.SetBool("isEnd", true);
        // delete the zombie after 3 seconds
        Destroy(gameObject, 5);
    }

    void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void KeepDistance()
    {
        Vector3 directionToMove = (transform.position - player.transform.position).normalized;
        Vector3 targetPosition = player.transform.position + directionToMove * (attack_distance);
        transform.position = targetPosition;
    }

    public ZombieState GetState()
    {
        return currentState;
    }


}
