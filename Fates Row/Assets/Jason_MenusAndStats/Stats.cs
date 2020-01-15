using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public Text KillCount;
    public Text DeathCount;
    public Text lvl1Time;
    public Text lvl2Time;
    public Text lvl3Time;
    // Start is called before the first frame update
    void Start()
    {
        if(AccountManager.isLoggedIn)
        AccountManager.instance.GetData(OnReceivedData);
    }

    void OnReceivedData(string data)
    {
        KillCount.text = Parser.DataToKills(data).ToString();
        DeathCount.text = Parser.DataToDeaths(data).ToString();
        lvl1Time.text = Parser.DataToLVL1(data).ToString();
        lvl2Time.text = Parser.DataToLVL2(data).ToString();
        lvl3Time.text = Parser.DataToLVL3(data).ToString();
        if (lvl1Time.text.Equals("0"))
        {
            lvl1Time.text = "";
        }
        if (lvl2Time.text.Equals("0"))
        {
            lvl2Time.text = "";
        }
        if (lvl3Time.text.Equals("0"))
        {
            lvl3Time.text = "";
        }
    }
}
