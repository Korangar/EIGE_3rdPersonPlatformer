using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTrigger : AbstractTrigger {

    public Vector3 defaultPosition = new Vector3(0, -0.15f, 0);
    public Vector3 pressedPosition = new Vector3(0, -0.2f, 0);
    public float weightToTrigger = 1;
    
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
            checkTrigger();
        }
    }

    void OnTriggerExit(Collider c)
    {
        Rigidbody rigidbody = c.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            currentWeight -= rigidbody.mass;
            checkTrigger();
        }
    }

    public void checkTrigger()
    {
        Trigger = currentWeight >= weightToTrigger;
        modelTransform.localPosition = defaultPosition + (Trigger ? pressedPosition : Vector3.zero);
    }
}
