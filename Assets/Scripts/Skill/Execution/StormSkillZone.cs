using UnityEngine;

public class StormSkillZone : MonoBehaviour
{
    [SerializeField] private float duration = 3f;
    [SerializeField] private float tickInterval = 0.5f;
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private float pullStrength = 2f;
    [SerializeField] private int tickDamage = 5;

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

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                Vector3 dirToCenter = (transform.position - hit.transform.position).normalized;
                hit.attachedRigidbody.AddForce(dirToCenter * pullStrength, ForceMode.Acceleration);
            }

            // TODO: 적 판별 후 실제 도트 대미지 처리
            ProjectLogger.UI($"Storm 영향 대상: {hit.name}, tickDamage={tickDamage}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}