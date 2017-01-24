using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour {

    public float weightToTrigger = 1;
    public TriggerableSceneBehaviour[] actions;
    public Vector3 buttonPressedPosition = new Vector3(0, -0.9f, 0);

    private bool isActivated = false;
    private float currentWeight = 0;
    private Transform button;

    private bool Trigger
    {
        set
        {
            foreach (TriggerableSceneBehaviour t in actions)
            {
                t.TriggerSceneBehaviour(value);
            }
            // button animation
            button.localPosition = value ? buttonPressedPosition : Vector3.zero;
        }
    }


    void Start()
    {
        button = GetComponentInChildren<MeshRenderer>().transform;
        Trigger = isActivated;
    }

    void OnTriggerEnter(Collider c)
    {
        Rigidbody rigidbody = c.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            currentWeight += rigidbody.mass;
            UpdatePlateState();
        }
    }

    void OnTriggerExit(Collider c)
    {
        Rigidbody rigidbody = c.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            currentWeight -= rigidbody.mass;
            UpdatePlateState();
        }
    }

    public void UpdatePlateState()
    {
        bool wasActivated = isActivated;
        isActivated = currentWeight >= weightToTrigger;

        if (isActivated != wasActivated)
        {
            // state changed
            Trigger = isActivated;
        }
    }
}
