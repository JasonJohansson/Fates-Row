using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBeast : MonoBehaviour
{
    [SerializeField] int Health = 50;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float roamRange = 10f;
    [SerializeField] flameground theGroundFire;
    [SerializeField] int groundfireMoveSpeed = 10;
    [SerializeField] FireBall velocitygivenBall;
    [SerializeField] FireBall nogravityBall;
    [SerializeField] FireBall cosineBall;
    [SerializeField] FireBall immobileBall;
    [SerializeField] FireBall straightball;

    int isdirright = 1;
    float initialposition;
    //deal with phases
    int phasecounter = 1;
    bool lavagroundphase = true;//true
    bool burstattackphase = false;
    bool laserbeamphase = false;
    bool flowattackphase = false;
    bool cosinephase = false;
    float switchphasecounter = 1;

    //cosine attack
    bool cosineattackagain = true;

    //flow attack
    bool flowattackagain = true;
    bool flowone = true;

    //laserbeam attack
    bool laserattackagain = true;
    float laseroffset = 0;
    bool laserswitch = false;
    float laserbeamangle = 0;
    float laserradius = 5;
    float anglechange = 4f;

    //lavaground attack
    bool flameground1 = false;
    bool flameground2 = true;
    bool attackAgain = true;
    int fireballcountdelay = 0;
    float groundfiredelay = 1f;//.05 is good for compact line

    //burst attack
    bool burstattackAgain = true;
    float burstfiredelay = 2f;
    bool BossDead = false;
    Animator myAnimator;
    //Player thePlayer;
    Rigidbody2D myRigidBody;
    bool BossFreezed = false;
    GameObject[] Players;
    bool getplayersonce = true;
    //laser beam like from boshy tekken boss
    //lava shoots out around him circular 
    //has bomb lava that he throws and breaks into more
    //lava balls that fly in any direction
    //rain of lava
    //charge attack
    //line of lava on the floor you have to jump to avoid

    //**punches ground and lava balls fly ina outward circle
    // Start is called before the first frame update
    void Start()
    {
        initialposition = transform.position.x;
        myRigidBody = GetComponent<Rigidbody2D>();
       // thePlayer = FindObjectOfType<Player>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BossDead)
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
                FacePlayer();
                DealWithPhases();
                //CheckIfEnemyIsWithinRange();
                //Roam();
                if (burstattackphase)
                {
                    if (burstattackAgain)
                    {
                        burstattackAgain = false;
                        BurstAttack();
                    }
                }
                if (lavagroundphase)
                {
                    if (attackAgain)
                    {
                        attackAgain = false;
                        LavaGroundAttack();
                    }
                }
                if (cosinephase)
                {
                    if (cosineattackagain)
                    {
                        cosineattackagain = false;
                        StartCoroutine(Cosineattack());
                    }
                }
                if (flowattackphase)
                {
                    if (flowattackagain)
                    {
                        flowattackagain = false;
                        StartCoroutine(FlowAttack());
                    }
                }
                if (laserbeamphase)
                {
                    if (laserattackagain == true)
                    {
                        laserattackagain = false;
                        StartCoroutine(LaserBeamAttack());
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
    }
    private void DealWithPhases()
    {
        /*bool lavagroundphase = true;
        bool burstattackphase = false;
        bool laserbeamphase = false;
        bool cosinephase = false;*/

        if (switchphasecounter % 6 == 0)//1000
        {
            if(!BossFreezed)
            {
                StartCoroutine(BossFrozen());
                switchphasecounter+= 1 * Time.deltaTime;
                BossFreezed = true;
            }
        }
        if(!BossFreezed)
        {
            switchphasecounter += 1 * Time.deltaTime;
        }
    }
    IEnumerator BossFrozen()
    {
        lavagroundphase = false;
        burstattackphase = false;
        flowattackphase = false;
        laserbeamphase = false;
        myAnimator.SetTrigger("hit_1");
        yield return new WaitForSeconds(4f);
        BossFreezed = false;
        if (phasecounter == 1)
        {
            lavagroundphase = false;
            burstattackphase = true;
        }
        if (phasecounter == 2)
        {
            burstattackphase = false;
            flowattackphase = true;
        }
        if (phasecounter == 3)
        {
            flowattackphase = false;
            laserbeamphase = true;
        }
        if (phasecounter == 4)
        {
            laserbeamphase = false;
            lavagroundphase = true;
        }
        phasecounter++;
        if (phasecounter > 4)
        {
            phasecounter = 1;
        }
    }
    private void BurstAttack()
    {
        StartCoroutine(BurstSpawn());
    }
    IEnumerator FlowAttack()
    {
        myAnimator.SetTrigger("jump");
        yield return new WaitForSeconds(.8f);
        //cool looking
        /* FireBall ball = Instantiate(velocitygivenBall, transform.position, Quaternion.identity) as FireBall;
            float x = -3 - i;
            float y = 5 + 2*i;
            ball.setVelocity(x, y);
            yield return new WaitForSeconds(.1f);*/
        bool left = true;
        if (transform.localScale.x < 0)
        {
            left = true;
        }
        else
        {
            left = false;
        }
        if (flowone)
        {
            flowone = false;
            int xoffset = Random.Range(2, 4);
            int yoffset = Random.Range(2, 4);
            for (int i = 0; i < 10; i++)
            {
                FireBall ball = Instantiate(velocitygivenBall, transform.position, Quaternion.identity) as FireBall;
                float x;
                if (left)
                {
                    x = -20 + xoffset * i;//20
                }
                else
                {
                    x = 20 - xoffset * i;
                }
                float y = 20 - yoffset * i;//20,2
                ball.setVelocity(x, y);
                if (left)
                {
                    if (x > 0 || y < 0)
                    {
                        break;
                    }
                }
                else
                {
                    if (x < 0 || y < 0)
                    {
                        break;
                    }
                }
                yield return new WaitForSeconds(.1f);
            }
        }
        else
        {
            flowone = true;
            int xoffset = Random.Range(1, 3);
            int yoffset = Random.Range(2, 4);
            for (int i = 0; i < 10; i++)
            {
                FireBall ball = Instantiate(velocitygivenBall, transform.position, Quaternion.identity) as FireBall;
                //float x = -3 - i;
                //float y = 5 + 2 * i;
                float x;
                if (left)
                {
                    x = -20 + xoffset * i;
                }
                else
                {
                    x = 20 - xoffset * i;
                }
                //float y = Mathf.Log10(2*i + 1);
                // Debug.Log(y);
                float y = yoffset * i + 10;
                 //float y = 20 - 2 * i;
                ball.setVelocity(x, y);
                if (left)
                {
                    if (x > 0 || y < 0)
                    {
                        break;
                    }
                }
                else
                {
                    if (x < 0 || y < 0)
                    {
                        break;
                    }
                }
                yield return new WaitForSeconds(.1f);
            }
        }
        yield return new WaitForSeconds(1f);
        flowattackagain = true;
    }
    IEnumerator LaserBeamAttack()
    {
        myAnimator.SetTrigger("skill_3");
        laseroffset = Mathf.Abs(-laserradius * Mathf.Cos(((Mathf.PI * laserbeamangle) / 180) - Mathf.PI / 2)) + 4;//-cos(x-pi/2) + 2
        FireBall ballupper = Instantiate(nogravityBall, new Vector2(transform.position.x-2f, transform.position.y+3), Quaternion.identity) as FireBall;
        ballupper.setVelocity(-17f, 0);
        ballupper.setoffest(laseroffset);
        FireBall ball = Instantiate(nogravityBall, new Vector2(transform.position.x-2f, transform.position.y+2), Quaternion.identity) as FireBall;
        ball.setVelocity(-17f,0);
        ball.setoffest(-laseroffset);
        yield return new WaitForSeconds(.01f);//.01f
        if (laserbeamangle > 180)
        {
            laserradius = Random.Range(1f, 6f);
            anglechange = Random.Range(4, 8);
            laserbeamangle = 0;
        }
        laserbeamangle+= anglechange;
   
        laserattackagain = true;
    }
    IEnumerator Cosineattack()
    {
        myAnimator.SetTrigger("skill_1");
        FireBall ball = Instantiate(cosineBall, new Vector2(transform.position.x,transform.position.y + 1), Quaternion.identity) as FireBall;
        ball.setMoveSpeed(Random.Range(5f,10f));
        ball.setDesiredRadius(Random.Range(1f, 10f));
        ball.setthetalower(Random.Range(0,2));
        yield return new WaitForSeconds(.5f);
        cosineattackagain = true;
    }
    IEnumerator BurstSpawn()
    {
        //settings 1 is gravity scale 2, x(-10,10),y(15,30) spawning 30 makes them go high and shoot down fast
        //settings gravity scale 1, x(-10,10),y(10,15) spawning 30 very symetric regular burst
        myAnimator.SetTrigger("jump");
        yield return new WaitForSeconds(.8f);
        for (int i = 0; i < 30; i++)
        {
            FireBall ball = Instantiate(velocitygivenBall, transform.position, Quaternion.identity) as FireBall;
            float x = Random.Range(-13f,13f);//-10f,10f
            float y = Random.Range(15f, 30f);//10f,15f
            ball.setVelocity(x,y);
        }
        yield return new WaitForSeconds(burstfiredelay);
        burstattackAgain = true;
    }
    private void FacePlayer()
    {
        if(Players[0].transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector2(-.02f, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(.02f, transform.localScale.y);
        }
    }

    private void LavaGroundAttack()
    {
        StartCoroutine(FireGround());
    }
    IEnumerator FireGround()
    {
        myAnimator.SetTrigger("skill_1");
        if(fireballcountdelay  % 5 == 0)
        {
            FireBall ball = Instantiate(velocitygivenBall, transform.position, Quaternion.identity) as FireBall;
            float x = Random.Range(-15f, -8f);//-10f,10f
            float y = Random.Range(15f, 20f);//10f,15f
            ball.setVelocity(x, y);
        }
        fireballcountdelay++;

        flameground flame = Instantiate(theGroundFire, new Vector2(transform.position.x,transform.position.y + .3f), Quaternion.identity) as flameground;
        if(transform.localScale.x < 0)
        {
            flame.setFlameMoveSpeed(-groundfireMoveSpeed);
        }
        else
        {
            flame.setFlameMoveSpeed(groundfireMoveSpeed);
        }
        if(flameground1)
        {
            if (Random.Range(-70, 30) < 0)
            {
                groundfiredelay = .05f;
            }
            else
            {
                groundfiredelay = 1f;
            }
        }
        else if(flameground2)
        {
            //40 percent quick 30 percent long 30 percent mediuem
            int i = Random.Range(0,100);
            if (i < 50)
            {
                groundfiredelay = .05f;
            }
            else if(i > 50 && i < 80)
            {
                groundfiredelay = .5f;
            }
            else
            {
                groundfiredelay = 1f;
            }
        }
        yield return new WaitForSeconds(groundfiredelay);
        attackAgain = true;
    }
    private void CheckIfEnemyIsWithinRange()
    {
        if (transform.position.x > initialposition + roamRange || transform.position.x < initialposition - roamRange)
        {
            ChildChangeisdirrightLava();
        }
    }
    public void ChildChangeisdirrightLava()
    {
        isdirright = isdirright * -1;
    }
    private void Roam()
    {
        if (isdirright == 1)
        {    
                transform.localScale = new Vector2(.02f, transform.localScale.y);
                myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y); 
        }
        else
        {
            transform.localScale = new Vector2(-.02f, transform.localScale.y);
            myRigidBody.velocity = new Vector2(-moveSpeed, myRigidBody.velocity.y);
        }
    }
    public void LavaBeastDeath()
    {
        myAnimator.SetTrigger("death");
        BossDead = true;
        lavagroundphase = false;
        burstattackphase = false;
        flowattackphase = false;
        laserbeamphase = false;
        GetComponentInChildren<EnemyToPlayerCollider>().GetComponent<BoxCollider2D>().enabled = false;
    }
}
