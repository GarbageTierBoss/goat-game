using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollidable//<T> where T : class
{
    //Collidables: platform / walls / boulders / hazard / smashable

    void OnCollision();

    //T GetCollisionType();
}
