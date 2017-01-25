using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTrigger : MonoBehaviour {

    public AbstractTriggerableSwitch[] switches;

    private bool isTriggered = false;
    protected bool Trigger
    {
        get
        {
            return isTriggered;
        }
        set
        {
            if (isTriggered != value)
            {
                isTriggered = value;
                foreach(AbstractTriggerableSwitch ats in switches)
                {
                    if (ats)
                    if (value)
                    { ats.Trigger(); }
                    else
                    { ats.Untrigger(); }
                }
            }
        }
    }

}
