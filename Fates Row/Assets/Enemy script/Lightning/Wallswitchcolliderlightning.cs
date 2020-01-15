using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallswitchcolliderlightning : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            return;
        }
        transform.parent.GetComponent<Lightning>().ChildChangeisdirrightArcher();
    }
}
