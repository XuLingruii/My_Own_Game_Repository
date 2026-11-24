using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header("耐力条配置")]
    [SerializeField]
    private float maxStamina = 50f;
    [SerializeField]
    private float recoverTimeSkip = 1f;
    [SerializeField]
    private float recoverVelocity = 5f;

    public float MaxStamina => maxStamina;

    public float currentStamina{get; private set;}
    private float recoverAt = 0f;



    // Start is called before the first frame update
    void Awake()
    {
        currentStamina = maxStamina;
    }

    public bool TryStaminaSpeed(float cost)
    {
        if(cost > currentStamina)
        {
            return false;
        }

        currentStamina -= cost;
        recoverAt = Time.fixedTime + recoverTimeSkip;
        return true;
    }

    public void Recover()
    {
        if(Time.fixedTime < recoverAt)
        {
            return ;
        }

        currentStamina = Mathf.Min(MaxStamina, currentStamina+recoverVelocity*Time.fixedDeltaTime);
    }


}
