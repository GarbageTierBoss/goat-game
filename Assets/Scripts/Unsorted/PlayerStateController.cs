using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    #region States
    public class InitialFallState : PlayerState
    {
        public override string ChangeState(string transition)
        {
            switch (transition)
            {
                case "Ram":
                    return "Ramming";
                case "AirWalk":
                    return "AirWalking";
                case "NormalLand":
                    return "NormalLanding";
                case "TerminalFall":
                    return "TerminalFalling";
                default:
                    return null;
            }
        }

        public override void Update()
        {
            //InitialFall logic
        }
    }

    public class IdleState : PlayerState
    {
        public override string ChangeState(string transition)
        {
            switch (transition)
            {
                case "Ram":
                    return "Ramming";
                case "InitialFall":
                    return "InitialFalling";
                case "InitialJump":
                    return "InitialJumping";
                case "Walk":
                    return "Walking";
                default:
                    return null;
            }
        }

        public override void Update()
        {
            //Idle logic
        }
    }
    #endregion

    private PlayerStateMachine _psm;

    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, PlayerState> states = new Dictionary<string, PlayerState>()
        {
            { "InitialFalling", new InitialFallState() },
            { "Idling", new IdleState() }
        };
    }

    // Update is called once per frame
    void Update()
    {
        _psm.GetCurrentState().Update();
    }
}
