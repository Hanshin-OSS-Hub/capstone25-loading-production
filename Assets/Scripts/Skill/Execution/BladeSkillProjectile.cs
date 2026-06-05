using System.Collections.Generic;
using UnityEngine;

public class BladeSkillProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float damage = 3f;

    private Vector3 _moveDirection;
    private HashSet<EnemyHealth> _sharedHitEnemies;

    public void Initialize(Vector3 direction)
    {
        Initialize(direction, null);
    }

    public void Initialize(Vector3 direction, HashSet<EnemyHealth> sharedHitEnemies)
    {
        _moveDirection = direction.normalized;
        _sharedHitEnemies = sharedHitEnemies;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += _moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();

        if (enemyHealth == null)
            return;

        if (_sharedHitEnemies != null && !_sharedHitEnemies.Add(enemyHealth))
        {
            Destroy(gameObject);
            return;
        }

        enemyHealth.TakeDamage(damage);
        Destroy(gameObject);
    }
}