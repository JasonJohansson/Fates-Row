using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{

    [SerializeField] float enemySeePlayerRange = 20f;
    [SerializeField] float moveSpeed = 1f;

    Player thePlayer;
    Animator myAnimator;
    Rigidbody2D myRigidBody;

    float xoffset = 0f;
    bool seePlayer = false;
    bool isThrowing = false;
    int isdirright = 1;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        DoesHeSeePlayer();
        if (seePlayer)
        {
            myRigidBody.velocity = new Vector2(0f, 0f);
            myAnimator.SetBool("Throw", true);
            myAnimator.SetBool("Walk", false);
        }
        else
        {
            myAnimator.SetBool("Throw", false);
            myAnimator.SetBool("Walk", true);
            Roam();
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
    private void DoesHeSeePlayer()
    {
        float EnemyPlayerXDifference = transform.position.x - thePlayer.transform.position.x;
        if (Mathf.Abs(EnemyPlayerXDifference) < enemySeePlayerRange)
        {
            seePlayer = true;
        }
    }
    public void ChildChangeisdirrightArcher()
    {
        isdirright = isdirright * -1;
    }
}
