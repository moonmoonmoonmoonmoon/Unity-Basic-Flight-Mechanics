using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aircraft : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetValues();
    }

    // Update is called once per frame
    void Update()
    {
        Manuver();
        ForwardSpeed();
        SetValues();
    }

    public virtual void Manuver()
    {
        Debug.Log("Base Class for Aircraft: Manuver()");
    } // axial manuvers

    public virtual void ForwardSpeed()
    {
        Debug.Log("Base Class For Aircraft: ForwardSpeed()");
    } // forward speed

    public virtual void SetValues()
    {
        Debug.Log("Base Class For Aircraft: SetValues()");
    } // set the values for rigidbody

}
