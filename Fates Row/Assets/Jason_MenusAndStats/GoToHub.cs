using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GoToHub : MonoBehaviour
{
    public GameObject obj;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "HUB")
        {
            obj.SetActive(false);

        }
    }
}
