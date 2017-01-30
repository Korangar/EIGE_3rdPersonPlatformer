using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRTransform : TimeReverse<MyTransform> {

    void Awake () {
        InitTR();
	}
	
	void Update () {
        UpdateTR();
    }

    public override void Load(MyTransform trans)
    {
        if (trans == null) return;

        transform.position = trans.position;
        transform.rotation = trans.rotation;
        transform.localScale = trans.localScale;
    }

    public override MyTransform Save()
    {
        MyTransform trans = new MyTransform();
        trans.position = transform.position;
        trans.rotation = transform.rotation;
        trans.localScale = transform.localScale;

        return trans;
    }
}

[Serializable]
public class MyTransform
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;
}