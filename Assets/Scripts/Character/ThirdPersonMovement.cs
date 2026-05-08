using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float gravity = -9.81f;

    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private DashSkillRunner dashRunner;

    private Vector3 velocity;

    private void Reset()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        dashRunner = GetComponent<DashSkillRunner>();
    }

    private void Awake()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (dashRunner == null)
            dashRunner = GetComponent<DashSkillRunner>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        Move();
        ApplyGravity();
        UpdateAnimator();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(h, 0f, v);

        if (inputDirection.sqrMagnitude < 0.01f)
            return;

        if (cameraTransform == null)
        {
            Debug.LogWarning("ThirdPersonMovement: cameraTransform이 연결되지 않았습니다.");
            return;
        }

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * v + cameraRight * h;
        moveDirection.Normalize();

        float finalSpeed = moveSpeed;
        if (dashRunner != null)
            finalSpeed *= dashRunner.GetSpeedBuffMultiplier();

        controller.Move(moveDirection * finalSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateAnimator()
    {
        if (animator == null)
            return;

        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        float speed = horizontalVelocity.magnitude;
        // animator.SetFloat("Speed", speed);
    }
}