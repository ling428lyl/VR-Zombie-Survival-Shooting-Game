using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitBoss : MonoBehaviour
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
        Boss boss;
        if(!isShoot)
        {
            return;
        }
        if (other.CompareTag("ZombieComponentHead"))
        {
            boss = other.transform.root.GetComponent<Boss>();
            Debug.Log("Hit the head");
            boss.TakeDamage(Boss.DamagePosition.Head);
            Destroy(transform.parent.gameObject);
        }
        else if (other.CompareTag("ZombieComponentUpper"))
        {
            boss = other.transform.root.GetComponent<Boss>();
            Debug.Log("Hit the upper body");
            boss.TakeDamage(Boss.DamagePosition.UpperBody);
            Destroy(transform.parent.gameObject);
        }
        else if (other.CompareTag("ZombieComponentLower"))
        {
            boss = other.transform.root.GetComponent<Boss>();
            Debug.Log("Hit the lower body");
            boss.TakeDamage(Boss.DamagePosition.LowerBody);
            Destroy(transform.parent.gameObject);
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
