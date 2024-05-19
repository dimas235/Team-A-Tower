using System.Collections;
using UnityEngine;
using TMPro;

public class DefenderHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public GameObject popUpDamagePrefabPhysical;
    public GameObject popUpDamagePrefabMage;
    public Animator animator;
    public bool isAlive = true;

    public Material blinkMaterial;
    public float flashDuration = 0.1f;
    private Renderer[] renderers;
    private Material[] originalMaterials;

    public enum DamageType
    {
        Physical,
        Mage
    }

    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();

        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }
    }

    public void TakeDamage(int damage, DamageType type)
    {
        if (!isAlive)
            return;

        health -= damage;
        StartCoroutine(FlashEffect());

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

    private IEnumerator FlashEffect()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = blinkMaterial;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }

    private void Die()
    {
        animator.SetTrigger("Death");
        isAlive = false;
        gameObject.layer = LayerMask.NameToLayer("IgnoreProjectiles");
        DisableOtherComponents();
        Destroy(gameObject, 5.0f);
    }

    private void DisableOtherComponents()
    {
        var movementComponent = GetComponent<DefenderMovement>();
        if (movementComponent != null)
            movementComponent.enabled = false;

        var attackComponent = GetComponent<TankTroopsAttack>();
        if (attackComponent != null)
            attackComponent.enabled = false;
    }
}
