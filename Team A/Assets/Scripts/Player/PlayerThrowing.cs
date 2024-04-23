using System.Collections;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{
    public GameObject magePrefab;
    public float attackCooldown;
    private PlayerMovement playerMovement;
    private float lastAttackTime;

    void Start()
    {
        lastAttackTime = attackCooldown;
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        lastAttackTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && lastAttackTime <= 0)
        {
            ShootInDirection();
            lastAttackTime = attackCooldown;
        }
    }

    void ShootInDirection()
    {
        Vector3 direction = playerMovement.facingRight ? Vector3.right : Vector3.left;
        GameObject mageInstance = Instantiate(magePrefab, transform.position, Quaternion.identity);
        mageInstance.GetComponent<PlayerMage>().Initialize(direction);
    }
}
