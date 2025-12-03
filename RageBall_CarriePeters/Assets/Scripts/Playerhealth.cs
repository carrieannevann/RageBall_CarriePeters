using UnityEngine;
using UnityEngine.UI;   // for Slider

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [Header("Health Settings")]
    public float maxHealth = 100f;       // total HP
    public float currentHealth;          // current HP

    [Header("UI")]
    public Slider healthSlider;          // drag your health bar Slider here
    public GameObject losePanel;         // drag your Lose Panel here

    [Header("Death Behaviour")]
    public bool pauseOnDeath = true;     // freeze the game on death
    public bool showCursorOnDeath = true;// show mouse for UI when dead

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // start at full health
        currentHealth = maxHealth;
        UpdateHealthUI();

        // hide lose panel at the start
        if (losePanel != null)
            losePanel.SetActive(false);

        // make sure game is running at normal speed when we start
        Time.timeScale = 1f;
    }

    // --------- THIS IS THE IMPORTANT PART ----------
    // TakeDamage now accepts a FLOAT, not an INT.
    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0f)
            return; // already dead

        currentHealth -= amount;
        if (currentHealth < 0f)
            currentHealth = 0f;

        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    // ------------------------------------------------

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            // assumes slider Min = 0, Max = 1
            healthSlider.value = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        // 1) stop movement script if you have one
        var controller = GetComponent<PlayerController>();
        if (controller != null)
            controller.enabled = false;

        // 2) stop physics movement
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = Vector3.zero;

        // 3) show lose panel
        if (losePanel != null)
            losePanel.SetActive(true);

        // 4) pause game (optional)
        if (pauseOnDeath)
            Time.timeScale = 0f;

        // 5) show cursor so you can click buttons
        if (showCursorOnDeath)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Debug.Log("[PlayerHealth] Player died.");
    }

    // Optional heal method if you want pickups later
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        UpdateHealthUI();
    }
}
