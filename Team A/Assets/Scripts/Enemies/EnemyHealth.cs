using System.Collections;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public GameObject popUpDamagePrefabPhysical;
    public GameObject popUpDamagePrefabMage;
    public Animator animator;  // Reference to the Animator component
    public bool isStunned = false;
    public bool isAlive = true;  // Status hidup atau mati
    public float stunDuration = 0;
    public event System.Action OnStunEnded;

    public delegate void OnStunChange(bool isStunned);
    public event OnStunChange StunStatusChanged;

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

    public void ApplyStun(float duration)
    {
        if (!isAlive)  // Hanya memproses stun jika masih hidup
            return;

        if (duration > 0 && !isStunned)
        {
            isStunned = true;
            stunDuration = duration;
            StartCoroutine(StunCountdown(duration));
        }
        else if (duration == 0)
        {
            isStunned = false;
        }
    }

    private IEnumerator StunCountdown(float duration)
    {
        yield return new WaitForSeconds(duration);
        isStunned = false;
        OnStunEnded?.Invoke();
        StunStatusChanged?.Invoke(isStunned);
    }

    private void Die()
    {
        animator.SetTrigger("Death");  // Aktivasi animasi kematian
        isAlive = false;  // Set status menjadi tidak hidup
        gameObject.layer = LayerMask.NameToLayer("IgnoreProjectiles");  // Ubah layer untuk menghiraukan proyektil
        DisableOtherComponents();  // Nonaktifkan komponen terkait
        Destroy(gameObject, 5.0f); // Delay penghancuran untuk memungkinkan animasi bermain
    }

    private void DisableOtherComponents()
    {
        // Nonaktifkan komponen gerakan
        var movementComponent = GetComponent<EnemyMovement>();
        if (movementComponent != null)
            movementComponent.SetMovement(false);
        
        // Nonaktifkan komponen serangan
        var attackComponent = GetComponent<TankEnemiesAttack>();
        if (attackComponent != null)
            attackComponent.enabled = false;
    }
}
