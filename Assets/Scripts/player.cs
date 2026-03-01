using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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
    InputAction tailThwapAction;
    SpriteRenderer spriteRenderer;
    Animator anim;
    BoxCollider2D tailThwapHitbox;
    
    //Health variables
    [SerializeField]
    int maxPlayerHealth = 5;
    public int playerHealth;

    //Attack variables
    [SerializeField]
    int thwapDamage = 5;
    List<GameObject> objectsInTailThwap = new List<GameObject>(); //list of object currently in the hitbox of the tail thwap
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //find object references
        rigidBody = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        tailThwapAction = InputSystem.actions.FindAction("Attack");
        spriteRenderer = GetComponent<SpriteRenderer>();
        tailThwapHitbox = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        //setup player health
        playerHealth = maxPlayerHealth;

        //call tail thwap if action is performed
        tailThwapAction.performed += OnTailThwapPerformed; 
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
        if (rigidBody.linearVelocity.magnitude > 0){
            anim.Play("PlayerSwim");
        } else
        {
            anim.Play("PlayerIdle");
        }
    }

    private void OnTailThwapPerformed(InputAction.CallbackContext context)
    {
        //for each object in objectInTailThwap call their attacked method
        for (int i = 0; i < objectsInTailThwap.Count; i++)
        {
            Attackable victimScript = objectsInTailThwap[i].GetComponent<Attackable>();;
            victimScript.Attacked(thwapDamage);
        }
    }


    //For tail thwap attack hitbox
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Attackable") && !objectsInTailThwap.Contains(col.gameObject))
        {
            Debug.Log("BUBLE ENTERED");
            objectsInTailThwap.Add(col.gameObject);
        }
    }

    //For tail thwap attack hitbox
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Attackable"))
        {
            Debug.Log("BUBLE exit");
            objectsInTailThwap.Remove(col.gameObject);
        }
    }
}
