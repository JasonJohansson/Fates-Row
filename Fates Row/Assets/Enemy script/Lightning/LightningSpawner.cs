using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpawner : MonoBehaviour
{
    [SerializeField] GameObject lightningstraight;
    [SerializeField] GameObject lightningup;
    [SerializeField] GameObject lightningdown;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(lightningstraight, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //spawn at angle facing player
        //randomly spawn 1 of the 3 lightnings after eachother making sure it doesnt happen too often in a rowa
        //have a number starting at 0 if its up it goes up, down it goes down if absolute value is past 2 or 3 or something it cant go higher and vice versa
    }
}
