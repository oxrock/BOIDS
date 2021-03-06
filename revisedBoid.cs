﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class revisedBoid : MonoBehaviour
{
    public Vector3 position;
    public int fieldSize = 25;
    public float flightSpeed = .5f;
    public float alignDistance = 10f;
    public float cohesionDistance = 10f;
    public float avoidanceDistance = 10f;
    public Vector3 momentum = new Vector3(0, 0, 0);
    Vector3 alignForce = new Vector3(0, 0, 0);
    Vector3 cohesionForce = new Vector3(0, 0, 0);
    Vector3 seperationForce = new Vector3(0, 0, 0);
    Vector3 BD_force = new Vector3(0, 0, 0);
    Vector3 boundryTarget = new Vector3(0, 0, 0);
    public float momMagnitude;
    public float settingMagnitudes;
    float turnSpeed = 5;
    Vector3 normalized;


    void seperation() {
        int count = 0;
        seperationForce = new Vector3(0, 0, 0);
        foreach (revisedBoid b in singleton.instance.boids)
        {
            if (b != this)
            {
                float distance = Vector3.Distance(transform.position, b.transform.position);
                if (distance < avoidanceDistance)
                {
                    Vector3 force = (transform.position - b.transform.position);
                    seperationForce += force;
                    count++;
                }
            }
        };
        seperationForce = Vector3.Normalize(seperationForce) * settingMagnitudes;
    }

    void align()
    {
        int count = 0;
        alignForce = new Vector3(0, 0, 0);
        foreach (revisedBoid b in singleton.instance.boids)
        {
            if (b != this)
            {
                if (Vector3.Distance(transform.position, b.transform.position) < alignDistance)
                {
                    alignForce += b.momentum;
                    count++;
                }
            }

        }
        if (count > 0)
        {
            alignForce /= count;
        }
        alignForce = Vector3.Normalize(alignForce)*settingMagnitudes;
        
    }

   
    void randomRotate()
    {
        transform.Rotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
    }

    void boundryAvoidance()
    {
        float distance = Vector3.Distance(boundryTarget, transform.position);
        if (distance > fieldSize * .9)
        {
            BD_force = Vector3.Normalize(boundryTarget - transform.position)*5;
        }

    }

    void cohesion()
    {
        cohesionForce = new Vector3(0, 0, 0);
        Vector3 avgPosition = new Vector3();
        int total = 0;
        foreach (revisedBoid b in singleton.instance.boids)
        {
            if (b != this)
            {
                if (Vector3.Distance(transform.position, b.transform.position) < cohesionDistance)
                {
                    avgPosition += b.transform.position;
                }
            }
        }
        if (total > 1)
        {
            avgPosition /= total;
        }
        cohesionForce = Vector3.Normalize(avgPosition - transform.position)*settingMagnitudes;


    }

    


    void Start()
    {
        singleton.instance.boids.Add(this);
        fieldSize = singleton.instance.fieldSize;
        position = new Vector3(Random.Range(-fieldSize*2, fieldSize * 2), Random.Range(-fieldSize, fieldSize), Random.Range(-fieldSize, fieldSize));
        transform.position = position;
        randomRotate();
        momentum = transform.forward;
        alignDistance = singleton.instance.alignDistance;
        cohesionDistance = singleton.instance.cohesionDistance;
        avoidanceDistance = singleton.instance.alignDistance;
        momMagnitude = singleton.instance.momMag;
        settingMagnitudes = singleton.instance.setMag;
    }



    void FixedUpdate()
    {
        Vector3 changes = new Vector3(0,0,0);
        align();
        cohesion();
        boundryAvoidance();
        seperation();
        momentum += seperationForce;
        momentum += alignForce;
        momentum += BD_force;
        momentum += cohesionForce;
        normalized = momentum.normalized;
        momentum = normalized*momMagnitude;
        if (momentum != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(normalized, Vector3.up);
        }
        transform.position += normalized * (flightSpeed * Time.fixedDeltaTime);

    }
}
