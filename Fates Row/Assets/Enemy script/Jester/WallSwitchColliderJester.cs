using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSwitchColliderJester : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            return;
        }
        transform.parent.GetComponent<Jester>().ChildChangeisdirrightArcher();
    }
}
