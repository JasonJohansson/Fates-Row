using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpColliderKnight : MonoBehaviour
{
    BoxCollider2D boxcollider;

    private void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        SeeifEnemyNeedsToJump();
    }

    private void SeeifEnemyNeedsToJump()
    {
        if (boxcollider.IsTouchingLayers(LayerMask.GetMask("Default")))
        {
            transform.parent.GetComponent<Knight>().JumpKnight();
        }
    }
}
