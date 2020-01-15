using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    GameObject[] Players;
    bool getplayersonce = true;
    int maxhealthone;
    int currenthealthone;
    int maxhealthtwo;
    int currenthealthtwo;
    // Start is called before the first frame update
    void Start()
    {
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
            maxhealthone = Players[0].GetComponent<Health>().GetMaxHealth();
            currenthealthone = Players[0].GetComponent<Health>().GetCurrentHealth();
            maxhealthtwo = Players[1].GetComponent<Health>().GetMaxHealth();
            currenthealthtwo = Players[1].GetComponent<Health>().GetCurrentHealth();
            GetComponent<Text>().text = "P1 HP" + currenthealthone + "/" + maxhealthone + "P2 HP" + currenthealthtwo + "/" + maxhealthtwo;
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
}
