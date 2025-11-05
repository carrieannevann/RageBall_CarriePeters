using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main == null) return;

        // look the same way the camera is looking
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
