using UnityEngine;
using Yun.Scripts.GamePlay;
using Yun.Scripts.GamePlay.Enemies;
using Yun.Scripts.GamePlay.IdleGame.Rooms.Others;

namespace Hovl_Studio.Toon_Projectiles_2.Scripts
{
    public class ProjectileMover : GameUnit
    {
        public int powerNumber = 1;
        public float timeLife = 1f;
        public float speed = 25f;
        public float spawnTime = 0.2f;
        public float hitOffset = 0f;
        public bool UseFirePointRotation;
        public Vector3 rotationOffset = new Vector3(0, 0, 0);
        public GameObject hit;
        public GameObject flash;
        private Rigidbody rb;
        public GameObject[] Detached;
        private CounterTime counterTime = new CounterTime();

        public virtual void OnInit()
        {
            rb = GetComponent<Rigidbody>();
            if (flash != null)
            {
                var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
                flashInstance.transform.forward = gameObject.transform.forward;
                var flashPs = flashInstance.GetComponent<ParticleSystem>();
                if (flashPs != null)
                {
                    Destroy(flashInstance, flashPs.main.duration);
                }
                else
                {
                    var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(flashInstance, flashPsParts.main.duration);
                }
            }
            counterTime.Start(OnDespawn, timeLife);
        }

        public virtual void FixedUpdate ()
        {
            if (speed != 0)
            {
                counterTime.Execute();
                rb.velocity = transform.forward * speed;
                //transform.position += transform.forward * (speed * Time.deltaTime);       
            }
        }

        //https ://docs.unity3d.com/ScriptReference/Rigidbody.OnCollisionEnter.html
        public virtual void OnCollisionEnter(Collision collision)
        {
             // var contact = collision.contacts[0];
             // var rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
             // var pos = contact.point + contact.normal * hitOffset;
             //
             // if (hit != null)
             // {
             //     var hitInstance = Instantiate(hit, pos, rot);
             //     if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
             //     else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
             //     else { hitInstance.transform.LookAt(contact.point + contact.normal); }
             //
             //     var hitPs = hitInstance.GetComponent<ParticleSystem>();
             //     if (hitPs != null)
             //     {
             //         Destroy(hitInstance, hitPs.main.duration);
             //     }
             //     else
             //     {
             //         var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
             //         Destroy(hitInstance, hitPsParts.main.duration);
             //     }
             // }
             // foreach (var detachedPrefab in Detached)
             // {
             //     if (detachedPrefab != null)
             //     {
             //         detachedPrefab.transform.parent = null;
             //     }
             // }
            
            OnDespawn();
            var target = collision.gameObject;
            var receiver = target.GetComponent<IReceiveProjectile>();
            if (receiver != null) receiver.TakeDamage(powerNumber);

            var overlay = target.GetComponent<ColorOverlay>();
            if (overlay != null) overlay.VictimHit();
        }

        public void OnDespawn()
        {
            counterTime.Cancel();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            
            SimplePool.Despawn(this);
        }
    }
}
