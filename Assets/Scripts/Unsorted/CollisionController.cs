using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private Collider2D _collidedCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _collidedCollider = collision;

        if (GetComponent<RamDash>())
        {
            if (GetComponent<RamDash>().IsRamming())
            {
                GetComponent<RamDash>().StopRam();
            }
        }

        if (GetComponent<StatelessPlayerController>())
        {
            

            GetComponent<StatelessPlayerController>().StopJumping();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GetComponent<StatelessPlayerController>().IsWalking())
        {
            GetComponent<StatelessPlayerController>().StopWalking();
        }
    }
}


