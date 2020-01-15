using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{

    //can roam a certain area back and forth and he can also jump when needed
    [SerializeField] int Health = 5;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float jumpHeight = 8f;
    [SerializeField] float roamRange = 10f;
    [SerializeField] float enemySeePlayerRange = 5f;
    [SerializeField] bool enemyswitchonwall = false;

    int isdirright = 1;
    float framestilljumpagain = 0;//to fix double jump bug
    float initialposition;
    bool Frozen = false;
    bool turnRed = true;
    bool goingright = true;

    Animator myAnimator;
    Rigidbody2D myRigidBody;
    PolygonCollider2D myFeetCollider;
    //Player thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        initialposition = transform.position.x;
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
            CheckIfEnemyIsWithinRange();
            Roam();
            if (framestilljumpagain > 0)
            {
                framestilljumpagain+= 1 * Time.deltaTime;
                if (framestilljumpagain == 5)
                {
                    framestilljumpagain = 0;
                }
            }
        }
    }

    public bool GetEnemyHugEdgeState()
    {
        return enemyswitchonwall;
    }
    public void ChildChangeisdirrightSkeleton()
    {
        isdirright = isdirright * -1;
    }
    private void CheckIfEnemyIsWithinRange()
    {
        if(transform.position.x > initialposition + roamRange || transform.position.x < initialposition - roamRange)
        {
            if(transform.position.x < initialposition - roamRange && isdirright == 1 || transform.position.x > initialposition + roamRange && isdirright == -1)
            {

            }
            else
            {
                ChildChangeisdirrightSkeleton();
            }
        }
    }
    private void Roam()
    {
        if (isdirright == 1)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y);
        }
        else
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            myRigidBody.velocity = new Vector2(-moveSpeed, myRigidBody.velocity.y);
        }
    }
    public void JumpSkeleton()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Default")))
        {
            return;
        }
        if (framestilljumpagain != 0)
        {
            return;
        }
        if (!IsPlayerInFront() && myRigidBody.velocity.x > 0)//rat is in front and velocity positive
        {
            return;
        }
        if (IsPlayerInFront() && myRigidBody.velocity.x < 0)//rat is in back and velocity negatives
        {
            return;
        }
        Vector2 jumpVector = new Vector2(0f, jumpHeight);
        myRigidBody.velocity += jumpVector;
        framestilljumpagain+= 1*Time.deltaTime;
    }
    private bool IsPlayerInFront()
    {
        float EnemyPlayerXDifference = transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x;
        if (EnemyPlayerXDifference < 0)//player is in front
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void EnemyDamaged()
    {
        Health = Health - 1;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void SkeletonFreeze()
    {
        float xvel = 5f;
        float yvel = 8f;
        if (IsPlayerInFront())
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

}
