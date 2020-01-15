using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //transform.parent.GetComponent<FireBall>().HitWall();
        Destroy(gameObject);
    }
}
