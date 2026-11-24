using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{   
    [Header("攻击设置")]
    [SerializeField]
    private float attackOffset = 0.8f;
    [SerializeField]
    private Vector2 attackSize = new Vector2(1.5f, 1.5f);
    [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField]
    private int attackDamage = 10;

    [SerializeField]
    private PlayerControl playerControl;

    private int FaceDirection;

    private Rigidbody2D rb;



    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            TryAttack();
        }
    }


    private void TryAttack()
    {
        FaceDirection = playerControl.FaceDirection;
        Vector2 attackCenter = (Vector2)transform.position + new Vector2(attackOffset * FaceDirection, 0f);
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0f, enemyLayer);
        if(hitEnemies.Length > 0)
        {
            Debug.Log("命中敌人");
            foreach (Collider2D enemy in hitEnemies)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                enemyHealth.TakeDamage(attackDamage);
            }
        }


    } 
}
