using UnityEngine;

public class CamFollow : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (!cam) return;

        // Direction FROM object TO camera
        Vector3 direction = cam.transform.position - transform.position;

        // Lock vertical tilt
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        // Rotate ONLY
        transform.rotation = Quaternion.LookRotation(-direction);
    }
}
