using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float checkRadius = 0.1f;
    // Start is called before the first frame update
    public bool IsGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, checkRadius, groundLayer);
        return collider != null;
    }
}
