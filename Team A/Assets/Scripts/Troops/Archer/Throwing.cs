using System.Collections;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    public GameObject stonePrefab; // Prefab dari batu yang akan dilempar
    public float coolDown;

    private float timer;
    private StateMachine stateMachineScript; // Skrip StateMachine yang terkait
    private DefenderHealth defenderHealth; // Komponen kesehatan defender

    void Start()
    {
        stateMachineScript = GetComponentInParent<StateMachine>(); // Mendapatkan skrip StateMachine dari parent GameObject
        defenderHealth = GetComponentInParent<DefenderHealth>(); // Mendapatkan komponen DefenderHealth
        timer = coolDown;    
    }

    void Update()
    {
        if (stateMachineScript.troopsState != StateMachine.TroopsState.Shooting || defenderHealth.isStunned)
        {
            return; // Jangan lempar batu jika bukan dalam keadaan Shooting atau jika unit ter-stun
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            LaunchStone();
            timer = coolDown;
        }
    }

    private void LaunchStone()
    {
        GameObject stone = Instantiate(stonePrefab, transform.position, Quaternion.identity);
        Collider stoneCollider = stone.GetComponent<Collider>();
        if (stoneCollider != null)
        {
            stoneCollider.enabled = false;
        }

        // Mengabaikan tabrakan dengan defender yang mengspawn batu
        Collider parentCollider = GetComponentInParent<Collider>();
        if (parentCollider != null)
        {
            Physics.IgnoreCollision(stoneCollider, parentCollider);
        }

        StartCoroutine(EnableColliderWhenPassed(stone, stoneCollider));
    }

    private IEnumerator EnableColliderWhenPassed(GameObject stone, Collider stoneCollider)
    {
        yield return new WaitUntil(() => stone.transform.position.x > transform.position.x);
        stoneCollider.enabled = true;
    }
}
