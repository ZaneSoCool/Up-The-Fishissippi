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
    BoxCollider2D tailThwapHitbox;
    
    //Health variables
    [SerializeField]
    int maxPlayerHealth = 5;
    public int playerHealth;

    //Wether player is currently doing an animation outside of standard movement
    public bool isDoingSpecialAnim = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //find object references
        rigidBody = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        spriteRenderer = GetComponent<SpriteRenderer>();
        tailThwapHitbox = transform.Find("TailThwapHitbox").gameObject.GetComponent<BoxCollider2D>();
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
                tailThwapHitbox.offset = new Vector2(0.8f, 0.0f); //flip tail thwap hitbox

            } else if (rigidBody.linearVelocity.x < 0)
            {
                spriteRenderer.flipX = true;
                tailThwapHitbox.offset = new Vector2(-0.8f, 0.0f);; //flip tail thwap hitbox
            }

        }  else
        {   //friction
            rigidBody.linearVelocity = Vector2.MoveTowards(rigidBody.linearVelocity, new Vector2(0,0), friction * Time.deltaTime);
        }

        //clamp max speed
        rigidBody.linearVelocity = Vector2.ClampMagnitude(rigidBody.linearVelocity, maxSpeed);

        decideAnimation();
    }

    void decideAnimation()
    {
        if (isDoingSpecialAnim) return;

        if (rigidBody.linearVelocity.magnitude > 0){
            anim.Play("PlayerSwim");
        } else
        {
            anim.Play("PlayerIdle");
        }
    }
}
