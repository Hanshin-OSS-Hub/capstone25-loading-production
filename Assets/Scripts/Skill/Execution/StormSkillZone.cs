using System.Collections.Generic;
using UnityEngine;

public class StormSkillZone : MonoBehaviour
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private float damageInterval = 1f;
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private float pullStrength = 2f;
    [SerializeField] private float damagePerSecond = 5f;

    private float _damageTimer;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        _damageTimer += Time.deltaTime;

        if (_damageTimer >= damageInterval)
        {
            _damageTimer = 0f;
            ApplyStormEffect();
        }
    }

    private void ApplyStormEffect()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        HashSet<EnemyHealth> damagedEnemies = new HashSet<EnemyHealth>();

        foreach (Collider hit in hits)
        {
            EnemyHealth enemyHealth = hit.GetComponentInParent<EnemyHealth>();

            if (enemyHealth == null)
                continue;

            if (hit.attachedRigidbody != null)
            {
                Vector3 dirToCenter = (transform.position - hit.transform.position).normalized;
                hit.attachedRigidbody.AddForce(dirToCenter * pullStrength, ForceMode.Acceleration);
            }

            if (damagedEnemies.Add(enemyHealth))
            {
                enemyHealth.TakeDamage(damagePerSecond);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}