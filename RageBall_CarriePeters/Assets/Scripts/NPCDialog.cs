using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private GameObject dialogUI;
    [SerializeField] private string playerTag = "Player";

    private NPCDialogLines dialogLines;

    private void Start()
    {
        if (dialogUI != null)
        {
            dialogUI.SetActive(false);
            dialogLines = dialogUI.GetComponent<NPCDialogLines>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && dialogUI != null)
        {
            dialogUI.SetActive(true);

            if (dialogLines != null)
                dialogLines.SetPlayerInRange(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) && dialogUI != null)
        {
            if (dialogLines != null)
                dialogLines.SetPlayerInRange(false);

            dialogUI.SetActive(false);
        }
    }
}
