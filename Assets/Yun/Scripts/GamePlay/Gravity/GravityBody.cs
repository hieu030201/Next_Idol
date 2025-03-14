using UnityEngine;
using Yun.Scripts.GamePlay.Gravity;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    public GravityAttractor attractor;
    Rigidbody rb;

    void Start()
    {
        // attractor = GameObject.FindGameObjectWithTag("Attractor").GetComponent<GravityAttractor>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        // rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        attractor.Attract(transform);

        /*var planet = attractor.transform;
        // Đảm bảo vật thể giữ vị trí trên bề mặt Trái Đất khi quay
        Vector3 directionToPlanet = (transform.localPosition - planet.localPosition).normalized;
        transform.localPosition = planet.localPosition + directionToPlanet * planet.localScale.x;*/
    }
}
