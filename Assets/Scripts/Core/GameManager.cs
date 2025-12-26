using UnityEngine;
using UnityEngine.SceneManagement;

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
