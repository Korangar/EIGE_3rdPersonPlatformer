using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerableBehaviour : MonoBehaviour {
    public abstract void Trigger();
    public abstract void Untrigger();
}
