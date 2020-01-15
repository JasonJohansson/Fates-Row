using UnityEngine;
using System.Collections;

public class spikes : MonoBehaviour
{
    public int damage;

    // Use this for initialization
    void OnTriggerEnter2D(Collider2D Collider)
    {

        if (Collider.gameObject.tag == "Player")
        {
            Collider.GetComponent<Health>().damagePlayer(damage);
            Debug.Log("spike collision detected");
            Debug.Log("applied damage to player");
        }
    }
}