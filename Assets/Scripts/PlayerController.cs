using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float attackCooldown = 0.3f;

    private Vector2 moveInput;
    private Vector2 mousePos;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Camera cam;
    private bool canShoot = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
    }

    void Update()
    {
        Vector2 dir = mousePos - rb.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        sr.flipY = mousePos.x < rb.position.x;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (!context.performed || cam == null) return;
        mousePos = cam.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.performed || !canShoot) return;

        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(attackCooldown);
        canShoot = true;
    }
}
