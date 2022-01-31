using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airliner : Aircraft
{
    private Rigidbody rb;
    private float speed = 1, weight = 5000, lift = 0.2f;
    private float dragMax = 0.5f, manuverMax = 5, manuverVal = 0.5f;
    public override void Manuver() // combined torque controls
    {
        pitch(manuverVal);
        yaw(manuverVal);
        roll(manuverVal);
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

    public override void ForwardSpeed()
    {
        // forward acceleration
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(transform.forward * speed * 0.5f, ForceMode.VelocityChange);
            rb.AddForce(transform.up * lift/200, ForceMode.VelocityChange);

        }

        // breaking
        if (Input.GetKey(KeyCode.F))
        {
            rb.AddForce(transform.forward * 0.0015f, ForceMode.VelocityChange);
            rb.AddForce(transform.up * 0, ForceMode.VelocityChange);
        }

        // crusing speed
        else
        {
            rb.AddForce(transform.forward * 0.25f, ForceMode.VelocityChange);
            Stall();
        }

    }

    public override void Stall()
    {
        // stall motion if speed is less than a value
        // affects the lift of the aircraft at low speed
        if (rb.velocity.magnitude < 18f)
        {
            rb.AddForce(transform.up * 0.005f);
        }
        else
        {
            rb.AddForce(transform.up * lift, ForceMode.VelocityChange);
        }
    }

    public override void SetValues()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = dragMax;
        rb.angularDrag = manuverMax;
        rb.mass = weight;
    }
}
