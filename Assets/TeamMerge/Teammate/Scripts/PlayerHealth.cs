using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("플레이어 체력 설정")]
    public float maxHealth = 10f;       // 플레이어의 최대 체력
    private float currentHealth;        // 실시간 현재 체력

    [Header("플레이어 UI 슬라이더 연결")]
    public Slider Player_HP_Slider;     // 화면에 표시되는 Player_HP_Slider UI

    private bool isDead = false;        // 중복 사망 방지용 플래그

    void Start()
    {
        // 게임 시작 시 현재 체력을 최대 체력으로 초기화
        currentHealth = maxHealth;      

        // 슬라이더 UI가 연결되어 있다면 최대 눈금과 현재 눈금을 체력에 동기화
        if (Player_HP_Slider != null)
        {
            Player_HP_Slider.maxValue = maxHealth;
            Player_HP_Slider.value = currentHealth;
        }
    }

    /// <summary>
    /// 외부(AttackTrigger 등)에서 플레이어에게 데미지를 줄 때 호출하는 함수입니다.
    /// </summary>
    /// <param name="damage">차감할 데미지 수량</param>
    public void TakeDamage(float damage)
    {
        // 이미 사망한 상태라면 피격 계산을 무시합니다.
        if (isDead) return; 

        // 체력 차감 및 UI 슬라이더 실시간 반영
        currentHealth -= damage;        
        if (Player_HP_Slider != null)
        {
            Player_HP_Slider.value = currentHealth;
        }

        Debug.Log($"플레이어 피격! 남은 체력: {currentHealth}/{maxHealth}");

        // 체력이 0 이하가 되면 사망 처리를 시작합니다.
        if (currentHealth <= 0)
        {
            PlayerDie();
        }
    }

    /// <summary>
    /// 플레이어의 체력이 0이 되었을 때 내부적으로 호출되는 사망 처리 함수입니다.
    /// </summary>
    void PlayerDie()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("플레이어 사망: 모든 이동 및 마우스 회전 제어를 정지합니다.");

        // PlayerMovement 스크립트를 찾아 이동 정지 및 사망 애니메이션(Dead)을 실행시킵니다.
        PlayerMovement_ moveScript = GetComponent<PlayerMovement_>();
        if (moveScript != null)
        {
            moveScript.Die();
        }
    }
}