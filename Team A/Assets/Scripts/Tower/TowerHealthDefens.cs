using UnityEngine;
using UnityEngine.Events;

public class TowerHealthDefens : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public UnityEvent onTowerDestroyed; // Event yang dipanggil ketika tower dihancurkan

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (health <= 0)
        {
            // Nonaktifkan tower dan panggil event onTowerDestroyed
            gameObject.SetActive(false);
            onTowerDestroyed.Invoke();
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }
}
