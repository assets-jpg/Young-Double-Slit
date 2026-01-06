using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Invoke(nameof(Recenter), 0.2f); // small delay is important
    }

    void Recenter()
    {
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);

        foreach (var subsystem in subsystems)
        {
            if (subsystem.running)
            {
                subsystem.TryRecenter();
            }
        }
    }

    /* =======================
     *  SCENE CONTROL
     * ======================= */

        // Load scene immediately (by name)
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Load scene immediately (by index)
    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    // 🔥 Load scene after delay (seconds)
    public void LoadSceneAfterDelay(string sceneName, float delay)
    {
        Invoke(nameof(InvokeLoadScene), delay);
        _pendingSceneName = sceneName;
    }

    // Reload current scene
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /* =======================
     *  INTERNAL
     * ======================= */

    private string _pendingSceneName;

    private void InvokeLoadScene()
    {
        if (!string.IsNullOrEmpty(_pendingSceneName))
            SceneManager.LoadScene(_pendingSceneName);
    }
}
