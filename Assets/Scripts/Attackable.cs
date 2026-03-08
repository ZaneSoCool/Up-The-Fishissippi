//Written by Zane Pederson

using UnityEngine;

public class Attackable : MonoBehaviour
{
    [SerializeField]
    int health;

    public void Attacked(int damage)
    {
        Debug.Log("was attacked!!");
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            //player died
        } else
        {
            Destroy(gameObject);
        }  
    }
}
