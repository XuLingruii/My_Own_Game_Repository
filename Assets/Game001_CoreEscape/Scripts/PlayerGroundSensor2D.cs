using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地面检测脚本
/// </summary>

public class PlayerGroundSensor2D : MonoBehaviour
{
    [Header("地面检测配置")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.12f;
    [SerializeField] private Transform groundCheckPoint;

    public bool CheckGrounded()
    {
        Collider2D ground = Physics2D.OverlapCircle(
            groundCheckPoint.position, 
            groundCheckRadius, 
            groundLayer
        );
        return ground != null;
    }

    private void OnDrawGizmosSelected()
    {
        if(groundCheckPoint == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            groundCheckPoint.position, 
            groundCheckRadius
        );

    }

}
