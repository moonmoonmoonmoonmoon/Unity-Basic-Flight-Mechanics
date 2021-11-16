using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFlightMovements : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Need to optimize this code. Organize the code better...
        // However, this is a working solution for a rigidbody.
        // It mimics flight behvaiour, but it does not simulate flight acurately.

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
            rb.AddForce(transform.up * 1/3, ForceMode.VelocityChange);

            if (Input.GetKey(KeyCode.E))
            {
                rb.AddTorque(transform.forward * -1/2, ForceMode.VelocityChange);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                rb.AddTorque(transform.forward * 1 / 2, ForceMode.VelocityChange);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddTorque(transform.up * -1 / 2, ForceMode.VelocityChange);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddTorque(transform.up * 1 / 2, ForceMode.VelocityChange);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddTorque(transform.right * -1 / 2, ForceMode.VelocityChange);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddTorque(transform.right * 1 / 2, ForceMode.VelocityChange);
            }


        }

        else
        {
            rb.AddForce(transform.forward * speed * 1/2, ForceMode.VelocityChange);
            rb.AddForce(transform.up * 1 / 3, ForceMode.VelocityChange);

            if (Input.GetKey(KeyCode.E))
            {
                rb.AddTorque(transform.forward * -1/2, ForceMode.VelocityChange);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                rb.AddTorque(transform.forward * 1 / 2, ForceMode.VelocityChange);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddTorque(transform.up * -1 / 2, ForceMode.VelocityChange);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddTorque(transform.up * 1 / 2, ForceMode.VelocityChange);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddTorque(transform.right * -1 / 2, ForceMode.VelocityChange);
            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddTorque(transform.right * 1 / 2, ForceMode.VelocityChange);
            }
        }
    }
}
