using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public Slider healthSlider;   // drag your UI health bar here

    [Header("Events")]
    public UnityEvent onDeath;    // hook your "You Lose" here

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    // === Damage APIs ===

    // Works with the enemy script's fraction call (e.g., 0.25f)
    public void TakeFractionDamage(float fraction)
    {
        fraction = Mathf.Clamp01(fraction);
        float amount = maxHealth * fraction;
        TakeDamage(amount);
    }

    // Original float version (kept)
    public void TakeDamage(float amount)
    {
        if (amount <= 0f) return;

        currentHealth = Mathf.Max(0f, currentHealth - amount);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (currentHealth <= 0f)
            Die();
    }

    // Overload so SendMessage("TakeDamage", int) also works
    public void TakeDamage(int amount)
    {
        TakeDamage((float)amount);
    }

    // === Death handling ===
    void Die()
    {
        Debug.Log("PLAYER DIED");

        // optional: stop player movement
        var rb = GetComponent<Rigidbody>();
        if (rb) rb.linearVelocity = Vector3.zero;

        // disable your movement script if you have one
        var mover = GetComponent<PlayerController>(); // rename if yours differs
        if (mover) mover.enabled = false;

        // fire UI event
        if (onDeath != null) onDeath.Invoke();
    }
}
