using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    [SerializeField]
    float maxSpeed = 1.0f;
    [SerializeField]
    float acceleration = 0.1f;
    [SerializeField]
    float friction = 1.0f;
    Rigidbody2D rigidBody;
    InputAction moveAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAction.ReadValue<Vector2>() != new Vector2(0,0))
        {   //move player
            rigidBody.linearVelocity += acceleration * moveAction.ReadValue<Vector2>() * Time.deltaTime;
        }  else
        {   //friction
            rigidBody.linearVelocity = Vector2.MoveTowards(rigidBody.linearVelocity, new Vector2(0,0), friction * Time.deltaTime);
        }

        //clamp max speed
        rigidBody.linearVelocity = Vector2.ClampMagnitude(rigidBody.linearVelocity, maxSpeed);

        Debug.Log(rigidBody.linearVelocity);
        
    }
}
