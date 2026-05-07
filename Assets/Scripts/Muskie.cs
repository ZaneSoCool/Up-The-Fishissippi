using UnityEngine;
using UnityEngine.InputSystem;

public class Muskie : MonoBehaviour
{
    Animator anim;
    private GameObject canvas;
    private player playerScript;
    private bool sequenceActive = false;

    private BoxCollider2D trigger;

    InputAction tailThwapAction;

    void Awake()
    {
        trigger = GetComponent<BoxCollider2D>();
        trigger.enabled = false;
    }

    void Start()
    {
        canvas = transform.GetChild(0).gameObject;
        anim = GetComponent<Animator>();
        canvas.SetActive(false);
        playerScript = GameObject.FindWithTag("Player").GetComponent<player>();
        tailThwapAction = InputSystem.actions.FindAction("Attack");

        //Destroy Muskie if player already has tailThwap
        if (playerScript.hasTailThwap)
        {
            Destroy(gameObject);
        }

        //call tail thwap if action is performed
        tailThwapAction.performed += OnTailThwapPerformed;

        //fixes bug where player activates trigger on entrance
        Invoke("enableTrigger", 0.5f);       
    }

    void enableTrigger()
    {
        trigger.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only the player should trigger muskie
        if (!other.CompareTag("Player")) return;

        sequenceActive = true;
        playerScript.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        playerScript.canMove = false;
        anim.Play("MuskieAttack");
    }

    public void MuskieAttackEnded()
    {
        playerScript.hasTailThwap = true;
        canvas.SetActive(true);
    }

    private void OnTailThwapPerformed(InputAction.CallbackContext context)
    {
        if (!sequenceActive || playerScript == null || !playerScript.hasTailThwap) return;

        sequenceActive = false;
        anim.Play("MuskieThwap");
    }

    public void MuskieWasThwapped()
    {
        playerScript.canMove = true;
        canvas.SetActive(false);
        Destroy(gameObject);
    }
    
}
