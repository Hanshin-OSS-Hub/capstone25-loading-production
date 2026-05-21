using System.Collections.Generic;
using UnityEngine;

public class StormSkillZone : MonoBehaviour
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private float tickInterval = 0.5f;
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private float pullStrength = 2f;
    [SerializeField] private float tickDamage = 5f;

    private float _tickTimer;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        _tickTimer += Time.deltaTime;

        if (_tickTimer >= tickInterval)
        {
            _tickTimer = 0f;
            ApplyStormEffect();
        }
    }

    private void ApplyStormEffect()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        HashSet<EnemyHealth> damagedEnemies = new HashSet<EnemyHealth>();

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                Vector3 dirToCenter = (transform.position - hit.transform.position).normalized;
                hit.attachedRigidbody.AddForce(dirToCenter * pullStrength, ForceMode.Acceleration);
            }

            EnemyHealth enemyHealth = hit.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null && damagedEnemies.Add(enemyHealth))
            {
                enemyHealth.TakeDamage(tickDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}