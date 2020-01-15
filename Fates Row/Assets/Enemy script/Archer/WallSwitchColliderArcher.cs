using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSwitchColliderArcher : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")//cant hit player anyways
        {
            return;
        }
        transform.parent.GetComponent<Archer>().ChildChangeisdirrightArcher();
    }
}
