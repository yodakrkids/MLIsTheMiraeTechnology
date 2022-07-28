using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMassTest : MonoBehaviour
{
    public Vector3 CenterOfMass;
    public Rigidbody rbody;
    
    private void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rbody.centerOfMass = CenterOfMass;
    }
}
