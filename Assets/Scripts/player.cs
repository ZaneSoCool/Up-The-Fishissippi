using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    
    //Movement variables
    [SerializeField]
    float maxSpeed = 1.0f;
    [SerializeField]
    float acceleration = 0.1f;
    [SerializeField]
    float friction = 1.0f;

    //Object references
    Rigidbody2D rigidBody;
    InputAction moveAction;
    SpriteRenderer spriteRenderer;
    Animator anim;
    

    //Health variables
    [SerializeField]
    int maxPlayerHealth = 5;
    public int playerHealth;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //find object references
        rigidBody = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        //setup player health
        playerHealth = maxPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAction.ReadValue<Vector2>() != new Vector2(0,0))
        {   //move player
            rigidBody.linearVelocity += acceleration * moveAction.ReadValue<Vector2>() * Time.deltaTime;

            if (rigidBody.linearVelocity.x > 0)
            {
                spriteRenderer.flipX = false;
            } else if (rigidBody.linearVelocity.x < 0)
            {
                spriteRenderer.flipX = true;
            }

        }  else
        {   //friction
            rigidBody.linearVelocity = Vector2.MoveTowards(rigidBody.linearVelocity, new Vector2(0,0), friction * Time.deltaTime);
        }

        //clamp max speed
        rigidBody.linearVelocity = Vector2.ClampMagnitude(rigidBody.linearVelocity, maxSpeed);

        decideAnimation();

        //check if dead
        if (playerHealth <= 0)
        {
            //TODO: GO BACK TO LAST SPAWNPOINT
        }
    }

    void decideAnimation()
    {
        if (rigidBody.linearVelocity.magnitude > 0){
            anim.Play("PlayerSwim");
        } else
        {
            anim.Play("PlayerIdle");
        }
    }
}
