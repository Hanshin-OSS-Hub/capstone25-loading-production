using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("플레이어 체력 설정")]
    public float maxHealth = 10f;
    private float currentHealth;

    [Header("플레이어 UI 슬라이더 연결")]
    public Slider Player_HP_Slider;

    private bool isDead = false;

    public bool IsDead => isDead;

    void Start()
    {
        currentHealth = maxHealth;

        if (Player_HP_Slider != null)
        {
            Player_HP_Slider.maxValue = maxHealth;
            Player_HP_Slider.value = currentHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (Player_HP_Slider != null)
        {
            Player_HP_Slider.value = currentHealth;
        }

        Debug.Log($"플레이어 피격! 남은 체력: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            PlayerDie();
        }
    }

    void PlayerDie()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("플레이어 사망: 이동 및 스킬 입력을 정지합니다.");

        PlayerMovement moveScript = GetComponent<PlayerMovement>();

        if (moveScript != null)
        {
            moveScript.Die();
        }
        else
        {
            Debug.LogWarning("PlayerHealth: PlayerMovement를 찾지 못했습니다.");
        }
    }
}