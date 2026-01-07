using UnityEngine;
using UnityEngine.Events;

public class WoodCubeCollider : MonoBehaviour
{
    public UnityEvent onCollision;

    private bool isActive;

    private void OnEnable()
    {
        isActive = true;
    }

    private void OnDisable()
    {
        isActive = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isActive) return;

        onCollision.Invoke();
    }
}
