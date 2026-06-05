using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    [Header("추적 설정")]
    public Transform playerTarget;     // 추격할 플레이어 타겟
    public NavMeshAgent agent;        // 네비게이션 제어용 컴포넌트
    public Animator animator;         // 애니메이션 제어용 컴포넌트
    public float detectionRange = 10f; // 플레이어 감지 반경

    [Header("속도 세부 조정")]
    public float patrolSpeed = 0.8f;   // 정찰 이동 속도
    public float chaseSpeed = 5.0f;    // 플레이어 추격 속도

    [Header("배회(순찰) 설정")]
    public float patrolRadius = 8f;    // 정찰 랜덤 반경
    public float minWaitTime = 2f;     // 정찰 최소 대기 시간
    public float maxWaitTime = 5f;     // 정찰 최대 대기 시간

    [Header("전투 및 스킬 설정")]
    public float attackRange = 5.0f;   // 스킬 발동 사정거리
    public float skillCooldown = 5.0f; // 스킬 재사용 대기 시간
    public float attackStopDuration = 1.0f;
    private float cooldownTimer = 0f;  // 쿨타임 타이머
    private bool isAttacking = false;  // 공격 상태 여부

    [Header("적 UI 슬라이더 제어")]
    public Slider Enemy_HP_Slider;     // 실시간 조건부 출력용 슬라이더

    private Vector3 startPosition;     // 최초 배치 위치
    private float waitTimer;           // 대기 시간 측정 타이머
    private bool isWaiting;            // 대기 상태 여부
    private bool isDead = false;       // 사망 여부

    private float sqrDetectionRange;   // 감지 거리의 제곱 값 (연산 최적화)
    private int speedHash;             // 애니메이터 Speed 해시 ID
    private int deadHash;              // 애니메이터 Dead 해시 ID

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        if (agent != null)
        {
            agent.updateRotation = true; 
        }

        // 타겟이 없으면 Player 태그를 가진 오브젝트 자동 탐색
        if (playerTarget == null || playerTarget == transform)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTarget = player.transform;
        }

        // 시작 시 체력바 비활성화
        if (Enemy_HP_Slider != null)
        {
            Enemy_HP_Slider.gameObject.SetActive(false);
        }

        startPosition = transform.position;

        sqrDetectionRange = detectionRange * detectionRange; 
        speedHash = Animator.StringToHash("Speed");          
        deadHash = Animator.StringToHash("Dead");            

        SetRandomPatrolDestination();
    }

    void Update()
    {
        // 사망 상태일 경우 로직 중단
        if (isDead) return;

        if (isAttacking)
        {
            if (agent != null && agent.enabled)
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero;
            }

            if (animator != null)
            {
                animator.SetFloat(speedHash, 0f);
            }

            return;
        }

        // 스킬 쿨타임 타이머 계산
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // 네비게이션 속도 기반 애니메이션 Speed 파라미터 갱신
        if (animator != null && agent != null && agent.enabled)
        {
            animator.SetFloat(speedHash, agent.velocity.magnitude);
        }

        // 플레이어가 없으면 체력바 끄고 리턴
        if (playerTarget == null)
        {
            if (Enemy_HP_Slider != null && Enemy_HP_Slider.gameObject.activeSelf)
            {
                Enemy_HP_Slider.gameObject.SetActive(false);
            }
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        // [조건 1] 스킬 사거리 진입 시 (공격)
        if (distanceToPlayer <= attackRange && cooldownTimer <= 0f)
        {
            if (Enemy_HP_Slider != null && !Enemy_HP_Slider.gameObject.activeSelf)
            {
                Enemy_HP_Slider.gameObject.SetActive(true);
            }

            UseRandomSkill();
            return; 
        }

        Vector3 offset = playerTarget.position - transform.position;
        float sqrDistance = offset.sqrMagnitude;

        // [조건 2] 플레이어 감지 범위 진입 시 (추격)
        if (sqrDistance <= sqrDetectionRange)
        {
            if (Enemy_HP_Slider != null && !Enemy_HP_Slider.gameObject.activeSelf)
            {
                Enemy_HP_Slider.gameObject.SetActive(true);
            }

            isWaiting = false;
            agent.speed = chaseSpeed;  
            agent.SetDestination(playerTarget.position); 
        }
        // [조건 3] 감지 범위를 벗어났을 시 (정찰)
        else
        {
            if (Enemy_HP_Slider != null && Enemy_HP_Slider.gameObject.activeSelf)
            {
                Enemy_HP_Slider.gameObject.SetActive(false);
            }

            PatrolLogic();
        }
    }

    // 랜덤 스킬 발동 함수
    void UseRandomSkill()
    {
        if (animator == null)
            return;

        if (isAttacking)
            return;

        StopAndFacePlayer();

        int randomSkillIndex = Random.Range(0, 2);

        if (randomSkillIndex == 0)
        {
            animator.SetTrigger("Skill");
            Debug.Log("적 스킬 발동: Skill");
        }
        else
        {
            animator.SetTrigger("Triple");
            Debug.Log("적 스킬 발동: Triple");
        }

        cooldownTimer = skillCooldown;
        StartCoroutine(AttackStopRoutine());
    }

    private void StopAndFacePlayer()
    {
        if (agent != null && agent.enabled)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }

        if (animator != null)
        {
            animator.SetFloat(speedHash, 0f);
        }

        if (playerTarget == null)
            return;

        Vector3 lookDirection = playerTarget.position - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude <= 0.001f)
            return;

        transform.rotation = Quaternion.LookRotation(lookDirection.normalized);
    }

    private IEnumerator AttackStopRoutine()
    {
        isAttacking = true;

        yield return new WaitForSeconds(attackStopDuration);

        isAttacking = false;

        if (!isDead && agent != null && agent.enabled)
        {
            agent.isStopped = false;
        }
    }

    // 순찰 상태 제어 로직
    void PatrolLogic()
    {
        if (agent == null || !agent.enabled) return;

        agent.speed = patrolSpeed; 

        // 목적지에 도달했는지 확인
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = Random.Range(minWaitTime, maxWaitTime); 
            }
        }

        // 정찰 대기 타이머 계산 및 작동
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                SetRandomPatrolDestination(); 
            }
        }
    }

    // 랜덤 정찰 목적지 설정 함수
    void SetRandomPatrolDestination()
    {
        if (agent == null || !agent.enabled) return;

        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += startPosition;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    // 외부에서 호출하는 사망 처리 함수
    public void Die()
    {
        if (isDead) return;

        isDead = true;
        isAttacking = false;

        StopAllCoroutines();

        // 체력바 UI 즉시 비활성화
        if (Enemy_HP_Slider != null)
        {
            Enemy_HP_Slider.gameObject.SetActive(false);
        }

        // 네비게이션 즉시 정지 및 비활성화
        if (agent != null)
        {
            if (agent.enabled)
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
            }

            agent.enabled = false;
        }

        // 콜라이더 비활성화
        Collider mainCollider = GetComponent<Collider>();
        if (mainCollider != null)
        {
            mainCollider.enabled = false;
        }

        // 공격/이동 애니메이션을 끊고 즉시 사망 애니메이션으로 전환
        if (animator != null)
        {
            animator.applyRootMotion = true;

            animator.ResetTrigger("Skill");
            animator.ResetTrigger("Triple");
            animator.ResetTrigger("Dead");

            animator.SetFloat(speedHash, 0f);

            // 현재 공격 모션이 끝나기를 기다리지 않고 즉시 Dead 상태로 보냄
            animator.CrossFade("Dead", 0.05f, 0, 0f);
        }
    }
}