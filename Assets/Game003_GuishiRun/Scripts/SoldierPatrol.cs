using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierPatrol : MonoBehaviour
{
    [Header("巡逻设置")]
    [SerializeField]
    private float patrolSpeed = 3f;
    [SerializeField]
    private Transform leftEdge;

    [SerializeField]
    private Transform rightEdge;


    private Rigidbody2D rb;
    private int patrolDirection = 1;
    private TargetDetect targetDetect;

    private Transform target;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        targetDetect = GetComponentInChildren<TargetDetect>();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        Vector2 enemyPos = rb.transform.position;
        Vector2 velocity = rb.velocity;
        target = targetDetect.Target;
        int moveDirection;
        if(target!= null)
        {
            float deltaX = target.position.x - enemyPos.x;
            if (Mathf.Abs(deltaX) <= 0.6f)
            {
                moveDirection = 0;
            }
            else
            {
                moveDirection = deltaX > 0f ? 1 : -1;
            }

            // 追击到巡逻边界时停下，仍然允许向区间内移动。
            if ((moveDirection > 0 && enemyPos.x >= rightEdge.position.x) ||
                (moveDirection < 0 && enemyPos.x <= leftEdge.position.x))
            {
                moveDirection = 0;
            }
        }
        else
        {
            if(enemyPos.x < leftEdge.position.x)
            {
                patrolDirection = 1;
            }
            else if(enemyPos.x > rightEdge.position.x)
            {
                patrolDirection = -1;
            }

            moveDirection = patrolDirection;
        }
        velocity.x = moveDirection * patrolSpeed;
        rb.velocity = velocity;
    }
}
