using UnityEngine;
using System.Collections;

public class Caltrops : MonoBehaviour
{
    public float lifetime = 10;
    private float timer;

	// Use this for initialization
	void Start ()
    {
        timer = lifetime;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
            timer = lifetime;
        }
	}
}
