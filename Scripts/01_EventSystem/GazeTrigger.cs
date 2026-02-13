using UnityEngine;

public class GazeTrigger : MonoBehaviour
{
    public Transform target;        // HeadOfHorseman
    public float gazeTime = 3f;

    private float timer = 0f;
    private bool triggered = false;
    private Camera cam;

    public delegate void GazeEvent();
    public event GazeEvent OnGazeComplete;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (triggered) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.transform == target)
            {
                timer += Time.deltaTime;

                if (timer >= gazeTime)
                {
                    triggered = true;
                    OnGazeComplete?.Invoke();
                }
            }
            else
            {
                timer = 0f;
            }
        }
        else
        {
            timer = 0f;
        }
    }
}
