using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheRam : MonoBehaviour //Inherit from action (IS-A)
{
    private enum States { NotDashing, Dashing, Recoiling, Decelerating }

    [SerializeField] KeyCode _triggerKey;

    [SerializeField] float _maxRamSpeed;
    [SerializeField] float _accelerationTime;
    [SerializeField] float _decelerationTimeIfNoImpact;
    [SerializeField] float _recoilTimeIfHitObstacle;

    private bool impact;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_triggerKey))   //space recommended
        {
            TurnOffPlayerController();

            //once process is over, turn player back on
        }
    }

    private void TurnOffPlayerController()
    {
        if (GetComponent<PlayerController>())
        {
            GetComponent<PlayerController>().enabled = false;
        }
        else
        {
            throw new System.ArgumentException("No Player Controller found");
        }
    }

    private void TurnOnPlayerController()
    {
        GetComponent<PlayerController>().enabled = true;
    }
}
