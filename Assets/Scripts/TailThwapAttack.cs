using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class TailThwapAttack : MonoBehaviour
{

    //references
    InputAction tailThwapAction;

    [SerializeField]
    int thwapDamage = 5;
    List<GameObject> objectsInTailThwap = new List<GameObject>(); //list of object currently in the hitbox of the tail thwap

    void Start()
    {
        tailThwapAction = InputSystem.actions.FindAction("Attack");
        //call tail thwap if action is performed
        tailThwapAction.performed += OnTailThwapPerformed; 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Attackable") && !objectsInTailThwap.Contains(col.gameObject))
        {
            Debug.Log("BUBLE ENTERED");
            objectsInTailThwap.Add(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Attackable"))
        {
            Debug.Log("BUBLE exit");
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
        }
    }
}
