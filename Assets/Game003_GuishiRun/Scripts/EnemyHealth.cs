using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    private int currentHealth;
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        Debug.Log("敌人初始化，血量：" + currentHealth);
    }

    // Update is called once per frame
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("敌人剩余血量" + currentHealth);
        if(currentHealth <= 0)
        {
            Debug.Log("敌人死亡");
            Die();

        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
