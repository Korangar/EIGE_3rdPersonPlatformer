using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTrigger : AbstractTrigger {

    public float weightToTrigger = 1;
    public Vector3 buttonPressedPosition = new Vector3(0, -0.9f, 0);
    
    private float currentWeight = 0;
    private Transform modelTransform;

    void Start()
    {
        modelTransform = GetComponentInChildren<MeshRenderer>().transform;
        Trigger = false;
    }

    void OnTriggerEnter(Collider c)
    {
        Rigidbody rigidbody = c.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            currentWeight += rigidbody.mass;
            Trigger = currentWeight >= weightToTrigger;
        }
    }

    void OnTriggerExit(Collider c)
    {
        Rigidbody rigidbody = c.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            currentWeight -= rigidbody.mass;
            Trigger = currentWeight >= weightToTrigger;
        }
    }
}
