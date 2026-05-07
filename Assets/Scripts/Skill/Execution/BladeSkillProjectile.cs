using UnityEngine;

public class BladeSkillProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 10;

    private Vector3 _moveDirection;

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
        // TODO: 적에게 damage 적용
        ProjectLogger.UI($"Blade 적중: {other.name}, damage={damage}");
        Destroy(gameObject);
    }
}