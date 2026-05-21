using UnityEngine;

public class BladeSkillProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float damage = 10f;

    private Vector3 _moveDirection;
    private bool _hasHit;

    public void Initialize(Vector3 direction)
    {
        _moveDirection = direction.normalized;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += _moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasHit)
            return;

        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();

        if (enemyHealth == null)
            return;

        _hasHit = true;
        enemyHealth.TakeDamage(damage);
        Destroy(gameObject);
    }
}