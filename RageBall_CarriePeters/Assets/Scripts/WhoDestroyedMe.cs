using UnityEngine;

public class WhoDestroyedMe : MonoBehaviour
{
    void OnDestroy()
    {
        // Use fully-qualified names to avoid the Debug clash
        UnityEngine.Debug.LogError(
            $"[WhoDestroyedMe] '{name}' DESTROYED.\n" +
            new System.Diagnostics.StackTrace(true)
        );
    }
}
