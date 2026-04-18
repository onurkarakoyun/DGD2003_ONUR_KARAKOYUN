using UnityEngine;
using UnityEngine.Events;

public class FusePickup : MonoBehaviour
{
    public static UnityEvent OnFuseCollected = new UnityEvent();

    public void Collect()
    {
        Debug.Log("Bir sigorta (Fuse) buldun!");
        
        OnFuseCollected.Invoke();
        
        Destroy(gameObject);
    }
}
