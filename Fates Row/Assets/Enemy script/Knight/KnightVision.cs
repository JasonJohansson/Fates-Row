using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightVision : MonoBehaviour
{
    //cache
    EdgeCollider2D myCollider;
   // Player thePlayer;
    Knight theKnight;
    PolygonCollider2D myPolyCollider;
    GameObject[] Players;
    bool getplayersonce = true;
    bool SeePlayer = false;
    int playerseenindex = 0;
    bool trigger;
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
        theKnight = transform.parent.GetComponent<Knight>();
       // thePlayer = FindObjectOfType<Player>();
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
                UpdateKnight();
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
    public bool IsPlayerInFrontKnightVision()
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
    public float KnightVisionPlayerX()
    {
        return Players[playerseenindex].transform.position.x;
    }
    public float KnightVisionPlayerY()
    {
        return Players[playerseenindex].transform.position.y;
    }
    private void DoesHeSeePlayer()
    {
        //do for player[1] whichever one is thats the index 0 or 1, then just make that a variable
        float EnemyPlayerXDifference = transform.position.x - Players[0].transform.position.x;
        float EnemyPlayer2XDifference = transform.position.x - Players[1].transform.position.x;
        if (!SeePlayer)
        {
            if (Mathf.Abs(EnemyPlayerXDifference) < transform.parent.GetComponent<Knight>().GetVisionRange())
            {
                SeePlayer = true;
                playerseenindex = 0;
            }
            else if (Mathf.Abs(EnemyPlayer2XDifference) < transform.parent.GetComponent<Knight>().GetVisionRange())
            {
                SeePlayer = true;
                playerseenindex = 1;
            }
        }
        else
        {
            float EnemyPlayerXDifferenceafter = transform.position.x - Players[playerseenindex].transform.position.x;
            if (Mathf.Abs(EnemyPlayerXDifferenceafter) < transform.parent.GetComponent<Knight>().GetVisionRange())
            {

            }
            else
            {
                SeePlayer = false;
                transform.parent.GetComponent<Knight>().SeesPlayer(false);
            }
        }
    }
    private void UpdateKnight()
    {
        if (trigger == true)
        {
            transform.parent.GetComponent<Knight>().SeesPlayer(false);
        }
        else
        {
            transform.parent.GetComponent<Knight>().SeesPlayer(true);
        }
    }

    private void UpdatePosition()
    {
        Vector2[] pointsHolder;
        pointsHolder = myPolyCollider.points;
        pointsHolder[0] = new Vector2(theKnight.transform.position.x - theKnight.transform.position.x, theKnight.transform.position.y - theKnight.transform.position.y + .4f);
        pointsHolder[1] = new Vector2(Players[playerseenindex].transform.position.x - theKnight.transform.position.x, Players[playerseenindex].transform.position.y - theKnight.transform.position.y - .5f);
        pointsHolder[2] = new Vector2(Players[playerseenindex].transform.position.x - theKnight.transform.position.x, Players[playerseenindex].transform.position.y - theKnight.transform.position.y - .4f);
        myPolyCollider.points = pointsHolder;
    }
}
