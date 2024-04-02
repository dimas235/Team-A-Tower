using System.Collections;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    public GameObject stonePrefab; // Prefab dari batu yang akan dilempar
    public float coolDown;

    private float timer;
    private StateMachine stateMachineScript; // Skrip StateMachine yang terkait

    void Start()
    {
        stateMachineScript = GetComponentInParent<StateMachine>(); // Mendapatkan skrip StateMachine dari parent GameObject
        timer = coolDown;    
    }

    void Update()
    {
        if(stateMachineScript.troopsState != StateMachine.TroopsState.Shooting)
        {
            return; // Jangan lempar batu jika bukan dalam keadaan Shooting
        }

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            GameObject stone = Instantiate(stonePrefab, transform.position, Quaternion.identity);
            Collider stoneCollider = stone.GetComponent<Collider>();
            if(stoneCollider != null)
            {
                stoneCollider.enabled = false;
            }

            // Mengabaikan tabrakan dengan defender yang mengspawn batu
            Collider parentCollider = GetComponentInParent<Collider>();
            if(parentCollider != null)
            {
                Physics.IgnoreCollision(stoneCollider, parentCollider);
            }

            // Mengabaikan tabrakan dengan semua defender lainnya
            Collider[] allDefenders = FindObjectsOfType<Collider>(); // Cari semua Collider yang mungkin adalah defender
            foreach (var defender in allDefenders)
            {
                if(defender.gameObject.layer == LayerMask.NameToLayer("Troops")) // Ganti "DefenderLayer" dengan nama layer defender Anda
                {
                    Physics.IgnoreCollision(stoneCollider, defender);
                }
            }

            StartCoroutine(EnableColliderWhenPassed(stone, stoneCollider));
            timer = coolDown;
        }   
    }

    private IEnumerator EnableColliderWhenPassed(GameObject stone, Collider stoneCollider)
    {
        yield return new WaitUntil(() => stone.transform.position.x > transform.position.x);
        stoneCollider.enabled = true;
    }
}
