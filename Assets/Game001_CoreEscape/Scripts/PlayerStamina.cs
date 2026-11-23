using UnityEngine;

/// <summary>
/// 管理玩家的通用体力资源，不关心体力被 Sprint、Dash 还是其他能力使用。
/// </summary>
[DisallowMultipleComponent]
public sealed class PlayerStamina : MonoBehaviour
{
    [Header("体力配置")]
    [SerializeField, Min(0f)] private float maxStamina = 100f;
    [SerializeField, Min(0f)] private float regenerationPerSecond = 20f;
    [SerializeField, Min(0f)] private float regenerationDelay = 0.75f;
    [SerializeField, Range(0f, 1f)] private float exhaustedRecoveryRatio = 0.25f;

    [Header("运行时状态（调试观察）")]
    [SerializeField] private float currentStamina;
    [SerializeField] private bool isExhausted;

    public float CurrentStamina => currentStamina;
    public float NormalizedStamina => maxStamina <= 0f
        ? 0f
        : currentStamina / maxStamina;
    public bool IsExhausted => isExhausted;

    private float regenerationDelayRemaining;

    private void Awake()
    {
        currentStamina = maxStamina;
        isExhausted = false;
    }

    private void Update()
    {
        if (regenerationDelayRemaining > 0f)
        {
            regenerationDelayRemaining = Mathf.Max(
                0f,
                regenerationDelayRemaining - Time.deltaTime
            );
            return;
        }

        currentStamina = Mathf.MoveTowards(
            currentStamina,
            maxStamina,
            regenerationPerSecond * Time.deltaTime
        );

        float recoveryThreshold = maxStamina * exhaustedRecoveryRatio;
        if (isExhausted && currentStamina >= recoveryThreshold)
        {
            isExhausted = false;
        }
    }

    public bool TrySpend(float amount)
    {
        if (amount <= 0f)
        {
            return true;
        }

        if (isExhausted)
        {
            return false;
        }

        if (currentStamina < amount)
        {
            currentStamina = 0f;
            isExhausted = true;
            regenerationDelayRemaining = regenerationDelay;
            return false;
        }

        currentStamina -= amount;
        regenerationDelayRemaining = regenerationDelay;

        if (currentStamina <= 0f)
        {
            isExhausted = true;
        }

        return true;
    }
}
