using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTroopsThrowing : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float coolDown;

    private float timer;
    private MageTroopsStateMachine mageTroopsStateMachineScript;
    private DefenderHealth defenderHealth;  // Referensi ke kesehatan

    void Start()
    {
        mageTroopsStateMachineScript = GetComponentInParent<MageTroopsStateMachine>();
        defenderHealth = GetComponentInParent<DefenderHealth>();  // Mendapatkan referensi kesehatan dari parent
        timer = coolDown;
    }

    void Update()
    {
        if (mageTroopsStateMachineScript.troopsState != MageTroopsStateMachine.TroopsState.Shooting || defenderHealth.isStunned)
        {
            return;  // Jangan lempar fireball jika dalam keadaan stun atau bukan Shooting
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            LaunchFireball();
            timer = coolDown;
        }
    }

    private void LaunchFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        Collider fireballCollider = fireball.GetComponent<Collider>();
        if (fireballCollider != null)
        {
            fireballCollider.enabled = false;
        }

        Collider parentCollider = GetComponentInParent<Collider>();
        if (parentCollider != null)
        {
            Physics.IgnoreCollision(fireballCollider, parentCollider);
        }

        StartCoroutine(EnableColliderWhenPassed(fireball, fireballCollider));
    }

    private IEnumerator EnableColliderWhenPassed(GameObject fireball, Collider fireballCollider)
    {
        yield return new WaitUntil(() => fireball.transform.position.x > transform.position.x);
        fireballCollider.enabled = true;
    }
}
