using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region UPDATE
    /*public class IdleState : State
    {
        public override string ChangeState(string transition)
        {
            switch (transition)
            {
                case "Move":
                    return "Running";
                default:
                    return null;
            }
        }

        public override void Update()
        {
            //put in idle logic
        }
    }

    public class RunState : State
    {
        public override string ChangeState(string transition)
        {
            switch (transition)
            {
                case "Stop":
                    return "Idling";
                default:
                    return null;
            }
        }

        public override void Update()
        {
            //put in run logic
        }
    }

    private FiniteStateMachine _fsm;

    private void Start()
    {
        Dictionary<string, State> states = new Dictionary<string, State>()
        {
            {"Idling", new IdleState() },
            {"Running", new RunState() }
        };

        _fsm = new FiniteStateMachine(states, "Idling");
    }

    private void Update()
    {
        _fsm.GetCurrentState().Update();
    }*/
    #endregion



    /*public float jump = 12;
    public static int doubleJump = 1;
    private int jumpRemain = doubleJump;
    public float runSpeed = 4;
    Rigidbody2D rigidbody2DComponent;
    float horizonAxis;


    void Awake()
    {
        rigidbody2DComponent = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButton("Horizontal"))
        {
            Run();
        }
    }

    void FixedUpdate()
    {

    }

    void Run()
    {
        if (Input.GetKey("d"))
        {
            transform.Translate(new Vector3(1, 0, 0) * runSpeed * Time.deltaTime);
            rigidbody2DComponent.AddForce(new Vector2(2, 0), ForceMode2D.Force);
        }
        else
        {
            transform.Translate(new Vector3(-1, 0, 0) * runSpeed * Time.deltaTime);
            rigidbody2DComponent.AddForce(new Vector2(-2, 0), ForceMode2D.Force);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Floor")
        {
            jumpRemain = doubleJump;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Item")
        {
            Destroy(col.gameObject);
            doubleJump++;
        }
    }

    void Jump()
    {
        if (jumpRemain > 0)
        {
            rigidbody2DComponent.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
            jumpRemain--;
        }
    }*/
}
