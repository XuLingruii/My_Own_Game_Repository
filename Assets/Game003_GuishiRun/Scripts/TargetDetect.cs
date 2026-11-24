using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetect : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("攻击设置")]
    [SerializeField]
    private int damage = 20;
    [SerializeField]
    private float attackTimeSkip = 1f;
    private Transform target;
    public Transform Target => target;
    private bool isAllowAtack = false;
    private float nextAttackAt = 0f;




    void FixedUpdate()
    {
        if(target == null) return ;
        PlayerHealth targetHealth = target.GetComponent<PlayerHealth>();
        if(isAllowAtack && Time.fixedTime>nextAttackAt){
            targetHealth.TakeDamage(damage);
            nextAttackAt = Time.fixedTime + attackTimeSkip;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("检测到玩家");
            target = other.transform;
            isAllowAtack = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            isAllowAtack = false;
            Debug.Log("玩家离开检测范围");
        }
    }
}
