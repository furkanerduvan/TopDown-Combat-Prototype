using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public Color hitColor = Color.red;
    public float flashDuration = 0.1f;

    private SpriteRenderer sr;
    private Color originalColor;
    private Coroutine flashCoroutine;

    private CameraControl cam;

    public AudioClip hitSfx;
    private AudioSource audioSource;

    void Awake()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        cam = FindObjectOfType<CameraControl>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (cam != null)
            cam.Shake(0.12f, 0.2f);
            cam.HitPause(0.05f);

        if (audioSource != null && hitSfx != null)
            audioSource.PlayOneShot(hitSfx);

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashDamage());

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator FlashDamage()
    {
        sr.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;
    }

    private void Die()
    {
        sr.color = hitColor;
        Destroy(gameObject, 0.1f);
    }
}
