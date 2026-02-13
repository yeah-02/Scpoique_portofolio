using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class Player_CC_Full : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float gravity = -9.81f;
    public float bobAmplitude = 0.05f;
    public float bobFrequency = 8f;

    [Header("Mouse Sensitivity")]
    public float SensitivityX = 60f;
    public float SensitivityY = 60f;

    [Header("Camera")]
    public Transform cameraPivot;

    [Header("NavMesh Check")]
    public float navSampleDistance = 0.5f;

    private CharacterController cc;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    private Vector3 originalCameraPos;
    private float bobTimer = 0f;

    // ★ 추가: 이동량 추적용
    private Vector3 lastPosition;
    private float flatSpeed;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        originalCameraPos = cameraPivot.localPosition;
        Cursor.lockState = CursorLockMode.Locked;

        lastPosition = transform.position; // ★
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleHeadBob();
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 input = transform.forward * v + transform.right * h;

        Vector3 move = Vector3.zero;

        if (input.sqrMagnitude > 0.01f)
        {
            move = input.normalized * moveSpeed * Time.deltaTime;

            // NavMesh 범위 체크
            NavMeshHit hit;
            Vector3 targetPos = cc.transform.position + move;

            if (NavMesh.SamplePosition(targetPos, out hit, navSampleDistance, NavMesh.AllAreas))
            {
                Vector3 nextPos = hit.position;
                nextPos.y = cc.transform.position.y;
                cc.Move(nextPos - cc.transform.position);
            }
        }

        // 중력 처리
        if (cc.isGrounded && velocity.y < 0)
            velocity.y = -0.5f;
        else
            velocity.y += gravity * Time.deltaTime;

        cc.Move(velocity * Time.deltaTime);

        // ★ 실제 수평 이동 속도 계산 (바운싱용)
        Vector3 flatMove = transform.position - lastPosition;
        flatMove.y = 0;
        flatSpeed = flatMove.magnitude / Time.deltaTime;
        lastPosition = transform.position;
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * SensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * SensitivityY * Time.deltaTime;

        // 좌우 회전
        transform.Rotate(Vector3.up * mouseX);

        // 상하 회전
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -30f, 30f);
        cameraPivot.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    void HandleHeadBob()
    {
        // ★ 기존 cc.velocity 대신 실제 frame 기반 이동속도 사용
        if (flatSpeed > 0.1f)
        {
            bobTimer += Time.deltaTime * bobFrequency;

            float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;

            Vector3 camPos = originalCameraPos;
            camPos.y += bobOffset;

            cameraPivot.localPosition = camPos;
        }
        else
        {
            // ★ 완전 리셋 대신 자연스럽게 0으로 회귀
            bobTimer = 0f;
            cameraPivot.localPosition = Vector3.Lerp(
                cameraPivot.localPosition,
                originalCameraPos,
                Time.deltaTime * 8f
            );
        }
    }
}
