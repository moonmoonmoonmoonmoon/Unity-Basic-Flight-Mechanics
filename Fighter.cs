using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Aircraft
{

    // Controlling these values would be better on the UI
    public Rigidbody rb;
    public float manuverMax = 5;
    public Quaternion stallDir;

    // serialized fields: --> turns out, just needed to make it public...
    // - need to optimize these in the future
    // - manipulating these values on the editor may be confusing
    // - however, manipulating these are easier now than before
    // NOTE: Set these on the editor to better tune it for an aircraft
    public float 
        pitchStrength = 1.0f, 
        yawStrength = 1.0f, 
        rollStrength = 1.0f,
        weight = 1.0f,
        breaking = 1.0f,
        dragMax = 1.0f,
        speedLift = 1.0f,
        cruisingLift = 1.0f,
        speed = 1.0f,
        cruisingSpeed = 1.0f;

    public override void Manuver() // combined torque controls
    {
        // These are unique to a derived aircraft class
        Pitch();
        Yaw();
        Roll(rollStrength);
    }

    private void Pitch() // lateral axis control or Elevator
    {
        if (Input.GetKey(KeyCode.S) || Input.GetAxisRaw("Mouse Y") > 0)
        {
            rb.AddTorque(-pitchStrength * Time.deltaTime * rb.transform.right, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetAxisRaw("Mouse Y") < 0)
        {
            rb.AddTorque(pitchStrength * Time.deltaTime * transform.right, ForceMode.VelocityChange);
        }
    }

    private void Yaw() // perpendicular axis control or Rudder
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(Time.deltaTime * -yawStrength * transform.up, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(Time.deltaTime * yawStrength * transform.up, ForceMode.VelocityChange);
        }
    }

    private void Roll(float roll) // longitudinal axis control or Aileron
    {
        if (Input.GetKey(KeyCode.E) || Input.GetAxisRaw("Mouse X") > 0)
        {
            rb.AddTorque(-roll * Time.deltaTime * transform.forward, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetAxisRaw("Mouse X") < 0)
        {
            rb.AddTorque(roll * Time.deltaTime * transform.forward, ForceMode.VelocityChange);
        }
    }

    public override void ForwardSpeed()
    {
        // forward acceleration
        if (Input.GetKey(KeyCode.LeftShift))
        {
            
            
            rb.AddForce(speed * Time.deltaTime * transform.forward, ForceMode.VelocityChange);
            rb.AddForce(speedLift * Time.deltaTime * transform.up, ForceMode.VelocityChange);
            if (rb.velocity.magnitude >= 80.0f)
            {
                // speed limit when using 'afterburner'
                rb.velocity = rb.velocity.normalized * 80.0f;
            }

        }

        // air brakes
        if (Input.GetKey(KeyCode.Space))
        {

            rb.AddForce(breaking * Time.deltaTime * transform.forward, ForceMode.VelocityChange);
            Stall();
            if (rb.velocity.magnitude <= 35)
            { 
                rb.AddTorque((Random.Range(-8.0f, 8.0f)) * Time.deltaTime * transform.forward, ForceMode.VelocityChange);
            }
            
            while (cruisingSpeed > -20 && cruisingSpeed < 20)
            {
                // decrease forward acceleration when braking
                cruisingSpeed -= 1;
            }
        }

        // cruising speed
        else
        {
            rb.AddForce(cruisingSpeed * Time.deltaTime * transform.forward, ForceMode.VelocityChange);
            while (cruisingSpeed < 10)
            {
                // increase forward acceleration when brakes are released
                // will optimize the numbers in the future
                cruisingSpeed += 1 / 1000000;
                
            }

            Stall();
            rb.AddForce(cruisingLift * Time.deltaTime * transform.up, ForceMode.VelocityChange);
            if (rb.velocity.magnitude >= 80.0f && !Input.GetKey(KeyCode.Space))
            {
                // speed limit
                rb.velocity = rb.velocity.normalized * 80.0f;
            }
            
        }

        Debug.Log(cruisingSpeed);
    }

    public void Stall()
    {
        // stall motion if speed is less than a value
        // affects the lift of the aircraft at low speed
        // affects the pitch angle of the aircraft
        if (rb.velocity.magnitude < 38.5f)
        {
            // stallDir is set here when the aircraft stalls
            // stallDir is the direction that an aircraft points to during a stall
            // points the nose of the aircraft down
            stallDir = Quaternion.FromToRotation(-Vector3.down, rb.transform.forward);

            // torque value is set here when the aircraft stalls
            // the torque value is a Vector3 that takes the XYZ values of the stallDir
            // multiplied by the stallDir.w and Time.deltaTime
            // all XYZW values are accounted for to use Quaternion rotations during a stall
            var torque = new Vector3(stallDir.x, stallDir.y, stallDir.z) * stallDir.w * Time.deltaTime;

            // applied the torque value to the pitch rotation of the aircraft during a stall
            rb.AddTorque(torque, ForceMode.VelocityChange);
            Debug.DrawRay(rb.transform.position, transform.forward * 100, Color.red);

            // decrease lift when aircraft velocity is below a certain value
            rb.AddForce(transform.up * -1 / 50, ForceMode.VelocityChange);
            Debug.Log("WARNING: STALL!!! PULL UP!!!");
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