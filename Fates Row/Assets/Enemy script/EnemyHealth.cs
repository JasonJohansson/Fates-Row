using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int Health; 
    Rigidbody2D myrigidbody;
    // Start is called before the first frame update
    void Start()
    {
        myrigidbody = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            if (tag == "LavaBeast")
            {
                GetComponent<LavaBeast>().LavaBeastDeath();
            }
            else
            {
                Destroy(gameObject);
            }
            FindObjectOfType<PlayerUnit>().levelKillCounter++; //Jason
        }
    }

    public void LowerHealth(int h)
    {
        Health = Health - h;
        DealWithHitBack();
        if (Health <= 0)
        {
            if(tag == "LavaBeast")
            {
                GetComponent<LavaBeast>().LavaBeastDeath();
            }
            else
            {
                Destroy(gameObject);
            }
           // FindObjectOfType<PlayerUnit>().levelKillCounter++; //Jason
        }
    }
    private void DealWithHitBack()
    {
       string enemyname = GetComponent<Animator>().name;
        Debug.Log(enemyname);
        switch(enemyname)
        {
            case "Enemy Possum": BroadcastMessage("PossumFreeze", 0);
                break;
            case "Enemy Frog": BroadcastMessage("FrogFreeze", 0);
                break;
            case "Enemy Skeleton": BroadcastMessage("SkeletonFreeze", 0);
                break;
            case "Enemy Bird": BroadcastMessage("BirdFreeze", 0);
                break;
            case "Enemy Mage": BroadcastMessage("MageFreeze", 0);
                break;
            case "Enemy Archer": BroadcastMessage("ArcherFreeze", 0);
                break;
            case "Enemy Jester": BroadcastMessage("JesterFreeze", 0);
                break;
            case "Enemy Knight": BroadcastMessage("KnightFreeze", 0);
                break;
        }
    }
}
