using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class TailThwapAttack : MonoBehaviour
{
    //setup references
    InputAction tailThwapAction;
    Animator anim;

    [SerializeField]
    int thwapDamage = 5;
    List<GameObject> objectsInTailThwap = new List<GameObject>(); //list of object currently in the hitbox of the tail thwap

    void Start()
    {
        //find references
        tailThwapAction = InputSystem.actions.FindAction("Attack");
        anim = transform.parent.gameObject.GetComponent<Animator>();

        //call tail thwap if action is performed
        tailThwapAction.performed += OnTailThwapPerformed; 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Attackable") && !objectsInTailThwap.Contains(col.gameObject))
        {
            objectsInTailThwap.Add(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Attackable"))
        {
            objectsInTailThwap.Remove(col.gameObject);
        }
    }

    private void OnTailThwapPerformed(InputAction.CallbackContext context)
    {
        //for each object in objectInTailThwap call their attacked method
        for (int i = 0; i < objectsInTailThwap.Count; i++)
        {
            Attackable victimScript = objectsInTailThwap[i].GetComponent<Attackable>();;
            victimScript.Attacked(thwapDamage);

            //play tail thwap animation
            transform.parent.gameObject.isDoingSpecialAnim = true;
            anim.Play("TailThwap");
        }
    }
}
