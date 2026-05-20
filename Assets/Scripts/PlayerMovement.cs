using UnityEngine;

[DefaultExecutionOrder(-100)] 
public class PlayerMovement : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public CharacterController controller; // 캐릭터 컨트롤러 컴포넌트
    public Animator animator;              // 애니메이터 컴포넌트
    
    [Header("이동 속도 세부 설정")]
    public float walkSpeed = 3f;           // 걷기 속도
    public float runSpeed = 6f;            // 달리기 속도
    public float gravity = -9.81f;         // 중력 가속도

    [Header("점프 설정")]
    public float jumpHeight = 2f;          // 점프 높이
    public float groundDistance = 1.1f;    // 바닥 체크 레이캐스트 거리
    public LayerMask groundMask;           // 바닥 레이어 마스크
    
    Vector3 velocity;                      // 수직 속도 벡터
    private bool isJumping = false;        // 점프 여부
    private bool isDead = false;           // 사망 여부

    private int speedHash;                 // 애니메이터 Speed 해시 ID
    private CameraMovement cameraScript;   // 카메라 스크립트 참조

    void Start()
    {
        if (controller == null) controller = GetComponent<CharacterController>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        speedHash = Animator.StringToHash("Speed");

        if (Camera.main != null)
        {
            cameraScript = Camera.main.GetComponent<CameraMovement>();
        }
    }

    void Update()
    {
        // 사망 상태 검사
        if (isDead) return;

        // 바닥 착지 체크
        bool isGroundedRay = Physics.Raycast(transform.position, Vector3.down, groundDistance, groundMask);
        bool isGrounded = controller.isGrounded || isGroundedRay;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
            isJumping = false; 
        }

        // 입력 값 받기
        float x = Input.GetAxisRaw("Horizontal"); 
        float z = Input.GetAxisRaw("Vertical");   

        // 카메라 시점 기준 이동 방향 축 계산
        Vector3 moveInput = Vector3.zero;
        if (Camera.main != null)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            moveInput = (camForward * z) + (camRight * x);
        }
        
        // 대각선 이동 속도 보정
        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }

        // 이동 속도 결정 및 애니메이터 제어
        float targetSpeed = 0f;
        if (moveInput.magnitude > 0.1f) 
        {
            targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; 
        }

        if (animator != null)
        {
            animator.SetFloat(speedHash, targetSpeed); 
        }

        // 최종 수평 이동 연산
        Vector3 finalMove = moveInput * targetSpeed;

        // 점프 처리
        if (Input.GetButtonDown("Jump") && isGrounded && !isJumping)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true; 
        }

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;

        // 최종 캐릭터 이동 적용
        Vector3 finalVelocity = finalMove + velocity;
        controller.Move(finalVelocity * Time.deltaTime);
    }

    void LateUpdate()
    {
        if (isDead) return;

        // 카메라 회전값과 동기화
        if (cameraScript != null)
        {
            transform.rotation = Quaternion.Euler(0f, cameraScript.Yaw, 0f);
        }
    }

    // 외부에서 호출하는 사망 처리 메서드
    public void Die()
    {
        if (isDead) return;
        isDead = true;
        
        // 사망 애니메이션 재생 및 루트 모션 강제 활성화
        if (animator != null)
        {
            animator.applyRootMotion = true; 
            animator.SetTrigger("Dead"); 
            animator.SetFloat(speedHash, 0f);
        }

        velocity = Vector3.zero;

        // 캐릭터 컨트롤러 비활성화
        if (controller != null) controller.enabled = false; 
    }
}