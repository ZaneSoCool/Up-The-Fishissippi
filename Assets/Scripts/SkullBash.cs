using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class SkullBash : MonoBehaviour
{
    //setup references
    InputAction skullBashAction;
    Animator anim;
    player playerScript;

    [SerializeField]
    int bashDamage = 5;
    List<GameObject> objectsInSkullBash = new List<GameObject>(); //list of object currently in the hitbox of the tail thwap

    public bool isSkullBashing = false;

    void Start()
    {
        //find references
        skullBashAction = InputSystem.actions.FindAction("Interact");
        anim = transform.parent.gameObject.GetComponent<Animator>();
        playerScript = transform.parent.GetComponent<player>();

        //call tail thwap if action is performed
        skullBashAction.performed += OnBashPerformed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Attackable") && !objectsInSkullBash.Contains(col.gameObject))
        {
            objectsInSkullBash.Add(col.gameObject);
        }

        if (isSkullBashing == true)
        {
            hurtVictim();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Attackable"))
        {
            objectsInSkullBash.Remove(col.gameObject);
        }
    }

    private void OnBashPerformed(InputAction.CallbackContext context)
    {
        if (playerScript == null || !playerScript.hasSkullBash) return;

        isSkullBashing = true;

        //play tail thwap animation
        playerScript.isDoingSpecialAnim = true;
        anim.Play("SkullBash");
    }

    private void hurtVictim()
    {
        //for each object in objectInSkullBash call their attacked method
        for (int i = 0; i < objectsInSkullBash.Count; i++)
        {
            Attackable victimScript = objectsInSkullBash[i].GetComponent<Attackable>();
            if (victimScript != null){
                victimScript.Attacked(bashDamage);
            }
        }
    }
}
