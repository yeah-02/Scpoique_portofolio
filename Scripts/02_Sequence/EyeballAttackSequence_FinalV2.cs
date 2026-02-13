using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EyeballAttackSequence_FinalV2 : MonoBehaviour
{
    [Header("References (assign in Inspector)")]
    public Transform eyeballRoot;
    public Transform player;
    public Camera playerCamera;
    public EyeballBehavior eyeballBehavior;
    public ScreenVignette screenVignette;
    public Transform finalViewTarget; // HeadOfHorseman

    [Header("Timings")]
    public float cameraRotateTime = 3f;   // eyeball을 바라보는 회전 시간
    public float eyeballGazeTime = 3f;    // eyeball 응시 고정 시간
    public float scaleDuration = 3f;      // eyeball scale up 시간

    [Header("Attack Settings")]
    public float scaleMultiplier = 3f;
    public float behindDistance = 5f;
    public bool disablePlayerScriptDuring = true;

    private bool isAttacking = false;
    private MonoBehaviour playerScript = null;
    private CharacterController playerCC = null;

    public void StartAttackSequence()
    {
        if (!isAttacking)
            StartCoroutine(FullSequence());
    }

    IEnumerator FullSequence()
    {
        isAttacking = true;

        // 1. eyeball을 player 뒤에 고정 배치
        Vector3 behindPos = player.position - player.forward * behindDistance;
        if (eyeballBehavior != null)
            behindPos.y = eyeballBehavior.fixedHeight;

        eyeballRoot.position = behindPos;

        // 2. player 이동 완전 고정
        var playerComp = player.GetComponent("Player_CC_Full") as MonoBehaviour;
        if (playerComp && disablePlayerScriptDuring)
        {
            playerComp.enabled = false;
            playerScript = playerComp;
        }

        playerCC = player.GetComponent<CharacterController>();
        if (playerCC) playerCC.enabled = false;

        // 3. eyeball 강제 가시화
        ForceEyeballVisible();

        // 4. 카메라 → eyeball 회전
        Quaternion startRot = playerCamera.transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(
            (eyeballRoot.position - playerCamera.transform.position).normalized
        );

        float t = 0f;
        while (t < cameraRotateTime)
        {
            t += Time.deltaTime;
            playerCamera.transform.rotation =
                Quaternion.Slerp(startRot, targetRot, Mathf.SmoothStep(0, 1, t / cameraRotateTime));
            yield return null;
        }
        playerCamera.transform.rotation = targetRot;

        // 5. eyeball 응시 고정 (3초) — vignette ❌
        yield return new WaitForSeconds(eyeballGazeTime);

        // 6. eyeball scale up (위치 고정)
        Vector3 startScale = eyeballRoot.localScale;
        Vector3 endScale = startScale * scaleMultiplier;

        float s = 0f;
        while (s < scaleDuration)
        {
            s += Time.deltaTime;
            eyeballRoot.localScale = Vector3.Lerp(startScale, endScale, s / scaleDuration);
            yield return null;
        }

        // 7. 즉시 trigger object 응시 (카메라 1회 고정)
        if (finalViewTarget != null)
        {
            Vector3 dir = (finalViewTarget.position - playerCamera.transform.position).normalized;
            playerCamera.transform.rotation = Quaternion.LookRotation(dir);
        }

        // 8. 이제서야 vignette 시작 (trigger object 응시 시점)
        if (screenVignette != null)
        {
            screenVignette.StartBlackout(5f);
        }

        yield break;
    }

    void ForceEyeballVisible()
    {
        Renderer[] rs = eyeballRoot.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in rs)
        {
            foreach (Material m in r.materials)
            {
                if (m.HasProperty("_Color"))
                {
                    Color c = m.color;
                    c.a = 1f;
                    m.color = c;
                }
            }
        }

        if (eyeballBehavior && eyeballBehavior.eyeballSpotLight)
        {
            eyeballBehavior.eyeballSpotLight.enabled = true;
        }
    }
}
