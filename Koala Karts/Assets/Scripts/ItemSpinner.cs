using UnityEngine;
using System.Collections;

public class ItemSpinner : MonoBehaviour
{
    private float rotSpeed = 50.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
    }
}
