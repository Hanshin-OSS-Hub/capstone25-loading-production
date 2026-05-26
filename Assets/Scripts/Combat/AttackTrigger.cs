using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [Header("공격 대상 설정")]
    [Tooltip("적 무기라면 'Player', 내 무기라면 'Enemy'라고 입력하세요.")]
    public string targetTag; 

    [Header("공격 데미지")]
    public float damageAmount = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if (targetTag == "Player")
            {
                PlayerHealth pHealth = other.GetComponent<PlayerHealth>();
                if (pHealth != null)
                {
                    pHealth.TakeDamage(damageAmount);
                }
            }
            if (targetTag == "Enemy")
            {
                EnemyHealth eHealth = other.GetComponent<EnemyHealth>();
                if (eHealth != null)
                {
                    eHealth.TakeDamage(damageAmount);
                }
            }
        }
    }
}