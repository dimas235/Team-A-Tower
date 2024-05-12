using System.Collections;
using UnityEngine;
using TMPro;

public class DefenderHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public GameObject popUpDamagePrefabPhysical;
    public GameObject popUpDamagePrefabMage;
    public Animator animator;  // Reference to the Animator component
    public bool isAlive = true;  // Status hidup atau mati

    public enum DamageType
    {
        Physical,
        Mage
    }

    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();  // Ensure the Animator component is assigned
    }

    public void TakeDamage(int damage, DamageType type)
    {
        if (!isAlive)  // Hanya memproses damage jika masih hidup
            return;

        health -= damage;
        
        if (health <= 0)
        {
            Die();
            return;
        }

        GameObject selectedPrefab = type == DamageType.Mage ? popUpDamagePrefabMage : popUpDamagePrefabPhysical;
        if (selectedPrefab != null)
        {
            GameObject popup = Instantiate(selectedPrefab, transform.position, Quaternion.identity);
            TMP_Text popupText = popup.GetComponentInChildren<TMP_Text>();
            if (popupText != null)
            {
                popupText.text = damage.ToString();
            }
        }
    }

    private void Die()
    {
        animator.SetTrigger("Death");  // Activate death animation
        isAlive = false;  // Set status to not alive
        gameObject.layer = LayerMask.NameToLayer("IgnoreProjectiles");  // Change layer to ignore projectiles
        DisableOtherComponents();  // Disable related components
        Destroy(gameObject, 5.0f); // Delay destruction to allow animation to play
    }

    private void DisableOtherComponents()
    {
        // Disable movement script
        var movementComponent = GetComponent<DefenderMovement>();
        if (movementComponent != null)
            movementComponent.enabled = false;
        
        // Disable any attack components
        var attackComponent = GetComponent<TankTroopsAttack>();
        if (attackComponent != null)
            attackComponent.enabled = false;
    }
}
