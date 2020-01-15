using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
public class PlayerUnit : NetworkBehaviour
{
    public CharacterController2D controller;
    float horizontalMove = 0f;
    public float runSpeed = 150f;
    bool jump = false;
    public Joystick joy;
    public Animator ani;

    public int levelKillCounter = 0; //Jason
    public int levelDeathCounter = 0;//Jason
    
    void Start() //Jason
    {
        StartCoroutine(SyncScore()); 
    }
    
    void onDestroy()
    {
        if(this != null)
            SyncNow();
    }
    // Update is called once per frame
    void Update()
    {
        if (hasAuthority == false)
        {
            return;
        }
        ani.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (joy.Horizontal > 0f)
        {
            horizontalMove = runSpeed;
            // ani.SetFloat("Speed", horizontalMove);
        }
        else if (joy.Horizontal < 0f)
        {
            horizontalMove = -runSpeed;
        }
        else
        {
            horizontalMove = 0f;

        }
         
        //Debug.Log("speed is " + ani.GetFloat("Speed"));
        }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
    public void OnLand()
    {
        ani.SetBool("jump", false);
    }
    public void isJumping() {
        ani.SetBool("jump", true);
        jump = true;
        
    }
    
   
    /* Jason's Database Functions */

    IEnumerator SyncScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);


            SyncNow();
        }
    }
    void SyncNow()
    {
        if (AccountManager.isLoggedIn)
        {
            AccountManager.instance.GetData(OnDataReceived);
        }
    }
    
    void OnDataReceived(string data)
    {
        if (levelKillCounter == 0 && levelDeathCounter == 0)
            return;

        int kills = Parser.DataToKills(data);
        int deaths = Parser.DataToDeaths(data);

        int totalKills = kills + levelKillCounter;
        int totalDeaths = deaths + levelDeathCounter;

        string newData = Parser.ValuesToData(totalKills, totalDeaths);
        Debug.Log("Syncing" + newData);
        levelKillCounter = 0;
        levelDeathCounter = 0;
        AccountManager.instance.SendData(newData);
    }
}