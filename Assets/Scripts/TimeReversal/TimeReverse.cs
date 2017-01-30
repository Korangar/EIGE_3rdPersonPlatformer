using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeReverse : MonoBehaviour {

    private CircularBuffer<object> history;
    Rigidbody rigit;

    public void InitTR()
    {
        history = new CircularBuffer<object>(150);
        rigit = GetComponent<Rigidbody>();
    }

    public void UpdateTR()
    {
        if (Input.GetAxis("TimeControl") > 0.9f)
        {
            if (history.Count > 0)
            {
                disableRigit();

                Debug.Log(history.Peek());
                Load(history.Pop());
            }
            else
            {
                enableRigit();
            }
        }
        else
        {
            enableRigit();

            history.Push(Save());
        }
    }

    private void enableRigit()
    {
        if (!rigit) return;

        rigit.isKinematic = false;
        rigit.useGravity = true;
        rigit.detectCollisions = true;
    }

    private void disableRigit()
    {
        if (!rigit) return;

        rigit.isKinematic = true;
        rigit.useGravity = false;
        rigit.detectCollisions = false;
    }

    public void Clear()
    {
        history.Clear();
    }

    public abstract void Load(object obj);
    public abstract object Save();
}
