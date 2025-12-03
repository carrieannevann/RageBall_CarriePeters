using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damageAmount = 25f;
    public string targetTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        TryDamage(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryDamage(collision.collider);
    }

    private void TryDamage(Collider other)
    {
        if (!other.CompareTag(targetTag))
            return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
            Debug.Log("[EnemyDamage] Hit player for " + damageAmount);
        }
    }
}
