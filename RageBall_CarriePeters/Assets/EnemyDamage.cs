using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Tooltip("Fraction of the player's MAX health to remove on hit.")]
    public float damageFraction = 0.25f;   // 1/4 HP

    public float knockbackForce = 6f;
    public string playerTag = "Player";

    // if your enemies use triggers instead of colliders, keep both
    private void OnCollisionEnter(Collision collision)
    {
        TryHitPlayer(collision.gameObject, collision.contacts.Length > 0 ? collision.contacts[0].point : transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryHitPlayer(other.gameObject, other.transform.position);
    }

    private void TryHitPlayer(GameObject other, Vector3 hitPoint)
    {
        if (!other.CompareTag(playerTag))
            return;

        // find the health on the player
        PlayerHealth health =
            other.GetComponent<PlayerHealth>() ??
            other.GetComponentInParent<PlayerHealth>() ??
            other.GetComponentInChildren<PlayerHealth>();

        if (health != null)
        {
            // 1/4 of MAX health
            float dmg = health.maxHealth * damageFraction;
            health.TakeDamage(dmg);
        }

        // knockback
        Rigidbody rb =
            other.GetComponent<Rigidbody>() ??
            other.GetComponentInParent<Rigidbody>() ??
            other.GetComponentInChildren<Rigidbody>();

        if (rb != null)
        {
            // push player away from enemy
            Vector3 dir = (other.transform.position - transform.position).normalized;
            dir.y = 0.25f; // tiny upward so it feels nicer
            rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
        }
    }
}
