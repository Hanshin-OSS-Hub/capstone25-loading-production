using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StormSkillZone : MonoBehaviour
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private float damageInterval = 0.3f;
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private float pullStrength = 0.35f;
    [SerializeField] private float damagePerTick = 1f;

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
        HashSet<EnemyHealth> affectedEnemies = new HashSet<EnemyHealth>();

        foreach (Collider hit in hits)
        {
            EnemyHealth enemyHealth = hit.GetComponentInParent<EnemyHealth>();

            if (enemyHealth == null)
                continue;

            if (!affectedEnemies.Add(enemyHealth))
                continue;

            enemyHealth.TakeDamage(damagePerTick);
            PullEnemySlightly(enemyHealth.transform);
        }
    }

    private void PullEnemySlightly(Transform enemyTransform)
    {
        if (enemyTransform == null)
            return;

        Vector3 directionToCenter = transform.position - enemyTransform.position;
        directionToCenter.y = 0f;

        if (directionToCenter.sqrMagnitude <= 0.001f)
            return;

        Vector3 pullOffset = directionToCenter.normalized * pullStrength;

        NavMeshAgent agent = enemyTransform.GetComponent<NavMeshAgent>();

        if (agent != null && agent.enabled)
        {
            agent.Move(pullOffset);
            return;
        }

        CharacterController controller = enemyTransform.GetComponent<CharacterController>();

        if (controller != null && controller.enabled)
        {
            controller.Move(pullOffset);
            return;
        }

        Rigidbody rigidbody = enemyTransform.GetComponent<Rigidbody>();

        if (rigidbody != null)
        {
            rigidbody.MovePosition(rigidbody.position + pullOffset);
            return;
        }

        enemyTransform.position += pullOffset;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}