using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Aircraft
{

    // Controlling these values would be better on the UI
    private Rigidbody rb;
    private float lift = 1.0f;
    private float manuverMax = 5, manuverVal = 0.1f;

    // serialized fields:
    // - need to optimize these in the future
    // - manipulating these values on the editor may be confusing
    // - however, manipulating these are easier now than before
    [SerializeField] private float 
        pitchStrength = 3.0f, 
        yawStrength = 8.0f, 
        rollStrength = 1.0f,
        weight = 50000,
        breaking = 10.0f,
        dragMax = 5.0f,
        speedLift = 60.0f,
        cruisingLift = 13.0f,
        speed = 0.25f,
        cruisingSpeed = 200.0f;
    public override void Manuver() // combined torque controls
    {
        // These are unique to a derived aircraft class
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            
            
            rb.AddForce(transform.forward * 1 / speed, ForceMode.VelocityChange);
            rb.AddForce(transform.up * lift / speedLift, ForceMode.VelocityChange);
            if (rb.velocity.magnitude >= 80.0f)
            {
                // speed limit when using 'afterburner'
                rb.velocity = rb.velocity.normalized * 80.0f;
            }

        }

        // air brakes
        if (Input.GetKey(KeyCode.Space))
        {

            rb.AddForce(transform.forward * 1 / breaking, ForceMode.VelocityChange);
            Stall();
            rb.AddForce(transform.up * lift / cruisingLift, ForceMode.VelocityChange);
            while (cruisingSpeed > -10 && cruisingSpeed < 10)
            {
                // decrease forward acceleration when braking
                cruisingSpeed = cruisingSpeed - 1;
            }
        }

        // cruising speed
        else
        {
            rb.AddForce(transform.forward * 1 / cruisingSpeed, ForceMode.VelocityChange);
            while (cruisingSpeed < 10)
            {
                // increase forward acceleration when brakes are released
                // will optimize the numbers in the future
                cruisingSpeed = cruisingSpeed + 1 / 1000000;
                
            }

            Stall();
            rb.AddForce(transform.up * lift / cruisingLift, ForceMode.VelocityChange);
            if (rb.velocity.magnitude >= 80.0f && !Input.GetKey(KeyCode.Space))
            {
                // speed limit
                rb.velocity = rb.velocity.normalized * 80.0f;
            }
            
        }

        // debugging an issue with z-axis angle for Stall()
        //Debug.Log(transform.eulerAngles.z);

    }

    public void Stall()
    {
        // stall motion if speed is less than a value
        // affects the lift of the aircraft at low speed
        // affects the pitch angle of the aircraft
        if (rb.velocity.magnitude < 30.0f)
        {
            // will need to optimize this in the future
            // this changes the pitch angle when stalling
            if ((transform.eulerAngles.z <= 75.0f && transform.eulerAngles.z >= 0.0f) || 
                !(transform.eulerAngles.z <= 315.0f && transform.eulerAngles.z >= 150.0f ) &&
                !(transform.eulerAngles.z >= 30.0f && transform.eulerAngles.z <= 150.0f)) 
            {
                rb.AddTorque(transform.right * manuverVal / 30, ForceMode.VelocityChange);
            }

            // decrease lift when aircraft velocity is below a certain value
            rb.AddForce(transform.up * -1 / 50, ForceMode.VelocityChange);
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