﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWindmill : MonoBehaviour
{
    public GameObject blades;
    public float rotationsPerMinute = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        blades.transform.Rotate(0, 0, 6.0f * rotationsPerMinute);
    }
}
