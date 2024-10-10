using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitZombie : MonoBehaviour
{
    private bool isShoot = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnTriggerEnter(Collider other)
    {
        Zombie zombie;
        Boss boss;
        if(!isShoot)
        {
            return;
        }
        if (other.CompareTag("ZombieComponentHead"))
        {
            zombie = other.transform.root.GetComponent<Zombie>();
            if(zombie != null) {
                zombie.TakeDamage(Zombie.DamagePosition.Head);
                Destroy(transform.parent.gameObject);
            } else {
                boss = other.transform.root.GetComponent<Boss>();
                if(boss != null){
                    Debug.Log("Hit the head");
                    boss.TakeDamage(Boss.DamagePosition.Head);
                    Destroy(transform.parent.gameObject);
                }
            }
            return;
            
        }
        else if (other.CompareTag("ZombieComponentUpper"))
        {
            zombie = other.transform.root.GetComponent<Zombie>();
            if(zombie != null) {
                Debug.Log("Hit the upper body");
                zombie.TakeDamage(Zombie.DamagePosition.UpperBody);
                Destroy(transform.parent.gameObject);
            } else {
                boss = other.transform.root.GetComponent<Boss>();
                if(boss != null){
                    Debug.Log("Hit the upper body");
                    boss.TakeDamage(Boss.DamagePosition.UpperBody);
                    Destroy(transform.parent.gameObject);
                }
            }
            /*Debug.Log("Hit the upper body");
            zombie.TakeDamage(Zombie.DamagePosition.UpperBody);
            Destroy(transform.parent.gameObject);*/
        }
        else if (other.CompareTag("ZombieComponentLower"))
        {
            zombie = other.transform.root.GetComponent<Zombie>();
            if(zombie != null) {
                Debug.Log("Hit the lower body");
                zombie.TakeDamage(Zombie.DamagePosition.LowerBody);
                Destroy(transform.parent.gameObject);
            } else {
                boss = other.transform.root.GetComponent<Boss>();
                if(boss != null){
                    Debug.Log("Hit the lower body");
                    boss.TakeDamage(Boss.DamagePosition.LowerBody);
                    Destroy(transform.parent.gameObject);
                }
            }
        }
        else
        {
            return;
        }
    }

    public void SetBulletStatusShoot()
    {
        isShoot = true;
    }
}
