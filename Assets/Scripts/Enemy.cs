using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stopDistance = 1.5f;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float shootDistance = 6f;
    public float fireRate = 1f;
    public float visionRadius = 8f;
    public LayerMask playerLayer;
    public LayerMask wallLayer;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float nextFire;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Collider2D detectedPlayer = Physics2D.OverlapCircle(transform.position, visionRadius, playerLayer);
        if (!detectedPlayer) return;

        Vector2 dir = (Vector2)(detectedPlayer.transform.position - transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, visionRadius, wallLayer | playerLayer);

        if (!hit || !hit.collider.CompareTag("Player")) return;

        player = detectedPlayer.transform;

        float dist = dir.magnitude;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        sr.flipY = dir.x < 0;

        if (dist > stopDistance)
            rb.MovePosition(rb.position + dir.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (!player) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= shootDistance && Time.time >= nextFire)
        {
            Shoot();
            nextFire = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (!firePoint || !bulletPrefab) return;

        GameObject b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        b.GetComponent<Projectile>().isEnemyBullet = true;
    }
}
