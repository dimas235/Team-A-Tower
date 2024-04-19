using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public TroopsState troopsState;
    public float detectionRange;
    public LayerMask enemyLayer;
    // public LayerMask defenderLayer; // Layer untuk defender lain
    // public float nonCollisionRadius = 1f; // Jarak untuk mengabaikan tabrakan antar defender
    public DefenderMovement defenderMovement;
    public Throwing throwingScript;

    // Start is called before the first frame update
    void Start()
    {
        troopsState = TroopsState.Walking;
        if (throwingScript != null)
        {
            throwingScript.enabled = false;
        }
    }

    // Update is called once per frame
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

        // IgnoreCollisionsWithDefenders();
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

    // void IgnoreCollisionsWithDefenders()
    // {
    //     // Mendeteksi defender lain dalam radius tertentu
    //     Collider[] defenders = Physics.OverlapSphere(transform.position, nonCollisionRadius, defenderLayer);
    //     foreach (var otherDefender in defenders)
    //     {
    //         if (otherDefender.gameObject != gameObject) // Pastikan tidak memilih collider sendiri
    //         {
    //             Physics.IgnoreCollision(GetComponent<Collider>(), otherDefender, true);
    //         }
    //     }
    // }

    public enum TroopsState
    {
        Walking,
        Shooting
    };
}
