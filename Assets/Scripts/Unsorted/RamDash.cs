﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamDash : MonoBehaviour //Inherit from action (IS-A)
{
    [SerializeField] KeyCode _triggerKey;
    [SerializeField] float _maxRamSpeed = 4;
    [SerializeField] float _accelerationTime = 1/6f;
    [SerializeField] float _decelerationTimeIfNoImpact;
    [SerializeField] float _recoilTimeIfHitObstacle;
    [SerializeField] float _dashDistance;

    private bool _isCrashed = false;
    private float _timeElapsed;

    private Vector2 _initialLocation;
    private Vector2 _crashLocation;
    private Rigidbody2D _rb;

    private bool _ramCommand;

    public GameObject cursor;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckTriggerKey();

        if (_ramCommand)
        {
            _rb.position = Vector2.MoveTowards(_rb.position, DashDestination(), _maxRamSpeed);
            if (_rb.position == DashDestination())
            {
                _ramCommand = false;
                TurnOnPlayerController();
            }
        }
    }

    private void CheckTriggerKey()
    {
        if (Input.GetKeyDown(_triggerKey))   //space recommended
        {
            _ramCommand = true;

            TurnOffPlayerController();

            /*_timeElapsed = 0;
            _initialLocation = _rb.position;

            StartCoroutine(AccelerateDash());*/
        }
    }

    private Vector2 FindMousePositionInWorldPoint()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private Vector2 VectorFromGoatToMouse()
    {
        return FindMousePositionInWorldPoint() - _rb.position;
    }

    private Vector2 DashDestination()
    {
        Vector2 direction = FindMousePositionInWorldPoint().normalized;
        return direction * _dashDistance;
    }

    public void OnMouseUp()
    {
        Debug.Log(DashDestination());
        _ramCommand = true;
    }

    /*IEnumerator AccelerateDash()
    {
        //max speed / time
        _rb.velocity += new Vector2(Mathf.Clamp(_maxRamSpeed / _accelerationTime, 0, _maxRamSpeed), 0);
        _timeElapsed += Time.deltaTime;

        yield return new WaitForEndOfFrame();

        float currentDistanceTravelled = Mathf.Abs(_initialLocation.magnitude - _rb.position.magnitude);

        if (/*currentDistanceTravelled <= _dashDistance(_timeElapsed <= _durationOfDash) && !_isCrashed)
        {
            StartCoroutine(AccelerateDash());
        }
        else
        {
            if (_isCrashed)
            {
                //recoil
            }
            else
            {
                //_rb.position = new Vector2(_initialLocation.x + _dashDistance, _rb.position.y);
                //decelerate
                StartCoroutine(DecelerateDash());

                Debug.Log("Decelerating dash.");
            }
        }
    }

    IEnumerator DecelerateDash()
    {
        _rb.velocity += new Vector2(Mathf.Clamp(-_maxRamSpeed/_decelerationTimeIfNoImpact, -_maxRamSpeed, _maxRamSpeed), 0);

        yield return new WaitForEndOfFrame();

        if (_rb.velocity.x > 0)
        {
            StartCoroutine(DecelerateDash());
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
    }*/

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