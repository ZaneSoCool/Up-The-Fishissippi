//Written by Zane Pederson

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
    SpriteRenderer tailThwapSprite;
    
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
        tailThwapSprite = transform.Find("TailThwapHitbox").gameObject.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        //setup player health
        playerHealth = maxPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();

        if (direction != new Vector2(0,0))
        {   //move player
            rigidBody.linearVelocity += acceleration * direction * Time.deltaTime;

            if (rigidBody.linearVelocity.x > 0)
            {
                spriteRenderer.flipX = false;
                tailThwapSprite.flipX = false;

            } else if (rigidBody.linearVelocity.x < 0)
            {
                spriteRenderer.flipX = true;
                tailThwapSprite.flipX = true;
            }

        }  else
        {   //friction
            rigidBody.linearVelocity = Vector2.MoveTowards(rigidBody.linearVelocity, new Vector2(0,0), friction * Time.deltaTime);
        }

        //clamp max speed
        rigidBody.linearVelocity = Vector2.ClampMagnitude(rigidBody.linearVelocity, maxSpeed);


        //rotate player based on vertical speed
        if (rigidBody.linearVelocity != new Vector2(0,0)){
            float angle = Mathf.Atan2(rigidBody.linearVelocity.y, rigidBody.linearVelocity.x) * Mathf.Rad2Deg;
            if (spriteRenderer.flipX == false)
            {
                gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                tailThwapHitbox.transform.localPosition = new Vector2(1f, 0f);

            } else{
                angle -= 180;
                gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                tailThwapHitbox.transform.localPosition = new Vector2(-1f, 0f);
            }
        }

        decideAnimation();
    }

    private void decideAnimation()
    {
        if (isDoingSpecialAnim) return;

        if (rigidBody.linearVelocity.magnitude > 0){
            anim.Play("PlayerSwim");
        } else
        {
            anim.Play("PlayerIdle");
        }
    }

    public void resetAnimation() //called when abnormal player animation finishes
    {
        isDoingSpecialAnim = false;
    }
}
