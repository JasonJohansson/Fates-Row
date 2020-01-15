using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArcherSpawner : NetworkBehaviour
{
    [SerializeField] GameObject Archer;
    // Start is called before the first frame update
    void Start()
    {
        NetworkIdentity.Instantiate(Archer, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
