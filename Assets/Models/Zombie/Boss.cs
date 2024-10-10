using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    private double health = 150;
    private double lowerBodyDamage = 20;
    private double upperBodyDamage = 30;
    private double headDamage = 40;
    private float runSpeed = 1f;

    float attack_distance = 1.8f;

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

    private bool isShot = false;


    // Start is called before the first frame update
    void Start()
    {
        // state = ZombieState.Walking;
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("MainCamera");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 0;

        StartCoroutine(Routine());
        StartCoroutine(ZombieSound());

    }

    private IEnumerator Routine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.01f);

        while (true)
        {
            yield return wait;
            checkZombieStatus();
        }
    }

    private IEnumerator ZombieSound()
    {
        float randomNumber = Random.Range(3, 6);
        WaitForSeconds wait = new WaitForSeconds(randomNumber);

        while (true)
        {
            yield return wait;
            GetComponent<AudioSource>().Play();
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
                // agent.speed = runSpeed;
                ChangeState(ZombieState.Walking);
            }

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

        if(health < 50 && !isShot)
        {
            isShot = true;
            animator.SetBool("isShot", true);
        } else {
            animator.SetBool("isShot", false);
        }

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
        agent.speed = runSpeed;

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
        Destroy(gameObject, 10);
        StartCoroutine(ChangeToNewScene(8));
        
    }

    IEnumerator ChangeToNewScene(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene("NarrtiveOne");
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
