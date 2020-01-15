using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    [SerializeField] int Health = 5;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float jumpHeight = 8f;
    [SerializeField] float enemySeePlayerRange = 5f;

    bool canSeePlayer = false;
    int framestilljumpagain = 0;//to fix double jump bug
    bool isAttacking = false;
    bool inAttackCoRoutine = false;
    //cache
    Animator myAnimator;
    Rigidbody2D myRigidBody;
    PolygonCollider2D myFeetCollider;
    //Player thePlayer;
    GameObject[] Players;
    bool getplayersonce = true;
    bool Frozen = false;
    bool turnRed = true;
    int randomPlayer = 0;

    //run up to player stops and swings, repeat
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
       // thePlayer = FindObjectOfType<Player>();
        myAnimator = GetComponent<Animator>();
        myFeetCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Frozen)
        {
            if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Default")))
            {
                Frozen = false;
                turnRed = true;
            }
            //StartCoroutine(UnFreeze());
        }
        else
        {
            if (getplayersonce)
            {
                getplayers();
            }
            else
            {
                if (canSeePlayer)
                {
                    IsHeAttacking();
                    if (isAttacking)
                    {
                        if (!inAttackCoRoutine)
                        {
                            StartCoroutine(Attack());
                        }
                    }
                    else
                    {
                        Chase();
                    }
                }
                else
                {
                    Roam();
                }
                if (framestilljumpagain > 0)
                {
                    framestilljumpagain++;
                    if (framestilljumpagain == 5)
                    {
                        framestilljumpagain = 0;
                    }
                }
            }
        }
    }
    private void getplayers()
    {
        //int counter = 0;
        Players = GameObject.FindGameObjectsWithTag("Player");
        /*if(Players.Length < 2)
        {

        }
        else
        {
            getplayersonce = false;
        }*/
        if (Players.Length == 0)
        {

        }
        else
        {
            getplayersonce = false;
        }
    }
    IEnumerator Attack()
    {
        inAttackCoRoutine = true;
        myRigidBody.velocity = new Vector2(0f, 0f);
        myAnimator.SetBool("Walk", false);
        myAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        inAttackCoRoutine = false;
    }
    
    private void IsHeAttacking()
    {
        if (Mathf.Abs(transform.position.x - GetComponentInChildren<KnightVision>().KnightVisionPlayerX()) < 1f && Mathf.Abs(transform.position.y - GetComponentInChildren<KnightVision>().KnightVisionPlayerY()) < 3f)
        {
            isAttacking = true;
        }
    }

    private void Roam()
    {
        myAnimator.SetBool("Walk", false);
        myRigidBody.velocity = new Vector2(0f, 0f);
    }

    private void Chase()
    {
        myAnimator.SetBool("Walk", true);
        Flip();
        if (GetComponentInChildren<KnightVision>().IsPlayerInFrontKnightVision())//player is in front
        {
            myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y);
        }
        else if (!GetComponentInChildren<KnightVision>().IsPlayerInFrontKnightVision())//player behind
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, myRigidBody.velocity.y);
        }
    }

    private void Flip()
    {
        if(canSeePlayer)
        {
            if(GetComponentInChildren<KnightVision>().IsPlayerInFrontKnightVision())
            {
                transform.localScale = new Vector2(-1f, 1f);
                GetComponentInChildren<KnightVision>().transform.localScale = new Vector3(-1f, 1f);
            }
            else
            {
                transform.localScale = new Vector2(1f, 1f);
                GetComponentInChildren<KnightVision>().transform.localScale = new Vector3(1f, 1f);
            }
        }
    }

    private bool IsPlayerInFront()
    {
        float EnemyPlayerXDifference = transform.position.x - Players[0].transform.position.x;
        if (EnemyPlayerXDifference < 0)//player is in front
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SeesPlayer(bool x)
    {
        if(x == false)
        {
           /* if(Mathf.Abs(transform.position.x - Players[0].transform.position.x) < enemySeePlayerRange)
            {
                //canSeePlayer = true;
                return;
            }*/
        }
        canSeePlayer = x;
    }
    public void JumpKnight()
    {
        if(isAttacking)
        {
            return;
        }
        if (!canSeePlayer)
        {
            return;
        }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Default")))
        {
            return;
        }
        if (framestilljumpagain != 0)
        {
            return;
        }
        if (!GetComponentInChildren<KnightVision>().IsPlayerInFrontKnightVision() && myRigidBody.velocity.x > 0)//rat is in front and velocity positive
        {
            return;
        }
        if (GetComponentInChildren<KnightVision>().IsPlayerInFrontKnightVision() && myRigidBody.velocity.x < 0)//rat is in back and velocity negatives
        {
            return;
        }
        Vector2 jumpVector = new Vector2(0f, jumpHeight);
        myRigidBody.velocity += jumpVector;
        framestilljumpagain++;
    }
    public void EnemyDamaged()
    {
        Health = Health - 1;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void KnightFreeze()
    {
        float xvel = 5f;
        float yvel = 8f;
        if (GetComponentInChildren<KnightVision>().IsPlayerInFrontKnightVision())
        {
            xvel = -xvel;
        }
        myRigidBody.velocity = new Vector2(xvel, yvel);
        if (turnRed)
        {
            myAnimator.SetTrigger("Damaged");
            turnRed = false;
        }
        Frozen = true;
    }
    public float GetVisionRange()
    {
        return enemySeePlayerRange;
    }
}
