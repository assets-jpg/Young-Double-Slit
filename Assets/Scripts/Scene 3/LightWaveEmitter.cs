using UnityEngine;

public class LightWaveEmitter : MonoBehaviour
{
    public GameObject waveSpherePrefab;
    public float spawnInterval = 0.6f;

    void Start()
    {
        InvokeRepeating(nameof(EmitWave), 0f, spawnInterval);
    }

    void EmitWave()
    {
        Instantiate(
            waveSpherePrefab,
            transform.position,
            transform.rotation   // 🔥 THIS is the key
        );
    }
}
