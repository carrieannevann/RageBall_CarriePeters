using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneWatchdog : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoaded;
        SceneManager.activeSceneChanged += OnChanged;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoaded;
        SceneManager.activeSceneChanged -= OnChanged;
    }

    void OnLoaded(Scene scene, LoadSceneMode mode)
    {
        UnityEngine.Debug.LogError($"[SceneWatchdog] sceneLoaded: {scene.name} ({mode})\n" +
            new System.Diagnostics.StackTrace(true));
    }

    void OnChanged(Scene from, Scene to)
    {
        UnityEngine.Debug.LogError($"[SceneWatchdog] activeSceneChanged: {from.name} ? {to.name}\n" +
            new System.Diagnostics.StackTrace(true));
    }
}
