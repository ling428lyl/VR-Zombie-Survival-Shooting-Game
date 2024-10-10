
using UnityEngine;
using UnityEngine.InputSystem;

public class handController : MonoBehaviour
{
    public InputActionReference gripInput;
    public InputActionReference triggerInput;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {

        if (!animator) return;
        float grip = gripInput.action.ReadValue<float>();
        float trigger = triggerInput.action.ReadValue<float>();
        animator.SetFloat("grip", grip);
        animator.SetFloat("trigger", trigger);
    }
}
