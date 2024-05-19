using UnityEngine;

public class DefenderMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody defenderRb;
    private bool isMovementEnabled = true;

    private DefenderHealth defenderHealth;

    void Start()
    {
        defenderHealth = GetComponent<DefenderHealth>();
    }

    void FixedUpdate()
    {
        if (isMovementEnabled && defenderHealth.isAlive)
        {
            defenderRb.velocity = Vector2.right * speed;
        }
        else
        {
            defenderRb.velocity = Vector2.zero;
        }
    }

    public void SetMovement(bool status)
    {
        isMovementEnabled = status;
        if (!isMovementEnabled)
        {
            defenderRb.velocity = Vector2.zero;
        }
    }
}
