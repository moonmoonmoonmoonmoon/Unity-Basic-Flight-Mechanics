using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFlightMovements : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 1, weight = 5000;
    private float dragMax = 0.5f, manuverMax = 5, manuverVal = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = dragMax;
        rb.angularDrag = manuverMax;
        rb.mass = weight;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Need to optimize this code. Organize the code better...
        // However, this is a working solution for a rigidbody.
        // It mimics flight behvaiour, but it does not simulate flight acurately.

        manuvers(manuverVal);
        forwardSpeed(speed);    

    }

    private void manuvers(float max) // combined torque controls
    {
        pitch(max);
        yaw(max);
        roll(max);
    }

    private void pitch(float max) // lateral axis control
    {
        if (Input.GetKey(KeyCode.S) || Input.GetAxisRaw("Mouse Y") > 0)
        {
            rb.AddTorque(transform.right * -max / 2, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetAxisRaw("Mouse Y") < 0)
        {
            rb.AddTorque(transform.right * max / 2, ForceMode.VelocityChange);
        }
    }

    private void yaw(float max) // perpendicular axis control
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(transform.up * -max / 8, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(transform.up * max / 8, ForceMode.VelocityChange);
        }
    }

    private void roll(float max) // longitudinal axis control
    {
        if (Input.GetKey(KeyCode.E) || Input.GetAxisRaw("Mouse X") > 0)
        {
            rb.AddTorque(transform.forward * -max / 2, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetAxisRaw("Mouse X") < 0)
        {
            rb.AddTorque(transform.forward * max / 2, ForceMode.VelocityChange);
        }
    }

    private void forwardSpeed(float maxSpeed)
    {
        // forward acceleration
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(transform.forward * maxSpeed, ForceMode.VelocityChange);
            rb.AddForce(transform.up * 0.005f, ForceMode.VelocityChange);

        }

        // breaking
        if (Input.GetKey(KeyCode.F))
        {
            rb.AddForce(transform.forward * 0.05f, ForceMode.VelocityChange);
            rb.AddForce(transform.up * 0, ForceMode.VelocityChange);
        }

        // crusing speed
        else
        {
            rb.AddForce(transform.forward * maxSpeed * 1 / 2, ForceMode.VelocityChange);
            rb.AddForce(transform.up * 1 / 5, ForceMode.VelocityChange);
        }

    }
}
