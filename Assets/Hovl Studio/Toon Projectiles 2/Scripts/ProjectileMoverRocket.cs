using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hovl_Studio.Toon_Projectiles_2.Scripts
{
    public class ProjectileMoverRocket : ProjectileMover
    {
        // public float launchForce = 2f;
        // public Vector3 targetPosition;
        // [SerializeField] private Rigidbody rb;
        // private bool hasReachedTarget = false;
        //
        // public void OnInit(Vector3 posTarget)
        // {
        //     targetPosition = posTarget;
        //     hasReachedTarget = false;
        //
        //     // Calculate initial launch direction and force
        //     Vector3 launchDirection = (transform.position - posTarget).normalized;
        //     rb.velocity = Vector3.zero;
        //     rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
        // }
        //
        // public override void FixedUpdate()
        // {
        //     if (hasReachedTarget) return;
        //
        //     float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        //
        //     if (distanceToTarget <= 0.5f)
        //     {
        //         rb.velocity = Vector3.zero;
        //         rb.angularVelocity = Vector3.zero;
        //         hasReachedTarget = true;
        //         Debug.Log("Reached target");
        //         return;
        //     }
        //
        //     // Update rotation to face movement direction
        //     if (rb.velocity != Vector3.zero)
        //     {
        //         transform.rotation = Quaternion.LookRotation(rb.velocity);
        //     }
        //
        // }
    
    }
}
