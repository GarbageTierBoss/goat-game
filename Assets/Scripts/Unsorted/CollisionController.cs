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

        if (GetComponent<PlayerController>())
        {
            

            GetComponent<PlayerController>().StopJump();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GetComponent<PlayerController>().IsWalking())
        {
            GetComponent<PlayerController>().StopWalking();
        }
    }
}


