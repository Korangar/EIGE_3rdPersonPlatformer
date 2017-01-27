using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularBuffer <T> :  {

    private T[] buffer;
    int start, end;

    public CircularBuffer(int size)
    {
        buffer = new T[size];
        start = 0;
        end = 0;
    }

    public void Push(T obj)
    {
        end = (end + 1) % buffer.Length;

        if (start == end)
            start = (start + 1) % buffer.Length;
        
        buffer[end] = obj;
    }

    public T Pop()
    {
        if (start == end) return default(T);

        end = (end - 1 + buffer.Length) % buffer.Length;
        return buffer[end];
    }

    public T Peek()
    {
        if (start == end) return default(T);

        int index = (end - 1 + buffer.Length) % buffer.Length;
        return buffer[index];
    }

    public int Count
    {
        get
        {
            return (end - start + buffer.Length) % buffer.Length;
        }
    }
}
