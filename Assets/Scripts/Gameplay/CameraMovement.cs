using UnityEngine;

// 플레이어 이동(Update)이 모두 완료된 시점의 최종 위치를 오차 없이 추적하기 위해 실행 순서를 보장합니다.
[DefaultExecutionOrder(100)]
public class CameraMovement : MonoBehaviour
{
    [Header("추적 대상")]
    public Transform target;           // 카메라가 따라다닐 타겟 (플레이어)

    [Header("마인크래프트식 3인칭 시점 설정")]
    public Vector3 offset = new Vector3(0f, 0f, 3f); // 타겟 중심점으로부터의 거리 및 높이 오프셋 (X값 조절 시 숄더뷰 가능)

    [Header("마우스 화면 회전 설정")]
    public bool useMouseRotation = true; // 마우스 이동에 따른 카메라 회전 활성화 여부
    public float rotationSpeed = 2f;    // 마우스 감도 (회전 속도)
    public float minPitch = -10f;       // 아래를 내려다볼 수 있는 최대 제한 각도
    public float maxPitch = 60f;        // 위를 올려다볼 수 있는 최대 제한 각도

    private float shakeAmount;
    private float shakeTimer;

    private float pitch = 0f;           // 수직 회전 축 누적 값 (X축 회전)

    // 플레이어 스크립트가 프레임 어긋남 없이 참조할 수 있도록 수평 회전 값을 public 프로퍼티로 제공
    public float Yaw { get; private set; } = 0f;

    void Start()
    {
        // 타겟이 설정되지 않은 경우 'Player' 태그를 가진 오브젝트를 자동으로 찾아 할당
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }

        // 게임 시작 시 현재 카메라의 초기 회전각을 마우스 제어 변수에 동기화
        Vector3 angles = transform.eulerAngles;
        Yaw = angles.y;
        pitch = angles.x;

        // 마우스 커서를 화면 중앙에 고정하고 숨겨 마우스 조작이 튀는 현상을 방지
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 마우스 입력(입력 시스템 렉 방지)은 매 프레임 가장 신속하게 반응하는 일반 Update에서 처리합니다.
    void Update()
    {
        if (useMouseRotation)
        {
            Yaw += Input.GetAxis("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch); // 상하 각도 제한 적용
        }
    }

    // 캐릭터 컨트롤러의 이동 연산이 완전히 끝난 직후, 실시간으로 변화된 플레이어의 좌표를 '칼같이' 동기화합니다.
    // 키 연타나 급격한 프레임 드랍 시 캐릭터가 카메라 화면 밖으로 탈출하는 현상을 완벽하게 차단합니다.
    void LateUpdate()
    {
        if (target == null) return;

        // 마우스 입력 기반의 최종 쿼터니언 회전값 생성
        Quaternion cameraRotation = Quaternion.Euler(pitch, Yaw, 0f);
        
        // 캐릭터의 발밑이 아닌 허리/골반 부근을 카메라의 명확한 주시 목표점으로 설정
        Vector3 targetCenter = target.position + Vector3.up * 1.0f;
        
        // 회전 쿼터니언에 X, Y, Z 오프셋 전체를 곱하여 카메라가 위치해야 할 상대적 방향 연산
        Vector3 calculatedOffset = cameraRotation * new Vector3(offset.x, offset.y, -offset.z);
        
        Vector3 finalPosition = targetCenter + calculatedOffset;

        if (shakeTimer > 0f)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeAmount;
            finalPosition += shakeOffset;

            shakeTimer -= Time.unscaledDeltaTime;
        }
        else
        {
            shakeAmount = 0f;
        }

        // 지연 시간 보간을 거치지 않고, 이동이 완료된 플레이어의 위치에 카메라 좌표를 1:1로 즉시 강제 대입
        transform.position = finalPosition;
        
        // 카메라 회전 값을 계산된 마우스 입력값과 완벽하게 일치시킴
        transform.rotation = cameraRotation;
    }

    public void Shake(float amount, float duration)
    {
        shakeAmount = Mathf.Max(shakeAmount, amount);
        shakeTimer = Mathf.Max(shakeTimer, duration);
    }
}