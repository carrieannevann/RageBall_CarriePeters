using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public float damagePerSecond = 25f;
    public string targetTag = "Player";

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}