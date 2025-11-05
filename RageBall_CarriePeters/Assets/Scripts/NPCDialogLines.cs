using UnityEngine;
using TMPro;

public class NPCDialogLines : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogText;
    [TextArea]
    [SerializeField] private string[] lines;

    private int currentIndex = 0;
    private bool playerInRange = false;

    private void OnEnable()
    {
        currentIndex = 0;
        ShowCurrentLine();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            NextLine();
        }
    }

    public void SetPlayerInRange(bool value)
    {
        playerInRange = value;
    }

    private void ShowCurrentLine()
    {
        if (dialogText == null) return;

        if (currentIndex >= 0 && currentIndex < lines.Length)
            dialogText.text = lines[currentIndex];
        else
            dialogText.text = "";
    }

    private void NextLine()
    {
        currentIndex++;
        if (currentIndex >= lines.Length)
        {
            // reached the end, you can hide the UI here if you want
            gameObject.SetActive(false);
        }
        else
        {
            ShowCurrentLine();
        }
    }
}
