using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Aircraft
{

    // Controlling these values would be better on the UI
    private Rigidbody rb;
    private float lift = 1.0f;
    private float manuverMax = 5, manuverVal = 0.1f;
    public Quaternion desiredDir;

    // serialized fields: --> turns out, just needed to make it public...
    // - need to optimize these in the future
    // - manipulating these values on the editor may be confusing
    // - however, manipulating these are easier now than before
    public float 
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

        
    }

    public void Stall()
    {
        // stall motion if speed is less than a value
        // affects the lift of the aircraft at low speed
        // affects the pitch angle of the aircraft
        if (rb.velocity.magnitude < 30.0f)
        {

            // change the nose direction to point to the ground when stalling
            // now utilizing Quaternion
            desiredDir = Quaternion.Euler(90, 0, 0) * Quaternion.Inverse(rb.rotation);
            var torque = new Vector3(desiredDir.x * 3.0f, 0, 0) * desiredDir.w * Time.deltaTime;
            rb.AddTorque(torque, ForceMode.VelocityChange);
            Debug.DrawRay(transform.position, transform.forward * 100, Color.red);

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

    ///
    /// Had to create a CopySign method because Mathf does not seem to have one...
    /// 
    public float CopySign(float m, float n)
    {
        if (!(n < 0))
        {
            return Mathf.Abs(m);
        }

        else
        {
            return -m;
        }
    } // end of CopySign

}