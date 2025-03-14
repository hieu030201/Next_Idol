using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Yun.Scripts.Animations
{
    public class ExploseAnimation : MonoBehaviour
    {
        public float explosionForce = 100;
        public float explosionRadius = 5f;
        public float upwardsModifier = 2f;

        public List<GameObject> tankParts = new(); // Assign tank parts in inspector

        private Vector3 _savePos;

        private List<Vector3> _savePositions;
        private Quaternion _saveRot;
        private List<Quaternion> _saveRotations;

        private void Awake()
        {
            /*_savePositions = new List<Vector3>();
            _saveRotations = new List<Quaternion>();
            foreach (var part in tankParts)
            {
                _savePositions.Add(part.transform.position);
                _saveRotations.Add(part.transform.rotation);
            }*/
        }

        private bool _isExploded;

        public void Explode()
        {
            // Debug.Log("Run Explode 1");
            _isExploded = true;
            
            _savePositions = new List<Vector3>();
            _saveRotations = new List<Quaternion>();
            foreach (var part in tankParts)
            {
                _savePositions.Add(part.transform.position);
                _saveRotations.Add(part.transform.rotation);
                part.AddComponent<Rigidbody>();
            }

            // Make parts non-kinematic and apply explosion force
            foreach (var part in tankParts)
            {
                part.GetComponent<Rigidbody>().isKinematic = false;
                // part.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);

                // Add random torque for more realistic spinning
                part.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * explosionForce / 2);

                // Optional: Destroy parts after some time
                // Destroy(part.GetComponent<Rigidbody>().gameObject, Random.Range(3f, 5f));
            }

            // gameObject.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * explosionForce / 2);
            // gameObject.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * explosionForce / 2);
            // Debug.Log("Run Explode 3");
            var o = gameObject;
            _savePos = o.transform.position;
            _saveRot = o.transform.rotation;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>()
                .AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);

            // Optional: Destroy the tank manager after explosion
            // Destroy(gameObject, 5f);
        }

        public void Reborn()
        {
            if(!_isExploded)
                return;
            _isExploded = false;
            for (var i = 0; i < tankParts.Count; i++)
            {
                tankParts[i].transform.position = _savePositions[i];
                tankParts[i].transform.rotation = _saveRotations[i];
                var rb = tankParts[i].GetComponent<Rigidbody>();
                Destroy(rb);
            }

            gameObject.transform.position = _savePos;
            gameObject.transform.rotation = _saveRot;
        }
    }
}