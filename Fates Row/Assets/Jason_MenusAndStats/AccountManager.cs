//Jason Johansson
/*
 * Modified version from Brackeys
 * on Youtube. Some fuctions from DCF's
 * provided script to access server.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DatabaseControl;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountManager : MonoBehaviour
{

    public static AccountManager instance;

    void Awake()
    {
        DontDestroyOnLoad(this);

        //if (instance != null)
        //{
       //    Destroy(gameObject);
       //    return;
      // }
        instance = this;
    }

    public static string LoggedInUserName { get; protected set; }
    private static string LoggedInUserPassword = "";

    public static string LoggedInData { get; protected set; }

    public static bool isLoggedIn;

    public string loggedInSceneName = "SampleScene";
    public string loggedOutSceneName = "Login";
    public delegate void OnDataReceivedCallBack(string data);

    public void LogOut()
    {
        LoggedInUserName = "";
        LoggedInUserPassword = "";
        isLoggedIn = false;
        SceneManager.LoadScene(loggedOutSceneName);
        Debug.Log("Logged Out");
    }
    public void LogIn(string userName, string password)
    {
        LoggedInUserName = userName;
        LoggedInUserPassword = password;
        isLoggedIn = true;
        SceneManager.LoadScene(loggedInSceneName);
        Debug.Log("Logged in as " + LoggedInUserName);
    }
    public void AlreadyLoggedIn()
    {
        if (isLoggedIn)
        {
            Debug.Log("Already Logged In");
            SceneManager.LoadScene("SampleScene");
        }
        else
            Debug.Log("Not already logged in");
    }
    public void SendData(string data)
    {
       
        if (isLoggedIn)
        {
            StartCoroutine(SendNeededData(data));
        }

    }
    //public void LoggedIn_LoadDataButtonPressed(OnDataReceivedCallBack OnDataReceived)
   // {
//
   //     GetData(OnDataReceived);
   // }

    public void GetData(OnDataReceivedCallBack OnDataReceived)
    {
        if (isLoggedIn)
        {
            StartCoroutine(SendGetDataRequest(OnDataReceived));
        }
    }
     IEnumerator SendGetDataRequest(OnDataReceivedCallBack OnDataReceived)
    {
        string data = "Error";

        IEnumerator e = DCF.GetUserData(LoggedInUserName, LoggedInUserPassword); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request



        if (response == "Error")
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            Debug.Log("Login error");
        }
        else
        {
            string DataRecieved = response;
            data = DataRecieved;
        }
        if(OnDataReceived != null)
            OnDataReceived.Invoke(data);
    }
    IEnumerator SendNeededData(string dataNeeded)
    {
        IEnumerator e = DCF.SetUserData(LoggedInUserName, LoggedInUserPassword, dataNeeded); // << Send request to set the player's data string. Provides the username, password and new data string
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
            //The data string was set correctly. Goes back to LoggedIn UI
            //            loggedInParent.gameObject.SetActive(true);
        }
        else
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.

        }
    }
}

