using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] int Health = 5;
    [SerializeField] float enemySeePlayerRange = 15f;
    [SerializeField] float roamSpeed = 2f;
    [SerializeField] float roamRadius = 20f;
    [Tooltip("The bird just follows you closely")]
    [SerializeField] bool birdFollows = false;
    [Header("Basic Strike tuning")]
    [Tooltip("The bird strikes at you")]
    [SerializeField] bool birdBasicStrike = false;
    [Tooltip("Time between each strike in seconds")]
    [SerializeField] float birdBasicStrikeSpeedMax = 2f;
    [SerializeField] float birdBasicStrikeSpeedMin = 2f;
    [Tooltip("speed he hovers around you Max")]
    [SerializeField] float MaxBasicBirdMoveSpeed = 2f;
    [Tooltip("speed he hovers around you Min")]
    [SerializeField] float MinBasicBirdMoveSpeed = 2f;
    [Tooltip("His strike velocity units/seconds")]
    [SerializeField] float strikeVelocity = 5f;
    [Header("Strike Acceleration (keep x and y same)")]
    [SerializeField] float AccelerationX = 1f;
    [SerializeField] float AccelerationY = 1f;
    

    //cache
    Rigidbody2D myRigidbody;
    Animator myAnimator;
   // Player thePlayer;

    bool SeePlayer = false;
    bool dirIsRight = true;
    bool canCharge = true;
    bool inRecoveryPhase = false;
    float originalXposition;
    bool Frozen = false;
    bool turnRed = true;

    GameObject[] Players;
    bool getplayersonce = true;
    int playerseenindex = 0;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
       // thePlayer = FindObjectOfType<Player>();
        originalXposition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Frozen)
        {
            
        }
        else
        {
            if (getplayersonce)
            {
                getplayers();
            }
            else
            {
                DoesHeSeePlayer();
                if (SeePlayer)
                {
                    Flip();
                    Chase();
                }
                else
                {
                    IsInRadius();
                    Roam();
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
    }
    private void DoesHeSeePlayer()
    {
        float EnemyPlayerXDifference = transform.position.x - Players[0].transform.position.x;
        float EnemyPlayer2XDifference = transform.position.x - Players[1].transform.position.x;
        if (Mathf.Abs(EnemyPlayerXDifference) < enemySeePlayerRange)
        {
            SeePlayer = true;
            playerseenindex = 0;
        }
        else if (Mathf.Abs(EnemyPlayer2XDifference) < enemySeePlayerRange)
        {
            SeePlayer = true;
            playerseenindex = 1;
        }
    }
    private bool PlayerInRange()
    {
        float EnemyPlayerXDifference = transform.position.x - Players[playerseenindex].transform.position.x;
        float EnemyPlayerYDifference = transform.position.y - Players[playerseenindex].transform.position.y;
        if (Mathf.Abs(EnemyPlayerXDifference) < enemySeePlayerRange)
        {
            return true;
        }
        return false;
    }
    public void TurnBird()
    {
        dirIsRight = !dirIsRight;
    }
    private void IsInRadius()
    {
        if(Mathf.Abs(originalXposition - transform.position.x) > roamRadius)
        {
            dirIsRight = !dirIsRight;
        }
    }
    private void Roam()
    {
        if(dirIsRight)
        {
            myRigidbody.velocity = new Vector2(roamSpeed, 0f);
            transform.localScale = new Vector2(-1f, 1f);
        }
        else
        {
            myRigidbody.velocity = new Vector2(-roamSpeed, 0f);
            transform.localScale = new Vector2(1f, 1f);
        }
       
    }

    private void Chase()
    {
        float birdToPlayerYDiff = transform.position.y - Players[playerseenindex].transform.position.y;
        float birdToPlayerXDiff = transform.position.x - Players[playerseenindex].transform.position.x;
        if(birdFollows)
        {
            myRigidbody.velocity = new Vector2(-birdToPlayerXDiff, -birdToPlayerYDiff);
        }
        if(!canCharge)
        {
            if (inRecoveryPhase)
            {
                HandleRecoveryPhase();
            }
        }
        if (canCharge)
        {
            if (!PlayerInRange())
            {
                GoToPlayer();
                return;
            }
            if (birdBasicStrike)
            {
                StartCoroutine(IsStriking());
            }
        }
    }

    private void HandleRecoveryPhase()
    {
        float EnemyPlayerXDifference = transform.position.x - Players[playerseenindex].transform.position.x;
        float EnemyPlayerYDifference = transform.position.y - Players[playerseenindex].transform.position.y;
        float velX = 0f;
        float velY = 0f;
        bool x = false;
        bool y = false;
        if (Mathf.Abs(EnemyPlayerXDifference) > 7f)
        {
            x = true;
            if(IsPlayerInFront())
            {
                velX = 5f;
            }
            else
            {
                velX = -5f;
            }
        }
        if(Mathf.Abs(EnemyPlayerYDifference) > 7f)
        {
            if (EnemyPlayerYDifference < 0)
            {

            }
            else
            {
                y = true;
                if (Mathf.Abs(EnemyPlayerYDifference) > 7.5f)
                {
                    velY = -3f;
                }
                else
                {
                    velY = 0f;
                }
            }
        }
        if(x == true && y == true)
        {
            myRigidbody.velocity = new Vector2(velX, velY);
        }
        else if(x == true)
        {
            myRigidbody.velocity = new Vector2(velX, myRigidbody.velocity.y);
        }
        else if(y == true)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, velY);
        }
    }
    private void GoToPlayer()
    {
        float randBirdX;
        float randBirdY;
        if (IsUnderPlayer())
        {
            randBirdY = Random.Range(MinBasicBirdMoveSpeed, MaxBasicBirdMoveSpeed);
        }
        else
        {
            randBirdY = Random.Range(-MinBasicBirdMoveSpeed, -MaxBasicBirdMoveSpeed);
        }
        if (IsPlayerInFront())
        {
            randBirdX = Random.Range(MinBasicBirdMoveSpeed, MaxBasicBirdMoveSpeed);
        }
        else
        {
            randBirdX = Random.Range(-MinBasicBirdMoveSpeed, -MaxBasicBirdMoveSpeed);
        }
        myRigidbody.velocity = new Vector2(0f, 0f);
        myRigidbody.velocity = new Vector2(randBirdX, randBirdY);//random
    }

    IEnumerator IsStriking()
    {
        canCharge = false;
        float playerX = Players[playerseenindex].transform.position.x;
        float playerY = Players[playerseenindex].transform.position.y;
        float birdX = transform.position.x;
        float birdY = transform.position.y;
        float xtime;
        float velX;
        float velY;
        float accelerationX = AccelerationX;
        float accelerationY = AccelerationY;
        if(!IsUnderPlayer())
        {
            accelerationY = -accelerationY;
        }
        if(!IsPlayerInFront())
        {
            accelerationX = -accelerationX;
        }
        //strike
        if (Mathf.Abs(playerX - birdX) > Mathf.Abs(playerY - birdY))
        {
            xtime = Mathf.Abs(playerX - birdX) / strikeVelocity;
            velX = (playerX - birdX) / xtime;
            velY = (playerY - birdY) / xtime;
        }
        else
        {
            xtime = Mathf.Abs(playerY - birdY) / strikeVelocity;
            velX = (playerX - birdX) / xtime;
            velY = (playerY - birdY) / xtime;
        }
        myRigidbody.velocity = new Vector2(velX, velY);

        while (xtime > -.1f)//can change this more negative more he will continue to strike
        {
            yield return new WaitForSeconds(.2f);
            xtime -= .2f;
            velX += accelerationX;
            velY += accelerationY;
            myRigidbody.velocity = new Vector2(velX, velY);
        }
        myRigidbody.velocity = new Vector2(0f,0f);
        //fly around
        inRecoveryPhase = true;
        int xdirection = Random.Range(-1, 1);
        if(xdirection > 0)
        {
            velX = Random.Range(1f, 2f);
        }
        else
        {
            velX = Random.Range(-2f, -1f);
        }
        velY = Random.Range(2f, 5f);
        myRigidbody.velocity = new Vector2(velX, velY);
        yield return new WaitForSeconds(Random.Range(birdBasicStrikeSpeedMin, birdBasicStrikeSpeedMax));
        inRecoveryPhase = false;
        myRigidbody.velocity = new Vector2(0f,0f);

        canCharge = true;
    }

    private float waitTimeFunct(float XSpeed)
    {
        return 1 / XSpeed;
    }
    private bool IsUnderPlayer()
    {
       if(transform.position.y > Players[playerseenindex].transform.position.y)
        {
            return false;
        }
        return true;
    }

    private float exponent(float bird, float x)
    {
        return bird/x;
    }
    private void Flip()
    {
        if(IsPlayerInFront())
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
        else
        {
            transform.localScale = new Vector2(1f, 1f);
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
    public void EnemyDamaged()
    {
        Health = Health - 1;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void BirdFreeze()
    {
        float xvel = 10f;
        float yvel = 4f;
        if (IsPlayerInFront())
        {
            xvel = -xvel;
        }
        myRigidbody.velocity = new Vector2(xvel, yvel);
        if (turnRed)
        {
            myAnimator.SetTrigger("Damaged");
            turnRed = false;
        }
        Frozen = true;
        StartCoroutine(UnFreeze());
    }
    IEnumerator UnFreeze()
    {
        yield return new WaitForSeconds(.5f);
        Frozen = false;
        turnRed = true;
    }
}

















/*is striking code
        float birdSlowdownX = 1f;
        float birdSlowdownY = 1f;
        canCharge = false;
        float birdToPlayerYDiff = transform.position.y - thePlayer.transform.position.y;
        float birdToPlayerXDiff = transform.position.x - thePlayer.transform.position.x;
        float playerX = thePlayer.transform.position.x;
        float playerY = thePlayer.transform.position.y;
        float birdX = transform.position.x;
        float birdY = transform.position.y;
        float xsign = Mathf.Sign(birdToPlayerXDiff);
        float ysign = Mathf.Sign(birdToPlayerYDiff);
        float XSpeed = Random.Range(1f, birdBasicStrikeSpeed);
        float YSpeed = XSpeed;

        myRigidbody.velocity = new Vector2(-birdToPlayerXDiff * XSpeed, -birdToPlayerYDiff * YSpeed);
        while (Mathf.Abs(birdX - transform.position.x) < Mathf.Abs(birdX - playerX) || Mathf.Abs(birdY - transform.position.y) < Mathf.Abs(birdY - playerY))
        {
            yield return new WaitForSeconds(.2f);
        }     

        float waitTime;
        if(XSpeed > 1.4f)
        {
            waitTime = Random.Range(2f, 4f);
        }
        else
        {
            waitTime = Random.Range(3f, 5f);
        }
        float x = 1.0f;
        //  waitTime = waitTimeFunct(birdToPlayerXDiff);
        while (Mathf.Abs(birdSlowdownX) > .01f)//Mathf.Sign(birdToPlayerXDiff) == Mathf.Sign(xsign) && Mathf.Sign(birdToPlayerYDiff) == Mathf.Sign(ysign)
        {
            myRigidbody.velocity = new Vector2(-birdToPlayerXDiff * XSpeed, -birdToPlayerYDiff * YSpeed);
            yield return new WaitForSeconds(.1f);
            if (BasicNoRecover)
            {
            }
            else
            {
                x += .4f;
            }

            birdSlowdownX = exponent(birdToPlayerXDiff, x);
            birdSlowdownY = exponent(birdToPlayerYDiff, x);
            if (xsign > 0)
            {
                birdToPlayerXDiff -= birdSlowdownX;
            }
            else
            {
                birdToPlayerXDiff -= birdSlowdownX;
            }
            if (ysign > 0)
            {
                birdToPlayerYDiff -= birdSlowdownY;
            }
            else
            {
                birdToPlayerYDiff -= birdSlowdownY;
            }
           
        }

        float randBirdX;
        float randBirdY;
        if(IsUnderPlayer())
        {
            randBirdY = Random.Range(MinBasicBirdMoveSpeed, MaxBasicBirdMoveSpeed);//0f,2f
        }
        else
        {
            randBirdY = Random.Range(-MinBasicBirdMoveSpeed, -MaxBasicBirdMoveSpeed);
        }
        if(IsPlayerInFront())
        {
            randBirdX = Random.Range(MinBasicBirdMoveSpeed, MaxBasicBirdMoveSpeed);
        }
        else
        {
            randBirdX = Random.Range(-MinBasicBirdMoveSpeed, -MaxBasicBirdMoveSpeed);
        }
        myRigidbody.velocity = new Vector2(randBirdX, randBirdY);//random
        if (BasicNoRecover)
        {

        }
        else
        {
            if (Random.Range(-1f, 6f) < 0)
            {

            }
            else
            {
                yield return new WaitForSeconds(Random.Range(.5f, 4f));//.5,4f
            }
        }
        canCharge = true;*/
