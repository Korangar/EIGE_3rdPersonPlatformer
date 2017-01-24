using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSceneTrigger : MonoBehaviour {

    public TriggerableBehaviour target;

    public void Trigger()
    {
        target.Trigger();
    }
    public void Untrigger()
    {
        target.Untrigger();
    }

}
