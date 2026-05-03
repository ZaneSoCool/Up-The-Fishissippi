using UnityEngine;
public class fleeTrigger : MonoBehaviour
{
    [SerializeField]
    private muskieTease muskie;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (muskie)
        {
            muskie.flee();
        }
        
        Destroy(this);
    }
}
