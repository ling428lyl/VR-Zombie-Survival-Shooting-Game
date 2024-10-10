using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;


public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")][SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")][SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")][SerializeField] private float ejectPower = 150f;

    public AudioSource source;
    public AudioClip fireSound;
    public AudioClip reload;
    public AudioClip noAmmo;
    public AudioClip insert;

    //private ActionBasedController _XRController;

    public Magazine magazine;
    public XRBaseInteractor socketInteractor;
    public bool hasSlide = true;

    public TextMeshProUGUI ammoText;
    public HapticInteractable hapticInteractable; // Assign this via the Inspector or find it dynamically

    public GunTutorialManager tutorialManager;

    public void AddMagazine(XRBaseInteractable interactable)
    {
        Magazine newMagazine = interactable.GetComponent<Magazine>();
        if (newMagazine != null)
        {
            magazine = newMagazine;
            source.PlayOneShot(insert);
            hasSlide = false;

            // Trigger the magazine inserted event
            if (hapticInteractable != null)
                hapticInteractable.OnMagazineInserted(newMagazine);
        }
        tutorialManager.MagazineInserted();
    }

    public void RemoveMagazine(XRBaseInteractable interactable)
    {
        if (magazine != null)
        {
            source.PlayOneShot(insert);
            if (hapticInteractable != null)
                hapticInteractable.OnMagazineRemoved();

            magazine = null;
        }
    }

    public void Slide()
    {
        if (magazine != null)
        {
            hasSlide = true;
            source.PlayOneShot(reload);
            ChangeAmmo(magazine.numberOfBullets);
            tutorialManager.GunReloaded();
        }
        else
        {
            // Handle case when there is no magazine
            hasSlide = false;
            source.PlayOneShot(noAmmo);
        }
    }


    void Start()
    {
        // Assign the haptic interactable component
        hapticInteractable = GetComponent<HapticInteractable>();

        // Existing setup code
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        socketInteractor.onSelectEntered.AddListener(AddMagazine);
        socketInteractor.onSelectExited.AddListener(RemoveMagazine);

        //// Check for an initial magazine and update accordingly
        //if (magazine != null)
        //{
        //    // Assume hasSlide is true if a magazine is present at start
        //    // Adjust based on your game's logic
        //    hasSlide = true;
        //    UpdateAmmoDisplay(); // Update the ammo display with the initial magazine's ammo count
        //}
        //else
        //{
        //    // Handle the case where no magazine is attached at the start
        //    hasSlide = false; // Or true, depending on your game logic
        //                      // Potentially update UI to indicate no magazine is present
        //    if (ammoText != null) ammoText.text = "No Mag";
        //}
    }


    void UpdateAmmoDisplay()
    {
        if (ammoText != null && magazine.numberOfBullets >= 0)
            ammoText.text = magazine.numberOfBullets.ToString();
    }

    public void ChangeAmmo(int newNumberOfBullets)
    {
        magazine.numberOfBullets = newNumberOfBullets;
        UpdateAmmoDisplay();
    }

    public void PullTrigger()
    {
        if (magazine && magazine.numberOfBullets > 0 && hasSlide)
        {
            gunAnimator.SetTrigger("Fire");
        }
        else
        {
            source.PlayOneShot(noAmmo);
        }
    }

    //This function creates the bullet behavior
    void Shoot()
    {
        magazine.numberOfBullets--;
        ChangeAmmo(magazine.numberOfBullets);
        source.PlayOneShot(fireSound);
        //_XRController.SendHapticImpulse(1f, 1f);

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        GameObject bulletObject = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        bulletObject.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
        bulletObject.GetComponent<BulletHitZombie>().SetBulletStatusShoot();
        bulletObject.GetComponent<BulletHitBoss>().SetBulletStatusShoot();

        tutorialManager.GunFired();
    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

}