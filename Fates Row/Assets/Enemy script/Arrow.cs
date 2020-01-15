using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Tooltip("this is velocity unit/second")]
    [SerializeField] float moveSpeed = 10f;//10
    [SerializeField] GameObject gasParticle;
    bool isGasArrow = false;

    float velX;
    float velX2;
    float velY;
    float playerX;
    float playerY;
    float arrowX;
    float arrowY;
    float t;
    GameObject Player;

    float DeathCounter = 0;

    Rigidbody2D myRigidBody;

    bool stick = false;
    float stickx;
    float sticky;

    float rotateAngle = 0f;

    public void setGasOn()
    {
        isGasArrow = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        Player = transform.parent.GetComponentInChildren<ArcherVision>().GetPlayer();
       // Player = GetComponent<ArcherVision>().GetPlayer();
        //Player = transform.parent.GetComponent<ArcherVision>().GetPlayer();
        //playerX = GameObject.FindGameObjectWithTag("Player").transform.position.x;
       // playerY = GameObject.FindGameObjectWithTag("Player").transform.position.y;
        playerX = Player.transform.position.x;
        playerY = Player.transform.position.y;
        arrowX = transform.position.x;
        arrowY = transform.position.y;

        //no matter where u r it will get to you in x seconds, and that will determine x and y velocity
        if (Mathf.Abs(playerX - arrowX) > Mathf.Abs(playerY - arrowY))
        {
            float xtime = Mathf.Abs(playerX - arrowX) / moveSpeed;
            velX = (playerX - arrowX) / xtime;
            velY = (playerY - arrowY + (4.9f * Mathf.Pow(xtime, 2))) / xtime;
        }
        else
        {
            float ytime = Mathf.Abs(playerY - arrowY) / moveSpeed;
            velY = (playerY - arrowY + (4.9f * Mathf.Pow(ytime, 2))) / ytime;
            velX = (playerX - arrowX) / ytime;
        }
        myRigidBody.velocity = new Vector2(velX, velY);
    }
    // Update is called once per frame
    void Update()
    {
        Rotatez();
        Stick();
        DeathCounter+= 1* Time.deltaTime;
        if (DeathCounter > 100)
        {
            Destroy(gameObject);
        }
    }
    public void HitWall()
    {
        if (isGasArrow)
        {
            GameObject gas = Instantiate(gasParticle, transform.position, Quaternion.identity) as GameObject;
            Destroy(gas, 2f);
        }
        stickx = transform.position.x;
        sticky = transform.position.y;
        stick = true;
        DeathCounter = 250;
        //Destroy(GetComponentInChildren<BoxCollider2D>());
        Destroy(GetComponent<BoxCollider2D>());
    }
    private bool isPlayerToRight(float px, float ax)
    {
        if(px > ax)
        {
            return true;
        }
        return false;
    }
    private bool isPlayerAbove(float py, float ay)
    {
        if(py > ay)
        {
            return true;
        }
        return false;
    }
    private void Rotatez()
    {
        if(stick)
        {
            return;
        }
        if (isPlayerToRight(playerX, arrowX))
        {
            if(Mathf.Abs(playerX - arrowX) < 5)
            {
                rotateAngle = 90 * Mathf.Sin(myRigidBody.velocity.y / (moveSpeed)) + 180;
            }
            else
            {
                rotateAngle = 90 * Mathf.Sin(myRigidBody.velocity.y / (10 + moveSpeed)) + 180;//10 + moveSpeed seems to work fine (magic number)
            }
        }
        else
        {
            if (Mathf.Abs(playerX - arrowX) < 5)
            {
                rotateAngle = -90 * Mathf.Sin(myRigidBody.velocity.y / (moveSpeed));
            }
            else
            {
                rotateAngle = -90 * Mathf.Sin(myRigidBody.velocity.y / (10 + moveSpeed));
            }   
        }
        transform.eulerAngles = new Vector3(0f, 0f, rotateAngle);
    }

    private void Stick()
    {
       if(!stick)
        {
            return;
        }
        transform.position = new Vector2(stickx, sticky);
    }
}





















/*set Xspeed code
if (isPlayerToRight(playerX, arrowX))
        {
            velX = moveSpeed;
        }
        else
        {
            velX = -moveSpeed;
        }
        if(isPlayerAbove(playerY,arrowY))
        {
            velY = 40f;
            velX2 = moveSpeed;
        }
        else
        {
            velY = -40f;
            velX2 = moveSpeed;
        }
        if (Mathf.Abs(playerY - arrowY) > 2 && Mathf.Abs(playerX - arrowX) < 4 || Mathf.Abs(playerY - arrowY) > 10)//Mathf.Abs(playerX - arrowX) < 5
        {
            t = (playerY - arrowY) / velX2;
            velY = (playerY - (arrowY * t) + (4.9f * Mathf.Pow(t, 2))) / t;
            Debug.Log(velY);
            // t = Quadratic(4.9f, velY, playerY - arrowY);
            velX = (playerX - arrowX) / t;
        }
        else
        {
            t = (playerX - arrowX) / velX;
            velY = (playerY - arrowY - (-4.905f * Mathf.Pow(t, 2))) / t;
        }
 
      private float Quadratic(float a, float b, float c)
    {
        float x1 = (-b + Mathf.Sqrt(Mathf.Pow(b, 2) - (4 * a * c))) / (2 * a);
        float x2 = (-b - Mathf.Sqrt(Mathf.Pow(b, 2) - (4 * a * c))) / (2 * a);
        Debug.Log(x1);
        if(Mathf.Abs(x1) < Mathf.Abs(x2))
        {
            return x1;
        }
        return x2;
    }
*/

/*Parabola Code
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    float Y1;
    float Y2;
    bool Ytimer = true;

    float arrowY;
    float arrowX;
    float playerX;
    float playerY;

    //y = a(x-w)^2 + h
    float a = -.1f;
    float w;
    float h;
    float distanceAwayFromArrow;

    int firstTime = 0;
    int DeathCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        arrowY = transform.position.y;
        arrowX = transform.position.x;
        playerY = FindObjectOfType<Player>().transform.position.y - 1;
        playerX = FindObjectOfType<Player>().transform.position.x + 1;
        distanceAwayFromArrow = .20f*(arrowX - playerX);
        w = arrowX - distanceAwayFromArrow;
        a = (playerY - arrowY) / (Mathf.Pow(playerX - w, 2) - Mathf.Pow(arrowX - w, 2));
        h = arrowY - (a * Mathf.Pow(arrowX - w, 2));
        Debug.Log("height " + h);
        Debug.Log("a " + a);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(arrowY);
        Move();
        DeathCounter++;
        if(DeathCounter > 300)
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        arrowY = (a * Mathf.Pow(arrowX - w, 2) + h);
        transform.position = new Vector2(arrowX, arrowY);
        arrowX = arrowX - moveSpeed * Time.deltaTime;
    }
}
  
*/
