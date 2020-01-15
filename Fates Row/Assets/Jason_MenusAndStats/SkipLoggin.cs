using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SkipLoggin : MonoBehaviour
{
    void AlreadyLoggedIn()
    {
        if (AccountManager.isLoggedIn)
        {
            SceneManager.LoadScene("HUB");
        }
    }
}
