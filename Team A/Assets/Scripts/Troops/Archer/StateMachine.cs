using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public TroopsState troopsState;
    public float detectionRange;
    public LayerMask enemyLayer;
    public DefenderMovement defenderMovement;
    public Throwing throwingScript;
    private DefenderHealth defenderHealth; // Referensi ke DefenderHealth

    void Start()
    {
        troopsState = TroopsState.Walking;
        defenderHealth = GetComponent<DefenderHealth>();  // Mengambil komponen DefenderHealth
        if (throwingScript != null)
        {
            throwingScript.enabled = false;
        }
    }

    void Update()
    {
        // Cek kondisi stun dan atur aktivitas berdasarkan kondisi tersebut
        if (defenderHealth.isStunned)
        {
            throwingScript.enabled = false;
            if (defenderMovement != null)
            {
                defenderMovement.enabled = false;
            }
        }
        else
        {
            if (throwingScript != null && troopsState == TroopsState.Shooting)
            {
                throwingScript.enabled = true;
            }
            if (defenderMovement != null && troopsState == TroopsState.Walking)
            {
                defenderMovement.enabled = true;
            }

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
    }


    void ChangeState(TroopsState newState)
    {
        troopsState = newState;
        throwingScript.enabled = (newState == TroopsState.Shooting);
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
