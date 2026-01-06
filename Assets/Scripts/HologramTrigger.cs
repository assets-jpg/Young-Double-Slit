using UnityEngine;
using UnityEngine.Events;

public class HologramTrigger : MonoBehaviour
{
    public UnityEvent onPlayerEnter;

    [Header("Teleport Settings")]

    private bool triggered = false;

   
    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        onPlayerEnter.Invoke();

        
        other.transform.rotation = gameObject.transform.rotation;
       
       

        gameObject.SetActive(false);
    }
}
