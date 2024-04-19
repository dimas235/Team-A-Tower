using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTroopsStateMachine : MonoBehaviour
{

    public TroopsState troopsState;
    public float detectionRange;
    public LayerMask enemyLayer;

    public MageTroopsThrowing mageTroopsThrowingScript;
    public DefenderMovement defenderMovement;

    void Start()
    {
        troopsState = TroopsState.Walking;
        if (mageTroopsThrowingScript != null)
        {
            mageTroopsThrowingScript.enabled = false;
        }
    }

    void Update()
    {
        RaycastHit hit;
        bool enemyDetected = Physics.Raycast(transform.position, Vector2.right, out hit, detectionRange, enemyLayer);

        if (troopsState == TroopsState.Walking && enemyDetected)
        {
            ChangeState(TroopsState.Shooting);
        }
        else if (!enemyDetected && troopsState == TroopsState.Shooting)
        {
            ChangeState(TroopsState.Walking);
        }
    }

    void ChangeState(TroopsState newState)
    {
        troopsState = newState;
        mageTroopsThrowingScript.enabled = (newState == TroopsState.Shooting);
        if (defenderMovement != null)
        {
            defenderMovement.enabled = (newState == TroopsState.Walking);
        }
    }

    public enum TroopsState
    {
        Walking,
        Shooting
    }
}
