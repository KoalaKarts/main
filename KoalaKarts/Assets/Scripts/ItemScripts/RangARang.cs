using UnityEngine;
using System.Collections;

public class RangARang : MonoBehaviour
{
    public float ProjectileSpeed = 50;
    public GameObject Explosion;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.Translate(Vector3.right * ProjectileSpeed);
	}
}
