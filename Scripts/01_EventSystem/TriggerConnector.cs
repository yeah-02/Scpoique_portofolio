using UnityEngine;
public class GazeAttackConnectorDebugger : MonoBehaviour 
{ public GazeTrigger gazeTrigger; 
    public EyeballAttackSequence_FinalV2 attackSeq; 
    void Start() 
    { if (gazeTrigger == null) 
            Debug.LogError("GazeTrigger 할당 안됨"); 
        if (attackSeq == null) Debug.LogError("AttackSeq 할당 안됨"); 
        gazeTrigger.OnGazeComplete += OnGazeCompleteHandler; } 
    void OnDestroy() 
    { if (gazeTrigger != null) 
            gazeTrigger.OnGazeComplete -= OnGazeCompleteHandler; } 
    void OnGazeCompleteHandler()
    { Debug.Log("[GazeAttackConnectorDebugger] GazeComplete 이벤트 수신됨."); 
        attackSeq.StartAttackSequence(); } 
}
