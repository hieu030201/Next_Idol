using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float speed = 3.0f;
    public Camera cam;
    public Transform destination;
    public Vector3 lastSpeed = new Vector3();
    public Transform enemyAim;
    

    public Vector3 enemyAimPos
    {
        get{ return enemyAim.position; }
    }


}
