using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    protected enum FacingDirection { Right = 1, Left = -1}

    protected FacingDirection _currentFacingDirection;
    protected FacingDirection _initialDirection;

    protected abstract void InitFacingDirection();
}
