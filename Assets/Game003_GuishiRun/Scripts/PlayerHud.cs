using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHud : MonoBehaviour
{
    [Header("血条配置")]
    [SerializeField]
    private PlayerHealth playerHealth;
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private TMP_Text healthText;

    [Header("耐力配置")]
    [SerializeField]
    private PlayerStamina playerStamina;
    [SerializeField]
    private Slider staminaBar;

    [Header("死亡计数")]
    [SerializeField]
    private TMP_Text deathCountText;
    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = playerHealth.MaxHealth;
        staminaBar.maxValue = playerStamina.MaxStamina;
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = playerHealth.CurrentHealth;
        healthText.text = $"HP {playerHealth.CurrentHealth} / {playerHealth.MaxHealth}";
        deathCountText.text = $"Death Count : {GameSession.Instance.DeathCount}";

        staminaBar.value = playerStamina.currentStamina;
        
    }
}
