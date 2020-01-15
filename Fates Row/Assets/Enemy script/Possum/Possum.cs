using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possum : MonoBehaviour
{
    [SerializeField] int Health = 5;
    [SerializeField] int moveSpeed = 3;
    [SerializeField] float acceleratorSpeed = 0.1f;
    [SerializeField] float jumpHeight = 8f;
    [Tooltip("This means he accelerates at the player")]
    [SerializeField] bool isAccelerator = false;
    [Tooltip("Range at which he will start chasing player")]
    [SerializeField] float enemySeePlayerRange = 5f;
    int isdirright = 1;
    bool SeePlayer = false;

    Rigidbody2D myRigidbody;
    Animator myAnimator;
    //PlayerUnit thePlayer;
    PolygonCollider2D feetCollider;
    CircleCollider2D frontCollider;
    CapsuleCollider2D wallswitchCollider;
    GameObject[] Players;
    bool getplayersonce = true;
    bool Frozen = false;
    bool turnRed = true;
    int playerseenindex = 0;


    float framestilljumpagain = 0;//to fix double jump bug

    public void ChildChangeisdirrightPossum()
    {
        isdirright = isdirright * -1;
    }

    private void Awake()
    {
      
    }
    // Start is called before the first frame update
    void Start()
    {        
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        //thePlayer = FindObjectOfType<PlayerUnit>();
        feetCollider = GetComponent<PolygonCollider2D>();
        frontCollider = GetComponent<CircleCollider2D>();
        wallswitchCollider = GetComponent<CapsuleCollider2D>();
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
            if (getplayersonce)
            {
                getplayers();
            }
            else
            {
                Flip();
                DoesHeSeePlayer();
                if (SeePlayer == true)
                {
                    Chase();
                }
                else
                {
                    Move();
                }

                if (framestilljumpagain > 0)
                {
                    framestilljumpagain+= 1 * Time.deltaTime;
                    if (framestilljumpagain == 3)
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
        if(Players.Length < 2)
         {

         }
        /*if (Players.Length == 0)
        {
        }*/
        else
        {
            getplayersonce = false;
        }
      /*  foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Players[counter] = player;
            counter++;
            getplayersonce = false;
            Debug.Log("player");
        }*/
    }
    private void DoesHeSeePlayer()
    {
        //do for player[1] whichever one is thats the index 0 or 1, then just make that a variable
        float EnemyPlayerXDifference = transform.position.x - Players[0].transform.position.x;
        float EnemyPlayer2XDifference = transform.position.x - Players[1].transform.position.x;
        if (Mathf.Abs(EnemyPlayerXDifference) < enemySeePlayerRange)
        {
            SeePlayer = true;
            playerseenindex = 0;
        }
        else if(Mathf.Abs(EnemyPlayer2XDifference) < enemySeePlayerRange)
        {
            SeePlayer = true;
            playerseenindex = 1;
        }
    }

    public void JumpPossom()
    {
        if(!SeePlayer)
        {
            return;
        }
        if(!feetCollider.IsTouchingLayers(LayerMask.GetMask("Default")))
        {
            return;
        }
        if(framestilljumpagain != 0)
        {
            return;
        }
        if(!IsPlayerInFront() && myRigidbody.velocity.x > 0)//rat is in front and velocity positive
        {
            return;
        }
        if (IsPlayerInFront() && myRigidbody.velocity.x < 0)//rat is in back and velocity negatives
        {
            return;
        }
        Vector2 jumpVector = new Vector2(0f, jumpHeight);
        myRigidbody.velocity += jumpVector;
        framestilljumpagain += 1 * Time.deltaTime;
    }

    private void Flip()
    {
        if(SeePlayer)//always facing player
        {
            if (Mathf.Abs(transform.position.x - Players[playerseenindex].transform.position.x) < 2)
            {

            }
            else
            {
                if (IsPlayerInFront())
                {
                    if (transform.localScale.x > 0)
                    {
                        transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
                    }
                }
                else
                {
                    if (transform.localScale.x < 0)
                    {
                        transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
                    }
                }
            }
        }
        else
        {
            transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), transform.localScale.y);
        }
        
    }

    private bool IsPlayerInFront()
    {
        float EnemyPlayerXDifference = transform.position.x - Players[playerseenindex].transform.position.x;
        if (EnemyPlayerXDifference < 0)//player is in front
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Chase()
    {
        if(IsPlayerInFront())//player is in front
        {
            if(isAccelerator)
            {
                myRigidbody.velocity += new Vector2(acceleratorSpeed, 0f);
            }
            else
            {
                myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
            }
            
        }
        else if (!IsPlayerInFront())//player behind
        {
            if(isAccelerator)
            {
                myRigidbody.velocity += new Vector2(-acceleratorSpeed, 0f);
            }
            else
            {
                myRigidbody.velocity = new Vector2(-moveSpeed, myRigidbody.velocity.y);
            }
        }
    }

    private void Move()
    {
        if(isdirright == 1)
        {
            myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        }
        else
        {
            myRigidbody.velocity = new Vector2(-moveSpeed, myRigidbody.velocity.y);
        }
    }
    public float GetDirectionPossum()
    {

        return transform.localScale.x;
    }
    public void EnemyDamaged()
    {
        Health = Health - 1;
        if(Health <= 0)
        {
            FindObjectOfType<killcontainer>().addtocounter();
            Destroy(gameObject);
        }
    }
    public void PossumFreeze()
    {
        float xvel = 5f;
        float yvel = 8f;
        if (IsPlayerInFront())
        {
            xvel = -xvel;
        }
        myRigidbody.velocity = new Vector2(xvel, yvel);
        if(turnRed)
        {
            myAnimator.SetTrigger("Damaged");
            turnRed = false;
        }
        Frozen = true;
    }
    IEnumerator UnFreeze()
    {
        yield return new WaitForSeconds(.5f);
        Frozen = false;
    }
    
}
