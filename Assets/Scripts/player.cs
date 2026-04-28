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
    BoxCollider2D skullBashHitbox;
    SkullBash skullBashScript;
    SpriteRenderer tailThwapSprite;
    
    //Health variables
    [SerializeField]
    int maxPlayerHealth = 5;
    public int playerHealth;

    //Wether player is currently doing an animation outside of standard movement
    public bool isDoingSpecialAnim = false;
    public bool inputEnabled = true;

    //progression trackers
    public bool hasTailThwap;
    public bool hasSkullBash;
    public bool canMove = true;
    public int coinsCount = 0;

    //vars for skullbash
    private Vector2 skullBashDirection = new Vector2(1,0);
    [SerializeField]
    private float bashSpeed = 5.0f;

    //above-water gravity (only active during boss fight)
    [SerializeField] private float aboveWaterGravity = 12f;
    [SerializeField] private float maxFallSpeed = 6f;
    private float fallVelocity = 0f;
    private bool wasAboveWater = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //find object references
        rigidBody = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        tailThwapHitbox = transform.Find("TailThwapHitbox").gameObject.GetComponent<BoxCollider2D>();
        tailThwapSprite = transform.Find("TailThwapHitbox").gameObject.GetComponent<SpriteRenderer>();

        skullBashHitbox = transform.Find("SkullBashHitbox").gameObject.GetComponent<BoxCollider2D>();
        skullBashScript = transform.Find("SkullBashHitbox").GetComponent<SkullBash>();

        //setup player health
        playerHealth = maxPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) //boolean that prevents player from moving (used in cutscenes)
        {
            rigidBody.linearVelocity = new Vector2(0,0);
            return;
        }
    

        bool aboveWater = IsAboveWater();

        if (skullBashScript.isSkullBashing == true) { //do dash — allowed above water, gravity kicks in after it ends
            skullBashDirection.Normalize();
            rigidBody.linearVelocity = bashSpeed * skullBashDirection;

        } else if (aboveWater) { //above water: fall, no swimming
            fallVelocity += aboveWaterGravity * Time.deltaTime;
            fallVelocity = Mathf.Min(fallVelocity, maxFallSpeed);
            float newX = Mathf.MoveTowards(rigidBody.linearVelocity.x, 0f, friction * Time.deltaTime);
            rigidBody.linearVelocity = new Vector2(newX, -fallVelocity);

        } else { //normal underwater swimming
            if (wasAboveWater) rigidBody.linearVelocity = Vector2.zero; //lose momentum on water re-entry
            fallVelocity = 0f;

            Vector2 direction = inputEnabled ? moveAction.ReadValue<Vector2>() : Vector2.zero;

            if (direction != new Vector2(0,0))
            {
                skullBashDirection = direction;
                rigidBody.linearVelocity += acceleration * direction * Time.deltaTime;
            }  else {
                rigidBody.linearVelocity = Vector2.MoveTowards(rigidBody.linearVelocity, new Vector2(0,0), friction * Time.deltaTime);
            }

            rigidBody.linearVelocity = Vector2.ClampMagnitude(rigidBody.linearVelocity, maxSpeed);
        }

        wasAboveWater = aboveWater;
        
        //flip sprite if needed
        if (rigidBody.linearVelocity.x > 0){
            spriteRenderer.flipX = false;
            tailThwapSprite.flipX = false;

        } else if (rigidBody.linearVelocity.x < 0){
            spriteRenderer.flipX = true;
            tailThwapSprite.flipX = true;
        }

        //rotate player & hitboxes based on vertical speed
        if (rigidBody.linearVelocity != new Vector2(0,0)){
            float angle = Mathf.Atan2(rigidBody.linearVelocity.y, rigidBody.linearVelocity.x) * Mathf.Rad2Deg;
            if (spriteRenderer.flipX == false)
            {
                gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                tailThwapHitbox.transform.localPosition = new Vector2(1f, 0f);
                skullBashHitbox.transform.localPosition = new Vector2(0.5f, 0f);

            } else{
                angle -= 180;
                gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                tailThwapHitbox.transform.localPosition = new Vector2(-1f, 0f);
                skullBashHitbox.transform.localPosition = new Vector2(-0.5f, 0f);
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

    // Cancels the bash and applies a light nudge in the given direction.
    public void BashBounce(Vector2 direction, float speed)
    {
        skullBashScript.CancelBash();
        rigidBody.linearVelocity = direction.normalized * speed;
    }

    private bool IsAboveWater()
    {
        if (TheRoyalFlush.Instance == null || !TheRoyalFlush.Instance.bossStarted) return false;
        return transform.position.y > TheRoyalFlush.Instance.WaterLevel;
    }
}
