using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private Rigidbody rb;
    private bool isCarried = false;
    private float bobbingSpeed = 2f;
    private float bobbingAmount = 0.05f;
    private float baseY;

    public GameObject GameObject => this.gameObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        baseY = transform.position.y;
    }

    void Update()
    {
        if (isCarried)
        {
            // Bobbing effect
            float newY = baseY + Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
            Vector3 newPos = new Vector3(transform.position.x, newY, transform.position.z);
            transform.position = newPos;

            // Slow rotation
            transform.Rotate(Vector3.up, 50f * Time.deltaTime);
        }
    }

    public void Pickup()
    {
        isCarried = true;
        rb.isKinematic = true;
        rb.detectCollisions = false;
    }

    public void Drop()
    {
        isCarried = false;
        rb.isKinematic = false;
        rb.detectCollisions = true;
        baseY = transform.position.y; // reset baseY on drop
    }
}
