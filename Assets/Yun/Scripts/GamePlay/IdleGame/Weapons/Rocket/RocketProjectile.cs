using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjecttile : MonoBehaviour
{
    private Vector3 StartPosition;

    [SerializeField] private float StartSpeed = 5f;

    private Vector3 vector_v_x;
    private Vector3 vector_v_y;

    private float v_x;
    private float v_y;
    private float t;
    private float alpha;

    [SerializeField] private float gravity = 5f;
    private void Start()
    {
        StartPosition = transform.position;
        vector_v_x = new Vector3(transform.forward.x, 0, transform.forward.z);
        vector_v_y = new Vector3(0, transform.forward.y, 0);
        alpha = Vector3.Angle(vector_v_x, transform.forward);
        v_x = StartSpeed * Mathf.Cos(alpha * Mathf.Deg2Rad);
    }

    private void FixedUpdate()
    {
        transform.position = StartPosition + vector_v_x * v_x * t;
        var y = StartSpeed * Mathf.Sin(alpha * Mathf.Deg2Rad) * t - 0.5f * gravity * t * t;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);

        var v_y = StartSpeed * Mathf.Sin(alpha * Mathf.Deg2Rad) - gravity * t;
        var vector_v_0_at_t = vector_v_x * v_x + vector_v_y * v_y;
        transform.forward = vector_v_0_at_t;
        t = t + Time.fixedDeltaTime;
    }
}
