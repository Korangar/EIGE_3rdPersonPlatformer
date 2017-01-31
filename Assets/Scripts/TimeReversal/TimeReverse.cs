using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class TimeReverse : MonoBehaviour {
    public abstract void Clear();
}

public abstract class TimeReverse<T> : TimeReverse
{
    private CircularBuffer<T> history;
    private int frameCounter;
    [SerializeField]
    public static int ReverseSpeed = 1;
    Rigidbody rigit;

    public void InitTR()
    {
        history = new CircularBuffer<T>(150);
        rigit = GetComponent<Rigidbody>();
    }

    public void UpdateTR()
    {
        int input = (int)(Input.GetAxis("TimeControl") * 10);

        if (input > 0)
        {
            if (history.Count > 0)
            {
                disable();

                for (int i = 0; i < input; i++)
                {
                    Load(history.Pop());
                }
            }
        }
        else
        {
            enable();

            frameCounter++;
            frameCounter %= ReverseSpeed;

            if (frameCounter == 0)
            history.Push(Save());
        }
    }

    private void enable()
    {
        if (!rigit) return;

        rigit.isKinematic = false;
        //rigit.useGravity = true;
        //rigit.detectCollisions = true;

        foreach (MonoBehaviour c in GetComponents<MonoBehaviour>())
        {
            if (c is TimeReverse<T>)
                continue;

            c.enabled = true;
        }

    }

    private void disable()
    {
        if (!rigit) return;

        rigit.isKinematic = true;
        //rigit.useGravity = false;
        //rigit.detectCollisions = false;

        foreach (MonoBehaviour c in GetComponents<MonoBehaviour>())
        {
            if (c is TimeReverse<T>)
                continue;

            c.enabled = false;
        }

    }

    public override void Clear()
    {
        history.Clear();
    }

    public abstract void Load(T obj);
    public abstract T Save();
}

