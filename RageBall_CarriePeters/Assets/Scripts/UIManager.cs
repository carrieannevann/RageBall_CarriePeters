// UIManager.cs
using UnityEngine;
using TMPro;
using System.Diagnostics; // for StackTrace (debug tracing)

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI (TextMeshPro)")]
    public TextMeshProUGUI pickupTextTMP;   // assign PickupCounterText here

    [Header("Win UI")]
    public GameObject winText;              // drag your "WinText" UI object here

    [Header("Lose UI")]
    public GameObject losePanel;            // drag your "You Lose" panel here
    public bool pauseOnLose = true;         // pause when lose is shown
    public bool showCursorOnLose = true;    // unlock cursor on lose

    [Header("Optional Settings")]
    public bool autoCountPickupsAtStart = true;

    int remainingPickups = 0;

    // Gate to prevent duplicate lose calls (and to stop spammy errors)
    bool _loseShown = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    void Start()
    {
        // make sure win/lose start hidden
        if (winText != null) winText.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        if (autoCountPickupsAtStart)
        {
            GameObject[] pickups = GameObject.FindGameObjectsWithTag("PickUp");
            remainingPickups = pickups != null ? pickups.Length : 0;
            UnityEngine.Debug.Log($"[UIManager] Found {remainingPickups} objects tagged 'PickUp' at Start.");
        }
        UpdateUI();
    }

    public void OnPickupCollected()
    {
        UnityEngine.Debug.Log("[UIManager] OnPickupCollected() called. remaining before = " + remainingPickups);
        remainingPickups = Mathf.Max(0, remainingPickups - 1);
        UpdateUI();
        UnityEngine.Debug.Log("[UIManager] remaining after = " + remainingPickups);
        if (remainingPickups == 0) LevelComplete();
    }

    public void AddPickup(int amount = 1)
    {
        remainingPickups += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (pickupTextTMP != null)
        {
            pickupTextTMP.text = $"Souls: {remainingPickups}";
        }
        else
        {
            UnityEngine.Debug.LogWarning("[UIManager] pickupTextTMP not assigned in Inspector!");
        }
    }

    void LevelComplete()
    {
        UnityEngine.Debug.Log("[UIManager] All pickups collected!");
        if (winText != null) winText.SetActive(true);
        else UnityEngine.Debug.LogWarning("[UIManager] winText not assigned in Inspector!");
    }

    // ---------- LOSE UI ----------
    // Called by PlayerHealth.onDeath (wire in Inspector), NOT by enemies/lava.
    public void ShowLoseUI()
    {
        if (_loseShown) return;      // ignore duplicates
        _loseShown = true;

        // Print who called this (helps find any rogue "auto lose" triggers)
        UnityEngine.Debug.LogError("[ShowLoseUI] CALLED BY:\n" + new StackTrace(1, true));

        if (!losePanel)
        {
            UnityEngine.Debug.LogWarning("[UIManager] losePanel is missing or was destroyed.");
            return;
        }

        losePanel.SetActive(true);

        if (pauseOnLose) Time.timeScale = 0f;
        if (showCursorOnLose)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        // debug helper to test death flow quickly (optional)
        // if (Input.GetKeyDown(KeyCode.L)) ShowLoseUI();

        if (Input.GetKeyDown(KeyCode.K)) OnPickupCollected();
    }
}
