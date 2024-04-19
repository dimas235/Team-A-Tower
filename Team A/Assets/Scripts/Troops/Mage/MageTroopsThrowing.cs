using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTroopsThrowing : MonoBehaviour
{
    public GameObject fireballPrefab; // Prefab dari fireball yang akan dilempar
    public float coolDown;

    private float timer;
    private MageTroopsStateMachine mageTroopsStateMachineScript; // Skrip MageTroopsStateMachine yang terkait

    void Start()
    {
        mageTroopsStateMachineScript = GetComponentInParent<MageTroopsStateMachine>(); // Mendapatkan skrip MageTroopsStateMachine dari parent GameObject
        timer = coolDown;    
    }

    void Update()
    {
        if(mageTroopsStateMachineScript.troopsState != MageTroopsStateMachine.TroopsState.Shooting)
        {
            return; // Jangan lempar fireball jika bukan dalam keadaan Shooting
        }

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            Collider fireballCollider = fireball.GetComponent<Collider>();
            if(fireballCollider != null)
            {
                fireballCollider.enabled = false;
            }

            // Mengabaikan tabrakan dengan defender yang mengspawn fireball
            Collider parentCollider = GetComponentInParent<Collider>();
            if(parentCollider != null)
            {
                Physics.IgnoreCollision(fireballCollider, parentCollider);
            }

            // // Mengabaikan tabrakan dengan semua defender lainnya
            // Collider[] allDefenders = FindObjectsOfType<Collider>(); // Cari semua Collider yang mungkin adalah defender
            // foreach (var defender in allDefenders)
            // {
            //     if(defender.gameObject.layer == LayerMask.NameToLayer("Troops")) // Ganti "DefenderLayer" dengan nama layer defender Anda
            //     {
            //         Physics.IgnoreCollision(fireballCollider, defender);
            //     }
            // }

            StartCoroutine(EnableColliderWhenPassed(fireball, fireballCollider));
            timer = coolDown;
        }   
    }

    private IEnumerator EnableColliderWhenPassed(GameObject fireball, Collider fireballCollider)
    {
        yield return new WaitUntil(() => fireball.transform.position.x > transform.position.x);
        fireballCollider.enabled = true;
    }
}
