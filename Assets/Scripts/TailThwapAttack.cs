//Written by Zane Pederson

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class TailThwapAttack : MonoBehaviour
{
    //setup references
    InputAction tailThwapAction;
    Animator anim;
    Animator effect_anim;
    player playerScript;

    [SerializeField]
    int thwapDamage = 5;
    List<GameObject> objectsInTailThwap = new List<GameObject>(); //list of object currently in the hitbox of the tail thwap

    void Start()
    {
        //find references
        tailThwapAction = InputSystem.actions.FindAction("Attack");
        anim = transform.parent.gameObject.GetComponent<Animator>();
        effect_anim = gameObject.GetComponent<Animator>();
        playerScript = transform.parent.GetComponent<player>();

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
        if (playerScript == null) return;

        //play tail thwap animation
        playerScript.isDoingSpecialAnim = true;
        anim.Play("TailThwap");
        effect_anim.Play("TailThwapWave");
       
        //for each object in objectInTailThwap call their attacked method
        for (int i = 0; i < objectsInTailThwap.Count; i++)
        {
            Attackable victimScript = objectsInTailThwap[i].GetComponent<Attackable>();
            if (victimScript != null){
                victimScript.Attacked(thwapDamage);
            }
        }

    }

    public void TailThwapWaveAnimEnded()
    {
        effect_anim.Play("TailThwapWaveIdle");
    }
}
