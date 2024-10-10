using UnityEngine;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour
{
    public Button button;
    public AudioSource audioSource;

    void Start()
    {
        button.onClick.AddListener(PlaySound);
    }

   public void PlaySound()
    {
        audioSource.Play();  // ????
    }
}
