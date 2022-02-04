using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Aircraft
{
    // Will serialize these at a later time
    // Controlling these values would be better on the UI
    private Rigidbody rb;
    private float speed = 1.0f, weight = 50000, lift = 1.0f;
    private float breaking = 10.0f;
    private float dragMax = 0.8f, manuverMax = 5, manuverVal = 0.1f;
    [SerializeField] private float pitchStrength = 3.0f, yawStrength = 8.0f, rollStrength = 1.0f;
    public override void Manuver() // combined torque controls
    {
        // These are unique to an Airliner or Large Aircraft
        Pitch(manuverVal);
        Yaw(manuverVal);
        Roll(manuverVal);
    }

    private void Pitch(float max) // lateral axis control or Elevator
    {
        if (Input.GetKey(KeyCode.S) || Input.GetAxisRaw("Mouse Y") > 0)
        {
            rb.AddTorque(transform.right * -max / pitchStrength, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetAxisRaw("Mouse Y") < 0)
        {
            rb.AddTorque(transform.right * max / pitchStrength, ForceMode.VelocityChange);
        }
    }

    private void Yaw(float max) // perpendicular axis control or Rudder
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(transform.up * -max / yawStrength, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(transform.up * max / yawStrength, ForceMode.VelocityChange);
        }
    }

    private void Roll(float max) // longitudinal axis control or Aileron
    {
        if (Input.GetKey(KeyCode.E) || Input.GetAxisRaw("Mouse X") > 0)
        {
            rb.AddTorque(transform.forward * -max / rollStrength, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetAxisRaw("Mouse X") < 0)
        {
            rb.AddTorque(transform.forward * max / rollStrength, ForceMode.VelocityChange);
        }
    }

    public override void ForwardSpeed()
    {
        // forward acceleration
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(transform.forward * speed * 0.5f, ForceMode.VelocityChange);
            rb.AddForce(transform.up * lift / 100.0f, ForceMode.VelocityChange);
            Stall();

        }

        // breaking
        if (Input.GetKey(KeyCode.F))
        {
            rb.AddForce(transform.forward * breaking, ForceMode.Impulse);
            Stall();
        }

        // cruising
        else
        {
            rb.AddForce(transform.forward * 0.5f, ForceMode.VelocityChange);
            rb.AddForce(transform.up * lift / 14.0f, ForceMode.VelocityChange);
            Stall();

        }

    }

    public void Stall()
    {
        // stall motion if speed is less than a value
        // affects the lift of the aircraft at low speed
        if (rb.velocity.magnitude < 35.0f && !(rb.velocity.magnitude < 1))
        {
            rb.AddForce(transform.up * 0.0f, ForceMode.VelocityChange);
            Debug.Log("WARNING: STALL!!!");
        }

        
    }

    public override void SetValues()
    {
        // set the values to a game object
        // For an Aircraft
        rb = GetComponent<Rigidbody>();
        rb.drag = dragMax;
        rb.angularDrag = manuverMax;
        rb.mass = weight;
    }
}