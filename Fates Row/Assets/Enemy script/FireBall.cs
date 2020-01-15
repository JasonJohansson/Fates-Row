using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] bool curvycosine = false;
    [SerializeField] bool curvydefault = false;
    [SerializeField] bool straight = false;
    [SerializeField] bool givenvelocity = false;
    [SerializeField] bool givenvelocitynogravity = false;
    [SerializeField] bool immobilecosine = false;
    [SerializeField] bool fromBoss = false;
    Rigidbody2D myRigidBody;

    //types of ball paths
    bool curvy = false;
    bool accelerating = false;

    //general stuff
    float moveSpeed = 10f;
    float DeathCounter = 0;
    float velX;
    float velY;
    float playerX;
    float playerY;
    float BallX;
    float BallY;

    //curvy cosine
    float desiredRadius = 1f;
    float actualMagnitude;
    float delaytime = 1f;
    bool invokeOnce = true;
    GameObject Player;
    //circle stuff
    float radius;
    bool thetalower = true;
    float theta = 0;
    float originaltheta = 0;
    //no gravity ball
    float theoffset = 1f;
    float initialY;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        if(fromBoss)
        {
            playerX = 0;
            playerY = 0;
        }
        else
        {
            Player = transform.parent.GetComponentInChildren<ArcherVision>().GetPlayer();
            playerX = Player.transform.position.x;
            playerY = Player.transform.position.y;
        }
       // Player = transform.parent.GetComponent<ArcherVision>().GetPlayer();
        //playerX = GameObject.FindGameObjectWithTag("Player").transform.position.x;
        //playerY = GameObject.FindGameObjectWithTag("Player").transform.position.y;
        BallX = transform.position.x;
        BallY = transform.position.y;

        if (straight)
        {
            if (Mathf.Abs(playerX - BallX) > Mathf.Abs(playerY - BallY))
            {
                //calculates time needed to get to distance. movespeed is 10 so 10 units a second if distance is 20 it would take 2 seconds so t is 2
                //equation x = x0 + v0t + 0 (no acceleration on x)
                //equation y = y0 + v0t  + 0 (no acceleration on y either)

                float xtime = Mathf.Abs(playerX - BallX) / moveSpeed;
                velX = (playerX - BallX) / xtime;
                velY = (playerY - BallY) / xtime;
            }
            else
            {
                float ytime = Mathf.Abs(playerY - BallY) / moveSpeed;
                velY = (playerY - BallY) / ytime;
                velX = (playerX - BallX) / ytime;
            }
            myRigidBody.velocity = new Vector2(velX, velY);
        }
        if(curvycosine)
        {
            if (Mathf.Abs(playerX - BallX) > Mathf.Abs(playerY - BallY))
            {
                float xtime = Mathf.Abs(playerX - BallX) / moveSpeed;
                velX = (playerX - BallX) / xtime;
                velY = (playerY - BallY) / xtime;
            }
            else
            {
                float ytime = Mathf.Abs(playerY - BallY) / moveSpeed;
                velY = (playerY - BallY) / ytime;
                velX = (playerX - BallX) / ytime;
            }
            radius = Mathf.Sqrt(Mathf.Pow(velX, 2) + Mathf.Pow(velY, 2));

            theta = Mathf.Acos(velX / (Mathf.Sqrt(Mathf.Pow(velX, 2) + Mathf.Pow(velY, 2))));
            theta = (theta * 180) / Mathf.PI;
            if (velY < 0)
            {
                theta = (180 - theta) + 180;
            }
            originaltheta = theta;
            myRigidBody.velocity = new Vector2(velX, velY);
            actualMagnitude = Mathf.Sqrt(Mathf.Pow(velX, 2) + Mathf.Pow(velY, 2));
            delaytime = desiredRadius / actualMagnitude;
        }
        if (curvydefault)
        {
            if (Mathf.Abs(playerX - BallX) > Mathf.Abs(playerY - BallY))
            {
                //calculates time needed to get to distance. movespeed is 10 so 10 units a second if distance is 20 it would take 2 seconds so t is 2
                //equation x = x0 + v0t + 0 (no acceleration on x)
                //equation y = y0 + v0t  + 0 (no acceleration on y either)

                float xtime = Mathf.Abs(playerX - BallX) / moveSpeed;
                velX = (playerX - BallX) / xtime;
                velY = (playerY - BallY) / xtime;
            }
            else
            {
                float ytime = Mathf.Abs(playerY - BallY) / moveSpeed;
                velY = (playerY - BallY) / ytime;
                velX = (playerX - BallX) / ytime;
            }
            //here radius is based off initial speed so bigger radius bigger speed / radius = Mathf.Sqrt(Mathf.Pow(velX, 2) + Mathf.Pow(velY, 2));
            //radius = Mathf.Sqrt(Mathf.Pow(velX, 2) + Mathf.Pow(velY, 2));
            radius = 5f;
            if (velX < 0)
            {
                //radius = radius * -1;
            }
            theta = Mathf.Acos(velX / (Mathf.Sqrt(Mathf.Pow(velX, 2) + Mathf.Pow(velY, 2))));
            theta = (theta * 180) / Mathf.PI;
            if (velY < 0)
            {
                theta = (180 - theta) + 180;
            }
            originaltheta = theta;
            myRigidBody.velocity = new Vector2(velX, velY);
        }
        if(givenvelocity)
        {
            //velocity should be given through public function by whatever spawned this
            initialY = transform.position.y;
            myRigidBody.velocity = new Vector2(velX, velY);
        }
        if(immobilecosine)
        {
            if (Mathf.Abs(playerX - BallX) > Mathf.Abs(playerY - BallY))
            {
                float xtime = Mathf.Abs(playerX - BallX) / moveSpeed;
                velX = (playerX - BallX) / xtime;
                velY = (playerY - BallY) / xtime;
            }
            else
            {
                float ytime = Mathf.Abs(playerY - BallY) / moveSpeed;
                velY = (playerY - BallY) / ytime;
                velX = (playerX - BallX) / ytime;
            }
            radius = 5f;
            theta = 0;
            myRigidBody.velocity = new Vector2(velX, velY);
        }
        if (accelerating)
        {
            //dont do anything right away
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (straight)
        {
            //nothing to do continuously
        }
        else if(curvycosine)
        {
            if(invokeOnce)
            {
                //delay time is our desired magnitude (radius) divided by actual magnitude (radius) d/a, if bigger actual than smaller time interval because its going faster vice versa
                //if our desired is 2 and we get 4 then our speed is doubled so we have to cut delaytime in half
                //we base this around frametime so if its our desired it goes once per frame if its double it goes twice per frame if its half it goes once per two frames
                InvokeRepeating("CurvyCosine", .01f, Time.deltaTime * delaytime);
                invokeOnce = false;
            }
        }
        else if(curvydefault)
        {
            Curve();
        }
        else if(givenvelocity)
        {
            if(givenvelocitynogravity)//specific to the ball for the lava boss
            {
                if (Mathf.Sign(theoffset) == 1)
                {
                    if (transform.position.y < initialY + theoffset)
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y + .2f);
                    }
                }
                else
                {
                    if (transform.position.y > initialY + theoffset)
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y - .2f);
                    }
                }
            }
           
        }
        else if(immobilecosine)
        {
            radius = 8f;
            velY = -radius*Mathf.Sin((Mathf.PI * theta) / 180);
            myRigidBody.velocity = new Vector2(velX, velY);
            theta += 5;
        }
        if (accelerating)
        {

        }

        DeathCounter += 1 * Time.deltaTime;
        if (DeathCounter > 3)
        {
            Destroy(gameObject);
        }
    }
    
    private void CurvyCosine()
    {
        velX = radius * Mathf.Cos((Mathf.PI * theta) / 180);
        velY = radius * Mathf.Sin((Mathf.PI * theta) / 180);
        myRigidBody.velocity = new Vector2(velX, velY);

        if (thetalower)
        {
            theta -= 1 * Time.deltaTime;
            if (theta < originaltheta - 45)
            {
                thetalower = false;
            }
        }
        else
        {
            theta += 1 * Time.deltaTime;
            if (theta > originaltheta + 60)
            {
                thetalower = true;
            }
        }
    }
    private void Curve()
    {
        velX = radius * Mathf.Cos((Mathf.PI * theta) / 180);
        velY = radius * Mathf.Sin((Mathf.PI * theta) / 180);
        myRigidBody.velocity = new Vector2(velX, velY);
     
        if (thetalower)
        {
            theta -= 1 * Time.deltaTime;
            if (theta < originaltheta - 45)
            {
                thetalower = false;
                theta = originaltheta;
            }
        }
        else
        {
            theta+= 1 * Time.deltaTime;
            if (theta > originaltheta + 60)
            {
                thetalower = true;
                theta = originaltheta;
            }
        }
    }

    public void setVelocity(float x, float y)
    {
        velX = x;
        velY = y;
    }
    public void setDesiredRadius(float x)
    {
        desiredRadius = x;
    }
    public void setMoveSpeed(float ms)
    {
        moveSpeed = ms;
    }
    public void setStraight()
    {
        straight = true;
    }
    public void setCurvy()
    {
        curvy = true;
    }
    public void setAccelerating()
    {
        accelerating = true;
    }
    public void setthetalower(int x)
    {
        if(x == 0)
        {
            thetalower = false;
        }
        else
        {
            thetalower = true;
        }
    }
    public void setoffest(float x)
    {
        theoffset = x;
    }
}
