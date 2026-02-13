using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EyeballBehavior : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Camera playerCamera;

    public Transform eyeballBody;
    public Transform innerEyeball;
    public Light eyeballSpotLight;

    [Header("Chase Settings")]
    public float chaseSpeed = 2.5f;     // 요청 반영
    public float stopDistance = 3.2f;   // 요청 반영

    [Header("Retreat Settings")]
    public float retreatDistance = 5f;
    public float pauseBeforeFade = 0.3f;
    public float fadeSpeed = 1f;
    public float reappearDelay = 1f;    // 요청 반영

    [Header("Eyeball Height")]
    public float fixedHeight = 1.2f;    // 요청 반영

    private NavMeshAgent agent;
    private bool isRetreating = false;
    private Renderer[] renderers;
    private float originalSpotIntensity;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = true;
        agent.speed = chaseSpeed;

        renderers = GetComponentsInChildren<Renderer>();

        if (eyeballSpotLight != null)
            originalSpotIntensity = eyeballSpotLight.intensity;

        MaintainFixedHeight();
    }

    void Update()
    {
        MaintainFixedHeight();
        RotateEyeballBody();
        RotateInnerEyeball();

        if (!isRetreating)
        {
            if (IsInPlayerView())
            {
                StartCoroutine(RetreatAndFade());
                return;
            }

            ChasePlayer();
        }
    }

    void MaintainFixedHeight()
    {
        Vector3 pos = transform.position;
        pos.y = fixedHeight;
        transform.position = pos;
    }

    void ChasePlayer()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < stopDistance)
        {
            agent.isStopped = true;
            return;
        }

        agent.isStopped = false;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(player.position, out hit, 1f, NavMesh.AllAreas))
            agent.SetDestination(hit.position);

        agent.speed = chaseSpeed;
    }

    void RotateEyeballBody()
    {
        if (!eyeballBody) return;

        Vector3 dir = (player.position - eyeballBody.position).normalized;
        dir.y = 0;

        if (dir != Vector3.zero)
            eyeballBody.rotation = Quaternion.LookRotation(dir);
    }

    void RotateInnerEyeball()
    {
        if (!innerEyeball) return;
        innerEyeball.LookAt(player.position);
    }

    bool IsInPlayerView()
    {
        Vector3 vp = playerCamera.WorldToViewportPoint(transform.position);

        if (vp.z < 0) return false;
        if (vp.x < 0 || vp.x > 1) return false;
        if (vp.y < 0 || vp.y > 1) return false;

        Vector3 toEyeball = (transform.position - playerCamera.transform.position).normalized;
        float dot = Vector3.Dot(playerCamera.transform.forward, toEyeball);

        return dot > 0.6f;
    }

    IEnumerator RetreatAndFade()
    {
        if (isRetreating) yield break;
        isRetreating = true;

        // Fade 중 이동 금지
        agent.isStopped = true;

        yield return new WaitForSeconds(pauseBeforeFade);

        // 후퇴 위치 계산
        Vector3 dir = (transform.position - player.position).normalized;
        Vector3 retreatPos = transform.position + dir * retreatDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(retreatPos, out hit, 2f, NavMesh.AllAreas))
            retreatPos = hit.position;

        // Fade Out: 여기서 랜덤 속도 1번만 생성 → 모든 요소 동일
        float randomFadeSpeed = fadeSpeed * Random.Range(0.7f, 1.3f);
        yield return StartCoroutine(FadeTo(0f, randomFadeSpeed));

        // 완전히 투명해진 후에만 이동
        agent.Warp(retreatPos);
        MaintainFixedHeight();

        // 플레이어의 시야에서 사라질 때까지 대기
        yield return new WaitUntil(() => !IsInPlayerView());
        yield return new WaitForSeconds(reappearDelay);

        // Fade In 직전 거리 보정 (stopDistance 이상 확보)
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < stopDistance)
        {
            Vector3 pushDir = (transform.position - player.position).normalized;
            Vector3 safePos = player.position + pushDir * (stopDistance + 0.2f);

            NavMeshHit hit2;
            if (NavMesh.SamplePosition(safePos, out hit2, 2f, NavMesh.AllAreas))
                agent.Warp(hit2.position);

            MaintainFixedHeight();
        }

        // Fade In: 동일한 랜덤 속도로 상승
        randomFadeSpeed = fadeSpeed * Random.Range(0.7f, 1.3f);
        yield return StartCoroutine(FadeTo(1f, randomFadeSpeed));

        agent.isStopped = false;
        isRetreating = false;
    }

    IEnumerator FadeTo(float targetAlpha, float speed)
    {
        float t = 0f;

        float startAlpha = renderers[0].material.color.a;
        float startIntensity = eyeballSpotLight.intensity;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;

            float a = Mathf.Lerp(startAlpha, targetAlpha, t);

            foreach (Renderer r in renderers)
            {
                if (r.material.HasProperty("_Color"))
                {
                    Color c = r.material.color;
                    c.a = a;
                    r.material.color = c;
                }
            }

            eyeballSpotLight.intensity = Mathf.Lerp(startIntensity, originalSpotIntensity * targetAlpha, t);

            yield return null;
        }
    }
}
