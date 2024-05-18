using UnityEngine;
using System.Collections;

public class SkillProjectile : MonoBehaviour
{
    public Rigidbody meteorRb;
    public float speed;
    public float range;
    public int damage;
    public float damageRadius; // Radius untuk AoE damage
    public LayerMask enemyLayer;
    public GameObject meteorParticlesPrefab;
    public GameObject explosionEffectPrefab; // Referensi ke prefab efek ledakan
    public Camera miniCamera; // Referensi ke mini camera
    private CameraManager cameraManager; // Referensi ke CameraManager

    private float timer;
    private Vector3 targetPosition;
    private GameObject meteorParticlesInstance;

    void Start()
    {
        timer = range;

        // Buat partikel meteor
        meteorParticlesInstance = Instantiate(meteorParticlesPrefab, transform.position, Quaternion.identity);
        meteorParticlesInstance.transform.parent = transform; // Pasang ke proyektil

        // Cari dan aktifkan panel mini camera
        cameraManager = FindObjectOfType<CameraManager>();
        if (cameraManager != null)
        {
            cameraManager.ActivateMiniCameraPanel();
        }

        // Set mini camera sebagai parent dari proyektil
        if (miniCamera != null)
        {
            transform.parent = miniCamera.transform;
        }
    }

    public void Initialize(Vector3 target)
    {
        targetPosition = target;
        Vector3 direction = (targetPosition - transform.position).normalized;
        meteorRb.velocity = direction * speed;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void FixedUpdate()
    {
        if (timer <= 0)
        {
            DestroyMeteor();
        }
        timer -= Time.fixedDeltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            // Berikan damage ke semua musuh dalam radius damage
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius, enemyLayer);
            foreach (Collider hitCollider in hitColliders)
            {
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage, EnemyHealth.DamageType.Mage);
                }
            }

            // Instansiasi efek ledakan di titik tabrakan
            GameObject explosionEffectInstance = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

            // Sesuaikan skala efek ledakan berdasarkan radius damage
            explosionEffectInstance.transform.localScale = new Vector3(damageRadius * 2, damageRadius * 2, damageRadius * 2);

            // Set efek ledakan sebagai anak dari mini camera
            if (miniCamera != null)
            {
                explosionEffectInstance.transform.parent = miniCamera.transform;
            }

            // Hancurkan prefab meteor terlebih dahulu
            Destroy(meteorParticlesInstance);

            // Mulai coroutine untuk menghancurkan efek ledakan setelah jeda
            StartCoroutine(DestroyExplosionAfterDelay(explosionEffectInstance, 3f));
        }
    }

    private void DestroyMeteor()
    {
        // Hancurkan game object proyektil
        Destroy(gameObject);
    }

    private IEnumerator DestroyExplosionAfterDelay(GameObject explosionEffectInstance, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(explosionEffectInstance);

        // Nonaktifkan panel mini camera setelah efek ledakan dihancurkan
        if (cameraManager != null)
        {
            cameraManager.DeactivateMiniCameraPanel();
        }

        // Hancurkan game object proyektil
        Destroy(gameObject);
    }
}
