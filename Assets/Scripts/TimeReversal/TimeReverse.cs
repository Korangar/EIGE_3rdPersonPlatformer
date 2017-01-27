using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeReverse : MonoBehaviour {

    private CircularBuffer<object> history;

    public void InitTR()
    {
        history = new CircularBuffer<object>(1000);
    }

    public void UpdateTR()
    {
        if (Input.GetButton("TimeControl"))
        {
            if (history.Count > 0)
                Load(history.Pop());
        }
        else
        {
            history.Push(Save());
        }
    }

    public abstract void Load(object obj);
    public abstract object Save();
}
