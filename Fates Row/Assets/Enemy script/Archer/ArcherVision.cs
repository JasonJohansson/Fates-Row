using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherVision : MonoBehaviour
{
    //cache
    EdgeCollider2D myCollider;
    //GameObject thePlayer;
    Archer theArcher;
    PolygonCollider2D myPolyCollider;
    GameObject[] Players;
    bool getplayersonce = true;
    bool SeePlayer = false;
    bool trigger;
    int playerseenindex = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        trigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        trigger = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        theArcher = transform.parent.GetComponent<Archer>();
        //thePlayer = GameObject.FindGameObjectWithTag("Player");
        myCollider = GetComponent<EdgeCollider2D>();
        myPolyCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (getplayersonce)
        {
            getplayers();
        }
        else
        {
            DoesHeSeePlayer();
            if(SeePlayer)
            {
                UpdatePosition();
                UpdateArcher();
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
    public bool IsPlayerInFrontArcher()
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
    private void DoesHeSeePlayer()
    {
        //do for player[1] whichever one is thats the index 0 or 1, then just make that a variable
        float EnemyPlayerXDifference = transform.position.x - Players[0].transform.position.x;
        float EnemyPlayer2XDifference = transform.position.x - Players[1].transform.position.x;
        if (!SeePlayer)
        {
            if (Mathf.Abs(EnemyPlayerXDifference) < transform.parent.GetComponent<Archer>().GetVisionRange())
            {
                SeePlayer = true;
                playerseenindex = 0;
            }
            else if (Mathf.Abs(EnemyPlayer2XDifference) < transform.parent.GetComponent<Archer>().GetVisionRange())
            {
                SeePlayer = true;
                playerseenindex = 1;
            }
        }
        else
        {
            float EnemyPlayerXDifferenceafter = transform.position.x - Players[playerseenindex].transform.position.x;
            if (Mathf.Abs(EnemyPlayerXDifferenceafter) < transform.parent.GetComponent<Archer>().GetVisionRange())
            {

            }
            else
            {
                SeePlayer = false;
                transform.parent.GetComponent<Archer>().SeesPlayer(false);
            }
        }
    }
    private void UpdateArcher()
    {
       if(trigger == true)
        {
            transform.parent.GetComponent<Archer>().SeesPlayer(false);
        }
       else
        {
            if(Mathf.Abs(transform.position.x - Players[playerseenindex].transform.position.x) > transform.parent.GetComponent<Archer>().GetVisionRange())
            {
                transform.parent.GetComponent<Archer>().SeesPlayer(false);
            }
            else
            {
                transform.parent.GetComponent<Archer>().SeesPlayer(true);
            }
        }
    }
    public GameObject GetPlayer()
    {
        return Players[playerseenindex];
    }
    private void UpdatePosition()
    {
        Vector2[] pointsHolder;
        pointsHolder = myPolyCollider.points;
        pointsHolder[0] = new Vector2(theArcher.transform.position.x - theArcher.transform.position.x, theArcher.transform.position.y- theArcher.transform.position.y + .4f);
        pointsHolder[1] = new Vector2(Players[playerseenindex].transform.position.x - theArcher.transform.position.x, Players[playerseenindex].transform.position.y - theArcher.transform.position.y - .5f);
        pointsHolder[2] = new Vector2(Players[playerseenindex].transform.position.x - theArcher.transform.position.x, Players[playerseenindex].transform.position.y - theArcher.transform.position.y - .4f);
        myPolyCollider.points = pointsHolder;

        /*Vector2[] pointsHolder;
        pointsHolder = myCollider.points;
        pointsHolder[0] = new Vector2(theArcher.transform.position.x - theArcher.transform.position.x, theArcher.transform.position.y- theArcher.transform.position.y + .4f);
        pointsHolder[1] = new Vector2(thePlayer.transform.position.x - theArcher.transform.position.x, thePlayer.transform.position.y - theArcher.transform.position.y - .5f); 
        myCollider.points = pointsHolder;*/
    }


}
