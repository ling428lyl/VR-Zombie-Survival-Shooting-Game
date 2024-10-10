using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GunTutorialManager : MonoBehaviour
{
    public enum TutorialStep
    {
        TeleportOne,
        TeleportTwo,
        InsertMagazine,
        ReloadGun,
        ShootGun,
        PlaceObjectInSocket,
        Completed
    }

    public static GunTutorialManager Instance { get; private set; }

    public TextMeshProUGUI tutorialText;
    public SimpleShoot gunScript; // Reference to the SimpleShoot script
    public TutorialStep currentStep = TutorialStep.TeleportOne;

    public Button restartButton;  // Assign this in the inspector
    public Button skipButton;  // Assign this in the inspector
    public TextMeshProUGUI skipButtonText;  // Assign this in the inspector
    public GameObject tutorialPanel;  // Assign this in the inspector

    public teleportDetector teleportDetector;

    void Start()
    {
        Debug.Log("Start Game");
        Debug.Log("Tutorial started. Initial step: " + currentStep);
        UpdateTutorialText();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void UpdateTutorialText()
    {
        switch (currentStep)
        {
            case TutorialStep.TeleportOne:
                tutorialText.text = "Pick up the TeleportWatch from your right seat and place on your wrist.";
                skipButtonText.text = "Skip";
                break;
            case TutorialStep.TeleportTwo:
                tutorialText.text = "Press Grip button on left controller to teleport.";
                skipButtonText.text = "Skip";
                break;
            case TutorialStep.InsertMagazine:
                tutorialText.text = "Insert the magazine into the gun.";
                skipButtonText.text = "Skip";  // Default text for skipping
                restartButton.gameObject.SetActive(true);
                break;
            case TutorialStep.ReloadGun:
                tutorialText.text = "Pull the slide to reload the gun.";
                skipButtonText.text = "Skip";  // Keep default text
                break;
            case TutorialStep.ShootGun:
                tutorialText.text = "Aim and shoot the gun.";
                skipButtonText.text = "Skip";  // Keep default text
                break;
            case TutorialStep.PlaceObjectInSocket:
                tutorialText.text = "Take the magazine out and place it your carrier below for later use.";
                skipButtonText.text = "Skip";  // Keep default text
                break;
            case TutorialStep.Completed:
                tutorialText.text = "Tutorial completed!";
                skipButtonText.text = "Finish";  // Change text to "Finish"
                break;
        }
    }


    public void AdvanceTutorial()
    {
        if (currentStep < TutorialStep.Completed)
        {
            currentStep++; // Move to the next step
            Debug.Log("Advanced to: " + currentStep);
            UpdateTutorialText();
        }
    }

    public void TeleportOneDone()
    {
        Debug.Log("putting on TeleportWatch.");
        if (currentStep==TutorialStep.TeleportOne)
        {
            Debug.Log("Put on.");
            //restartButton.gameObject.SetActive(false);
            AdvanceTutorial();
        }
    }

    public void TeleportTwoDone()
    {
        Debug.Log("try teleport.");
        if (currentStep==TutorialStep.TeleportTwo)
        {
            Debug.Log("teleport done.");
            //restartButton.gameObject.SetActive(false);
            AdvanceTutorial();
        }
    }

    public void MagazineInserted()
    {
        Debug.Log("Trying to insert magazine.");
        if (currentStep == TutorialStep.InsertMagazine)
        {
            
            Debug.Log("Magazine inserted");
            AdvanceTutorial();
        }
    }

    public void GunReloaded()
    {
        Debug.Log("Trying to reload gun.");
        if (currentStep == TutorialStep.ReloadGun)
        {
            Debug.Log("Gun reloaded");
            AdvanceTutorial();
        }
    }

    public void GunFired()
    {
        Debug.Log("Trying to fire gun.");
        if (currentStep == TutorialStep.ShootGun)
        {
            Debug.Log("Gun fired");
            AdvanceTutorial();
        }
    }

    public void ObjectPlacedInSocket()
    {
        Debug.Log("Object placed in socket.");
        if (currentStep == TutorialStep.PlaceObjectInSocket)
        {
            AdvanceTutorial();
        }
    }


    public void RestartTutorial()
    {
        currentStep = TutorialStep.InsertMagazine;
        Debug.Log("Tutorial restarted.");
        UpdateTutorialText();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SkipTutorial()
    {
        // Hide the tutorial panel
        tutorialPanel.SetActive(false);
        Debug.Log("Tutorial skipped.");
        // Load level 1 game scene or perform completion actions
        SceneManager.LoadScene("SampleScene");
    }

}