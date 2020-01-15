using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jester : MonoBehaviour
{
    [SerializeField] int Health = 5;
    [Header("Bottle Tuning *bottle speed is on bottle prefab*")]
    [Tooltip("offset from center of parabola, higher number higher throw")]
    [SerializeField, Range(-20, 20)] float xoffsetRangeMax;
    [Tooltip("offset from center of parabola, higher number higher throw")]
    [SerializeField, Range(-20, 20)] float xoffsetRangeMin;
    [Tooltip("smaller a wider throw is (a*x^2)(-.1 good base)")]
    [SerializeField, Range(-3,0)] float a;
    [Header("Jester tuning")]
    [SerializeField] float enemySeePlayerRange = 20f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float throwSpeed = 2f;
    [SerializeField] GameObject theBottle;

    //Player thePlayer;
    Animator myAnimator;
    Rigidbody2D myRigidBody;
    PolygonCollider2D feetCollider;

    float xoffset = 0f;
    bool seePlayer = false;
    bool isThrowing = false;
    int isdirright = 1;
    bool Frozen = false;
    bool turnRed = true;

    GameObject[] Players;
    bool getplayersonce = true;
    int playerseenindex;
    // Start is called before the first frame update
    void Start()
    {
        //thePlayer = FindObjectOfType<Player>();
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
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
            if (getplayersonce)
            {
                getplayers();
            }
            else
            {
                DoesHeSeePlayer();
                if (seePlayer)
                {
                    myRigidBody.velocity = new Vector2(0f, 0f);
                    myAnimator.SetBool("Throw", true);
                    myAnimator.SetBool("Walk", false);
                    Attack();
                }
                else
                {
                    myAnimator.SetBool("Throw", false);
                    myAnimator.SetBool("Walk", true);
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
       /* if (Players.Length == 0)
        {

        }*/
        else
        {
            getplayersonce = false;
        }
    }
    private void Roam()
    {
        if (isdirright == 1)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y);
        }
        else
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
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
        if(!isThrowing)
        {
            isThrowing = true;
            StartCoroutine(Throwing());
        }
        
    }
    private void Flip()
    {
        if(IsPlayerInFront())
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
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
    IEnumerator Throwing()
    {
        //throw bottle
        xoffset = Random.Range(xoffsetRangeMin, xoffsetRangeMax);//10f
        GameObject newBottle = Instantiate(theBottle, transform.position, Quaternion.identity) as GameObject;
        newBottle.transform.parent = transform;
        yield return new WaitForSeconds(throwSpeed);//2f
        isThrowing = false;
    }
    public float getXoffset()
    {
        return xoffset;
    }
    public float getA()
    {
        return a;
    }
    private void DoesHeSeePlayer()
    {
        float EnemyPlayerXDifference = transform.position.x - Players[0].transform.position.x;
        float EnemyPlayer2XDifference = transform.position.x - Players[1].transform.position.x;
        if (!seePlayer)
        {
            if (Mathf.Abs(EnemyPlayerXDifference) < enemySeePlayerRange)
            {
                seePlayer = true;
                playerseenindex = 0;
            }
            else if (Mathf.Abs(EnemyPlayer2XDifference) < enemySeePlayerRange)
            {
                seePlayer = true;
                playerseenindex = 0;
            }
        }
        else
        {
            float EnemyPlayerXDifferenceafter = transform.position.x - Players[playerseenindex].transform.position.x;
            if (Mathf.Abs(EnemyPlayerXDifferenceafter) < enemySeePlayerRange)
            {

            }
            else
            {
                seePlayer = false;
            }
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
    public void JesterFreeze()
    {
        float xvel = 3f;
        float yvel = 10f;
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
    public GameObject GetPlayer()
    {
        return Players[playerseenindex];
    }
}
