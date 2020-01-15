using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSwitchColliderSkeleton : MonoBehaviour
{
    bool doesEnemyHugEdges = false;
    private void OnTriggerExit2D(Collider2D collision)
    {
        doesEnemyHugEdges = transform.parent.GetComponent<Skeleton>().GetEnemyHugEdgeState();
        if (collision.name == "Player")
        {
            return;
        }
        if(!doesEnemyHugEdges)
        {
            return;
        }
        transform.parent.GetComponent<Skeleton>().ChildChangeisdirrightSkeleton();
    }
}
