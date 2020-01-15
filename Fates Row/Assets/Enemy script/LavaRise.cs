using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRise : MonoBehaviour
{
    float wheretoriseto = -6f;
    float risespeed = 1f;
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(counter > 100)
        {

        }
        else
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + risespeed * Time.deltaTime);
        }
        counter++;
    }
}
