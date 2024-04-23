using System.Collections;
using UnityEngine;

public class MageTroopsStateMachine : MonoBehaviour
{
    public enum TroopsState
    {
        Walking,
        Shooting
    }

    public TroopsState troopsState;
    public float detectionRange;
    public LayerMask enemyLayer;
    public MageTroopsThrowing mageTroopsThrowingScript;
    public DefenderMovement defenderMovement;
    private DefenderHealth defenderHealth;

    void Start()
    {
        troopsState = TroopsState.Walking;
        defenderHealth = GetComponent<DefenderHealth>();
        defenderHealth.OnStunEnded += HandleStunEnded; // Subscribe to the OnStunEnded event
        mageTroopsThrowingScript.enabled = false;
    }

    void Update()
    {
        if (defenderHealth.isStunned)
        {
            mageTroopsThrowingScript.enabled = false;
            defenderMovement.enabled = false;
            return;
        }

        RaycastHit hit;
        bool enemyDetected = Physics.Raycast(transform.position, Vector2.right, out hit, detectionRange, enemyLayer);

        if (enemyDetected && !defenderHealth.isStunned)
        {
            ChangeState(TroopsState.Shooting);
        }
        else if (!enemyDetected)
        {
            ChangeState(TroopsState.Walking);
        }
    }

    void ChangeState(TroopsState newState)
    {
        troopsState = newState;
        mageTroopsThrowingScript.enabled = (newState == TroopsState.Shooting);
        defenderMovement.enabled = (newState == TroopsState.Walking);
    }

    private void HandleStunEnded()
    {
        if (Physics.Raycast(transform.position, Vector2.right, detectionRange, enemyLayer))
        {
            ChangeState(TroopsState.Shooting);
        }
    }

    void OnDestroy()
    {
        defenderHealth.OnStunEnded -= HandleStunEnded; // Unsubscribe to prevent memory leaks
    }
}
