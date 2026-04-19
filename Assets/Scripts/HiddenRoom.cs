using UnityEngine;
using System.Collections;

public class HiddenRoom : MonoBehaviour
{
void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fade(0.8f, 1.0f));
        }
    }

    public IEnumerator Fade(float newOpacity, float duration)
    {
        Renderer rend = GetComponent<Renderer>();
        float ogOpacity = rend.material.color.a;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newAlpha = Mathf.Lerp(ogOpacity, newOpacity, timer / duration);
            
            // You must reassign the entire color because material.color.a is read-only
            Color newColor = rend.material.color;
            newColor.a = newAlpha;
            rend.material.color = newColor;

            yield return null;
        }
    }
}