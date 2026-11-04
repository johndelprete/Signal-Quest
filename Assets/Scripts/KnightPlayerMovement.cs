using UnityEngine;
using UnityEngine.InputSystem;

public class KnightPlayerMovementPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float stepInterval = 0.3f;

    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource footstepAudio;
    private Vector2 movement;

    private string currentAnimation = "";
    private string lastDirection = "Down";
    private float stepTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        footstepAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        movement = Vector2.zero;

        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
        {
            return;
        }

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
        {
            movement.y += 1;
        }

        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
        {
            movement.y -= 1;
        }

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            movement.x -= 1;
        }

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            movement.x += 1;
        }

        movement = movement.normalized;

        if (movement != Vector2.zero)
        {
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                lastDirection = movement.x > 0 ? "Right" : "Left";
            }
            else
            {
                lastDirection = movement.y > 0 ? "Up" : "Down";
            }

            PlayAnimation("Walk_" + lastDirection);
            PlayFootsteps();
        }
        else
        {
            PlayAnimation("Idle_" + lastDirection);
            stepTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        Vector2 newPosition =
            rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);
    }

    void PlayAnimation(string animationName)
    {
        if (currentAnimation == animationName)
        {
            return;
        }

        currentAnimation = animationName;
        animator.Play(currentAnimation);
    }

    void PlayFootsteps()
    {
        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            footstepAudio.Play();
            stepTimer = stepInterval;
        }
    }
}