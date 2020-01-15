using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 8f;

    Rigidbody2D myRigidBody;
    Animator myAnimator;
    BoxCollider2D feetCollider;
    CapsuleCollider2D bodyCollider;

    bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        feetCollider = GetComponent<BoxCollider2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Die();
        if(!alive)
        {
            return;
        }
        Run();
        Jump();
    }

    private void Die()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            StartCoroutine(GotHit());
        }
    }

    IEnumerator GotHit()
    {
        alive = false;
        Vector2 hitBackVector = new Vector2(-(Mathf.Sign(transform.localScale.x)) * 5f, 1f);
        myRigidBody.velocity = hitBackVector;
        yield return new WaitForSeconds(.2f);
        alive = true;
    }

    private void Jump()
    {
        if(!feetCollider.IsTouchingLayers(LayerMask.GetMask("ForegroundTile")))
        {
            return;
        }
        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVector = new Vector2(0f, jumpHeight);
            myRigidBody.velocity += jumpVector;
            myAnimator.SetTrigger("Jump");
        }
    }

    private void Run()
    {
        float playerXInput = Input.GetAxis("Horizontal");
        if(playerXInput == 0)
        {
            myAnimator.SetBool("Running", false);
            myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);
            return;
        }
        myAnimator.SetBool("Running", true);
        Flip(playerXInput);
        Vector2 newVector = new Vector2(playerXInput * moveSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = newVector;
       
    }

    private void Flip(float xDir)
    {
       if(xDir > 0)
        {
            transform.localScale = new Vector2(8, 8f);
        }
        else
        {
            transform.localScale = new Vector2(-8f, 8f);
        }
    }
}
