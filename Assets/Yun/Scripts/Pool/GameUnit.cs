using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yun.Scripts.Cores;

public class GameUnit : YunBehaviour
{
    private Transform tf;
    public Transform TF
    {
        get
        {
            //tf = tf ?? gameObject.transform;
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }

    public PoolType poolType;
}