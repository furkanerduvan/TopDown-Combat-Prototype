using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public TMP_Text healthText;

    void Update()
    {
        if (playerHealth == null || healthText == null) return;

        healthText.text = playerHealth.currentHealth.ToString();
    }
}
