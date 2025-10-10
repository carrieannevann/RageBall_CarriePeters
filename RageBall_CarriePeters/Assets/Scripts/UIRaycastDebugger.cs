using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIRaycastDebugger : MonoBehaviour {
    public GraphicRaycaster raycaster;    // drag Pause Canvas' GraphicRaycaster here
    public EventSystem eventSystem;      // drag EventSystem here

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            PointerEventData ped = new PointerEventData(eventSystem);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(ped, results);
            if (results.Count == 0) Debug.Log("UIRaycastDebugger: no UI hit at mouse position.");
            else {
                Debug.Log("UIRaycastDebugger: UI hits:");
                foreach (var r in results) Debug.Log(" - " + r.gameObject.name + " (depth " + r.depth + ")");
            }
        }
    }
}
