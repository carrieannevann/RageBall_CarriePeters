using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyDamage : MonoBehaviour
{
    [Header("Damage")]
    [Range(0f, 1f)] public float damageFraction = 0.25f; // 1/4 max HP
    public string playerTag = "Player";
    public float hitCooldown = 0.6f;

    [Header("Knockback")]
    public float knockbackForce = 6f;
    public float knockbackUpward = 0.75f;

    private float _lastHitTime = -999f;

    private void Reset()
    {
        Collider c = GetComponent<Collider>();
        if (c != null) c.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryHit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        // Delete this method if you want only one hit per touch.
        TryHit(other);
    }

    private void TryHit(Component other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (Time.time - _lastHitTime < hitCooldown) return;
        _lastHitTime = Time.time;

        // ---- DAMAGE ----
        // Try fraction-based methods if your player has them:
        other.SendMessage("TakeFractionDamage", damageFraction, SendMessageOptions.DontRequireReceiver);
        other.SendMessage("ApplyFractionDamage", damageFraction, SendMessageOptions.DontRequireReceiver);

        // Otherwise compute an absolute amount (defaults to 25 if no maxHealth found):
        int amount = 25;
        int max = TryGetMaxHealthFromComponents((other as Component).gameObject);
        if (max > 0) amount = Mathf.CeilToInt(max * damageFraction);

        other.SendMessage("TakeDamage", amount, SendMessageOptions.DontRequireReceiver);
        other.SendMessage("ApplyDamage", amount, SendMessageOptions.DontRequireReceiver);

        // ---- KNOCKBACK ----
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();
        if (rb == null) rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (rb.worldCenterOfMass - transform.position).normalized;
            Vector3 force = dir * knockbackForce + Vector3.up * knockbackUpward;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

    private int TryGetMaxHealthFromComponents(GameObject go)
    {
        Component[] comps = go.GetComponentsInParent<Component>(true);
        for (int i = 0; i < comps.Length; i++)
        {
            Component c = comps[i];
            if (c == null) continue;
            System.Type t = c.GetType();

            var f = t.GetField("maxHealth");
            if (f != null)
            {
                object v = f.GetValue(c);
                if (v is int) return (int)v;
                if (v is float) return Mathf.RoundToInt((float)v);
            }

            var p = t.GetProperty("maxHealth");
            if (p != null && p.CanRead)
            {
                object v = p.GetValue(c, null);
                if (v is int) return (int)v;
                if (v is float) return Mathf.RoundToInt((float)v);
            }
        }
        return -1;
    }
}
