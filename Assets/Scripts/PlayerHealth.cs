using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public float deathDelay = 0.5f;

    private SpriteRenderer sr;
    private Color originalColor;
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

        if (audioSource != null && hitSfx != null)
            audioSource.PlayOneShot(hitSfx);

        if (cam != null)
        {
            cam.Shake(0.12f, 0.2f);
            cam.HitPause(0.05f);
        }

        StartCoroutine(HitFlash());

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

    private void Die()
    {
        Invoke(nameof(ReloadScene), deathDelay);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
