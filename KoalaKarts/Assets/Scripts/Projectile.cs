using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float ProjectileSpeed = 50;
    public GameObject Explosion;

    // Use this for initialization
    void Start()
    {
        transform.Rotate(90, 0, 0, Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * ProjectileSpeed);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "PlayerOneKart" || col.gameObject.name == "PlayerTwoKart")
        {
            KartController kart = col.gameObject.GetComponent<KartController>();
            ContactPoint contact = col.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            Instantiate(Explosion, pos, rot);
            if (!kart.shieldEnabled)
            {
                kart.kartStatus.Hit();
                col.rigidbody.AddExplosionForce(500000.0f, contact.point, 10.0f, 10000.0f);
                col.rigidbody.AddTorque(Vector3.up * 10000000.0f);
            }
        }

        Destroy(gameObject);
    }
}