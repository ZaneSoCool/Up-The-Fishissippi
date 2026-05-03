using UnityEngine;
using System.Collections;

public class muskieTease : MonoBehaviour
{
    Animator anim;
    private player playerScript;
    private bool fleeReady = false; //used to handle muskie behavior if trigger is hit earlier than expected

    void Start()
    {
        anim = GetComponent<Animator>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<player>();

        StartCoroutine(timer()); //short timer to avoid player pos not being correct yet

        //Destroy Muskie if player already has tailThwap
        if (playerScript.transform.position.x < transform.position.x)
        {
            Destroy(gameObject);
        }

        anim.Play("MuskieSwim");
    }

    public void muskiePeeked()
    {
        if (fleeReady)
        {
            flee();
        } else
        {
            fleeReady = true;
        }
    }

    public void flee()
    {
        if (!fleeReady)
        {
            fleeReady = true;
            return;
        }
        anim.Play("MuskieFlee");
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(0.1f);
    }
}
