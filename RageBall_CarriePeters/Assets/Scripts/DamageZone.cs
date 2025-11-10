using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public float damagePerSecond = 25f;
    public string targetTag = "Player";

    // ------------- TRIGGERS -------------
    private void OnTriggerStay(Collider other)
    {
        TryDamage(other.gameObject);
    }

    // ------------- COLLISIONS -------------
    private void OnCollisionStay(Collision collision)
    {
        TryDamage(collision.gameObject);
    }

    private void TryDamage(GameObject other)
    {
        // tag check
        if (!other.CompareTag(targetTag))
            return;

        // try to find PlayerHealth on this object or nearby
        PlayerHealth health =
            other.GetComponent<PlayerHealth>() ??
            other.GetComponentInParent<PlayerHealth>() ??
            other.GetComponentInChildren<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(damagePerSecond * Time.deltaTime);
        }
        else
        {
            // uncomment to debug
            // Debug.LogWarning("DamageZone: found Player tag but no PlayerHealth on " + other.name);
        }
    }
}
