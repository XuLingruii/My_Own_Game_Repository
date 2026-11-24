using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("玩家状态设置")]
    [SerializeField]
    private int maxHealth = 100;
    public int MaxHealth => maxHealth;
    [SerializeField]
    private LevelRestart levelRestart;

    private int currentHealth;
    public int CurrentHealth => currentHealth;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("开始攻击,剩余血量："+ currentHealth);
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("玩家已死亡");
            GameSession.Instance.RecordDeath();
            levelRestart.RestartLevel();
        }
        
        
    }
}
