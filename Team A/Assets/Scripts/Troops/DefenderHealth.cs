using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefenderHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public GameObject popUpDamagePrefabPhysical;
    public GameObject popUpDamagePrefabMage;
    public bool isStunned = false;
    public event System.Action OnStunEnded;
    public float stunDuration = 0;

     // Event delegate for stun status changes
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
    }

    public void TakeDamage(int damage, DamageType type)
    {
        health -= damage;
        
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

        if (health <= 0)
        {
            Die();
        }
    }

    public void ApplyStun(float duration)
    {
        if (duration > 0 && !isStunned)
        {
            isStunned = true;
            stunDuration = duration;
            StartCoroutine(StunCountdown(duration));
        }
        else if (duration == 0)
        {
            isStunned = false;  // Immediately unstun if duration is 0
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
        GameManager.instance.defenders.Remove(gameObject); 
        Destroy(gameObject);
    }
}
