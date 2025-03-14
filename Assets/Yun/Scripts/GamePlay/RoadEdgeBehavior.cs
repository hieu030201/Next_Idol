using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yun.Scripts.GamePlay.Vehicles;

public class RoadEdgeBehavior : MonoBehaviour
{
    public float bounceForce = 1000f;
    public float upwardForce = 200f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle")) // Đảm bảo xe có tag "Player"
        {
            Rigidbody carRb = other.GetComponent<Rigidbody>();
            if (carRb != null)
            {
                Debug.Log("BBBBBBBBBBBBBBBBBBBBBBBBBBBB ");
                Vector3 bounceDirection = (other.transform.position - transform.position).normalized;
                bounceDirection.y = 0; // Giữ cho lực ngang
                
                // Áp dụng lực nẩy
                carRb.AddForce(bounceDirection * bounceForce + Vector3.up * upwardForce, ForceMode.Impulse);
            }
        }
    }
}
