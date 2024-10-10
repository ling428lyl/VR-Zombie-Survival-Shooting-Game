using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera mainCamera;
    private int userPoints = 0;
    private int userHealth = 100;

    public int maxHealth;

    private Image hurtBackgroundImage;
    private GameObject restartCanvas;
    private GameObject hurtCanvas;

    private bool canBeAttacked = true;
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        hurtBackgroundImage = GameObject.FindWithTag("HurtBackground").GetComponent<Image>();
        restartCanvas = GameObject.FindWithTag("RestartCanvas");
        restartCanvas.SetActive(false);
        // hurtCanvas = GameObject.FindWithTag("HurtCanvas");
        maxHealth = userHealth;
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            // 发射一条射线，从相机位置通过鼠标位置
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Zombie zombie = hit.collider.transform.root.GetComponent<Zombie>();
                // Debug.Log("Hit: " + zombie.gameObject.name);
                bool zombieDie = false;

                if (hit.collider.CompareTag("ZombieComponentHead"))
                {
                    Debug.Log("Hit the head");
                    zombieDie = zombie.TakeDamage(Zombie.DamagePosition.Head);
                }
                else if (hit.collider.CompareTag("ZombieComponentUpper"))
                {
                    Debug.Log("Hit the upper body");
                    zombieDie = zombie.TakeDamage(Zombie.DamagePosition.UpperBody);
                }
                else if (hit.collider.CompareTag("ZombieComponentLower"))
                {
                    Debug.Log("Hit the lower body");
                    zombieDie = zombie.TakeDamage(Zombie.DamagePosition.LowerBody);
                }
                else
                {
                    return;
                }
                // if the zombie is dead, add points
                if (zombieDie == true)
                {
                    userPoints += 10;
                    Debug.Log("User points: " + userPoints);
                }
            }
        }

        // if hurtCanvas is active, decrease the alpha value
        if (hurtBackgroundImage != null)
        {
            if (hurtBackgroundImage.color.a > 0 && userHealth > 0)
            {
                var color = hurtBackgroundImage.color;
                color.a -= 0.01f;
                hurtBackgroundImage.color = color;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieComponentFinger") && canBeAttacked)
        {
            // get the zombie object
            Zombie zombie = other.transform.root.GetComponent<Zombie>();
            // check the zombie status
            if (zombie.GetState() == Zombie.ZombieState.Attacking)
            {
                Debug.Log("Zombie is attacking");
                // decrease the user health by 10
                var color = hurtBackgroundImage.color;
                color.a = 0.8f;
                hurtBackgroundImage.color = color;
                userHealth -= 10;
                Debug.Log("User health: " + userHealth);
                GetComponent<AudioSource>().Play();

                if (userHealth <= 0)
                {
                    restartCanvas.SetActive(true);
                    var c = hurtBackgroundImage.color;
                    c.a = 0f;
                    hurtBackgroundImage.color = c;
                    Debug.Log("Game over");
                    restartCanvas.SetActive(true);
                }
            }
            StartCoroutine(DisableAttacksForDuration(4f));
        }
    }

    public void IncreaseHealth(int addHealth)
    {
        maxHealth += addHealth;
        userHealth = maxHealth;
        Debug.Log("User health: " + userHealth);
    }

    private IEnumerator DisableAttacksForDuration(float duration)
    {
        // Prevent further attacks for the specified duration
        canBeAttacked = false;
        yield return new WaitForSeconds(duration);
        canBeAttacked = true;
    }

    public int getUserHealth()
    {
        return userHealth;
    }
    public void setUserHealth(int health)
    {
        userHealth = health;
    }

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestoreHealth(int rHealth)
    {
        userHealth += rHealth;
    }
}
