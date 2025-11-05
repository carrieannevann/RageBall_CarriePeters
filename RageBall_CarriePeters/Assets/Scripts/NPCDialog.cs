using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private GameObject dialogUI;   // assign the Canvas here
    [SerializeField] private string playerTag = "Player";

    private void Start()
    {
        if (dialogUI != null)
            dialogUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && dialogUI != null)
        {
            dialogUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) && dialogUI != null)
        {
            dialogUI.SetActive(false);
        }
    }
}
