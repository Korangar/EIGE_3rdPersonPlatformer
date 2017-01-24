using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableHingeMotor : TriggerableSceneBehaviour {

    public bool inverseTrigger = false;
    private HingeJoint myHinge;

    // Use this for initialization
    void Start () {
        myHinge = GetComponent<HingeJoint>();
    }

    public override void TriggerSceneBehaviour(bool trigger)
    {
        myHinge.useMotor = inverseTrigger ? !trigger : trigger;
    }
}
