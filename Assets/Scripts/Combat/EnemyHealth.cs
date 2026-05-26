using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("적 체력 설정")]
    public float maxHealth = 20f;       
    private float currentHealth;        

    [Header("적 UI 슬라이더 연결")]
    public Slider Enemy_HP_Slider;     

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;      

        if (Enemy_HP_Slider != null)
        {
            Enemy_HP_Slider.maxValue = maxHealth;
            Enemy_HP_Slider.value = currentHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;        

        if (Enemy_HP_Slider != null)
        {
            Enemy_HP_Slider.value = currentHealth;
        }

        Debug.Log($"적 피격! 남은 체력: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // 체력바 비활성화
        if (Enemy_HP_Slider != null)
        {
            Enemy_HP_Slider.gameObject.SetActive(false);
        }

        // 이동 스크립트의 사망 로직 호출
        EnemyMovement movementScript = GetComponent<EnemyMovement>();
        if (movementScript != null)
        {
            movementScript.Die(); 
        }

        // 3D 콜라이더 비활성화 (플레이어 충돌 방지)
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // 2D 콜라이더 비활성화
        Collider2D[] colliders2D = GetComponents<Collider2D>();
        foreach (Collider2D col2D in colliders2D)
        {
            col2D.enabled = false;
        }
    }
}