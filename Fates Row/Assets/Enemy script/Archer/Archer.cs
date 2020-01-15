using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    [SerializeField] int Health = 5;
    [SerializeField] int VisionRange = 30;
    [SerializeField] bool isMage = false;
    [SerializeField] FireBall thefireball;
    [SerializeField] float fireBallMoveSpeed = 10f;
    [SerializeField] float fireBallShootSpeed = 1f;
    [SerializeField] float FireBallChargeUpTime = 1f;
    [Header("IF curvycosine")]
    [SerializeField] float desiredRadius = 5f;

   // [SerializeField] bool straightBall = false;
   // [SerializeField] bool curvyBall = false;
   // [SerializeField] bool acceleratingBall = false;
    bool canSeePlayer = false;
    [Header("*To change arrow speed go to arrow prefab*")]
    [SerializeField] Arrow theArrow;
    [SerializeField] bool gasArrow;
    [Tooltip("Time between each shot in seconds")]
    [SerializeField] float shootSpeed = 1f;
    [Tooltip("Time for him to charge attack in seconds")]
    [SerializeField] float chargeUpTime = 1f;
    [SerializeField] float moveSpeed = 1f;
    bool shootingisrunning = false;
    int isdirright = 1;

    Rigidbody2D myRigidBody;
    PolygonCollider2D feetCollider;
    Animator myAnimator;
    bool Frozen = false;
    bool turnRed = true;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        feetCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Frozen)
        {
            if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Default")))
            {
                Frozen = false;
                turnRed = true;
            }
            //StartCoroutine(UnFreeze());
        }
        else
        {
            if (canSeePlayer)
            {
                myRigidBody.velocity = new Vector2(0f, 0f);
                GetComponent<Animator>().SetBool("Walk", false);
                Attack();
            }
            else
            {
                //GetComponent<Animator>().SetBool("Walk", true);
                //Roam();
            }
        }


    }

    private void Roam()
    {
        if (isdirright == 1)
        {
            GetComponentInChildren<WallSwitchColliderArcher>().transform.localScale = new Vector3(1 * transform.localScale.x, transform.localScale.y);
            GetComponentInChildren<WallSwitchColliderArcher>().transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            GetComponent<SpriteRenderer>().flipX = true;
            myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y);
        }
        else
        {
            GetComponentInChildren<WallSwitchColliderArcher>().transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
            GetComponentInChildren<WallSwitchColliderArcher>().transform.position = new Vector2(transform.position.x - 1, transform.position.y);
            GetComponent<SpriteRenderer>().flipX = false;
            myRigidBody.velocity = new Vector2(-moveSpeed, myRigidBody.velocity.y);
        }
    }
    public void ChildChangeisdirrightArcher()
    {
        isdirright = isdirright * -1;
    }
    private void Attack()
    {
        Flip();
        if (!shootingisrunning)
        {
            shootingisrunning = true;
            if (isMage)
            {
                StartCoroutine(ShootingMage());
            }
            else
            {
                StartCoroutine(Shooting());
            }
        }
    }

    private void Flip()
    {
        if (GameObject.FindGameObjectWithTag("Player").transform.position.x - transform.position.x > 0)//to your right
        {
            GetComponent<SpriteRenderer>().flipX = true;
            return;
        }
        //to your left
        GetComponent<SpriteRenderer>().flipX = false;
    }

    IEnumerator Shooting()
    {
        GetComponent<Animator>().SetBool("Shoot", true);
        yield return new WaitForSeconds(chargeUpTime);
        GetComponent<Animator>().SetBool("Shoot", false);
        Arrow arrow = Instantiate(theArrow, transform.position, Quaternion.identity) as Arrow;
        arrow.transform.parent = transform;
        if (gasArrow == true)
        {
            //change color
            arrow.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
            //change arrow gasboolean
            arrow.setGasOn();
        }
        yield return new WaitForSeconds(shootSpeed);
        shootingisrunning = false;

    }
    IEnumerator ShootingMage()
    {
        yield return new WaitForSeconds(FireBallChargeUpTime);
        FireBall fireball = Instantiate(thefireball, transform.position, Quaternion.identity) as FireBall;
        fireball.transform.parent = transform;
        fireball.setMoveSpeed(fireBallMoveSpeed);
        fireball.setDesiredRadius(desiredRadius);
       
        yield return new WaitForSeconds(fireBallShootSpeed);
        shootingisrunning = false;

    }
    public void SeesPlayer(bool x)
    {
        canSeePlayer = x;
    }
    public void EnemyDamaged()
    {
        Health = Health - 1;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void MageFreeze()
    {
        myAnimator.SetTrigger("Damaged");
    }
    public void ArcherFreeze()
    {
        float xvel = 5f;
        float yvel = 10f;
        if (GetComponentInChildren<ArcherVision>().IsPlayerInFrontArcher())
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
    public int GetVisionRange()
    {
        return VisionRange;
    }
}
