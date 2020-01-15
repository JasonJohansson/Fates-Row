using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    [SerializeField]
    AudioClip[] aclip;
    public int CurrentScene;

    void Awake()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
         //   player.GetComponent<AudioSource>().clip = ;

        }
    }

}
