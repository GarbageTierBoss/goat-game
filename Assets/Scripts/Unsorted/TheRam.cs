using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheRam : MonoBehaviour //Inherit from action (IS-A)
{
    private enum States { NotDashing, Dashing, Recoiling, Decelerating }

    [SerializeField] KeyCode _triggerKey;

    [SerializeField] float _maxRamSpeed = 4;
    [SerializeField] float _accelerationTime = 1/6f;
    [SerializeField] float _decelerationTimeIfNoImpact;
    [SerializeField] float _recoilTimeIfHitObstacle;
    [SerializeField] float _durationOfDash;

    private bool _isCrashed;
    private float _timeElapsed;

    Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_triggerKey))   //space recommended
        {
            TurnOffPlayerController();
            _timeElapsed = 0;
            StartCoroutine(AccelerateDash());
            //once process is over, turn player back on
        }
    }

    IEnumerator AccelerateDash()
    {
        //max speed / time
        _rb.velocity += new Vector2(Mathf.Clamp(_maxRamSpeed / _accelerationTime, 0, _maxRamSpeed), 0);
        _timeElapsed += Time.deltaTime;

        yield return new WaitForEndOfFrame();

        if (_timeElapsed <= _durationOfDash)
        {
            StartCoroutine(AccelerateDash());
        }
        else
        {
            StopDash();
        }

    }

    private void StopDash()
    {
        _rb.velocity = Vector2.zero;
        TurnOnPlayerController();
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
