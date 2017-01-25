using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class HingeMotorSwitch : AbstractTriggerableSwitch{

    public bool inverseTrigger = false;
    private HingeJoint myHinge;

    // Use this for initialization
    void Start () {
        myHinge = GetComponent<HingeJoint>();
    }

    public override void Trigger()
    {
        myHinge.useMotor = !inverseTrigger;
    }

    public override void Untrigger()
    {
        myHinge.useMotor = inverseTrigger;
    }
    
}
