using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRPickUp : TimeReverse<MyEnabled>
{

    void Awake()
    {
        InitTR();
    }

    void Update()
    {
        UpdateTR();
    }

    public override void Load(MyEnabled trans)
    {
        if (trans == null) return;

        transform.position = trans.position;
        transform.rotation = trans.rotation;
        transform.localScale = trans.localScale;
    }

    public override MyEnabled Save()
    {
        MyEnabled trans = new MyEnabled();
        trans.position = transform.position;
        trans.rotation = transform.rotation;
        trans.localScale = transform.localScale;

        return trans;
    }

}

public class MyEnabled
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;
}