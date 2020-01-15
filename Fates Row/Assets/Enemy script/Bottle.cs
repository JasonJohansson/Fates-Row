using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidBody;
    //Player thePlayer;
    //a(x+xoffset - w)^2 + h
    float a;
    float w;
    float h;
    float xoffset;
    float bottleX;
    float bottleY;
    float playerX;
    float playerY;
    float rotateAngle = 0f;
    float DeathCounter = 0;
    bool playerInFront = false;
    GameObject Player;

    //pass parameters through jester maybe

    // Start is called before the first frame update
    void Start()
    {
        Player = transform.parent.GetComponent<Jester>().GetPlayer();
        myRigidBody = GetComponent<Rigidbody2D>();
        //thePlayer = FindObjectOfType<Player>();
        IsPlayerInFront();
        //playerX = GameObject.FindGameObjectWithTag("Player").transform.position.x;
        // playerY = GameObject.FindGameObjectWithTag("Player").transform.position.y;
        playerX = Player.transform.position.x;
        playerY = Player.transform.position.y;
        bottleX = transform.position.x;
        bottleY = transform.position.y;
        h = bottleY;
        w = bottleX;

        //a = -1 * Random.Range(aRangeMin, aRangeMax);//-.1f
        a = transform.parent.GetComponent<Jester>().getA();
        if(playerInFront)
        {
            xoffset = -1 * transform.parent.GetComponent<Jester>().getXoffset();
        }
        else
        {
            xoffset = transform.parent.GetComponent<Jester>().getXoffset();
        }
        h = bottleY - a * Mathf.Pow(bottleX + xoffset - w, 2);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
        DeathCounter+= 1 * Time.deltaTime;
        if (DeathCounter > 200)
        {
            Destroy(gameObject);
        }
    }

    private void Rotate()
    {
        rotateAngle += 10 * Time.deltaTime;
        transform.eulerAngles = new Vector3(0f, 0f, rotateAngle);
    }

    private void Move()
    {
        bottleY = a * Mathf.Pow((bottleX + xoffset) - w, 2) + h;
        if(playerInFront)
        {
            bottleX += moveSpeed * Time.deltaTime;
        }
        else
        {
            bottleX -= moveSpeed * Time.deltaTime;
        }

        transform.position = new Vector2(bottleX, bottleY);
    }
     private bool IsPlayerInFront()
    {
        float EnemyPlayerXDifference = transform.position.x - Player.transform.position.x;
        if (EnemyPlayerXDifference < 0)//player is in front
        {
            playerInFront = true;
            return true;
        }
        else
        {
            playerInFront = false;
            return false;
        }
    }
}
