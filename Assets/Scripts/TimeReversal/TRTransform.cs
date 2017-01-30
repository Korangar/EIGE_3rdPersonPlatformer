using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRTransform : TimeReverse {

    void Awake () {
        InitTR();
	}
	
	void FixedUpdate () {
        UpdateTR();
    }

    public override void Load(object obj)
    {
        if (obj != null)
        {
            MyTransform trans = (MyTransform)obj;
            transform.position = trans.position;
            transform.rotation = trans.rotation;
            transform.localScale = trans.localScale;
        }
    }

    public override object Save()
    {
        MyTransform trans = new MyTransform();
        trans.position = transform.position;
        trans.rotation = transform.rotation;
        trans.localScale = transform.localScale;

        return trans;
    }

    struct MyTransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 localScale;
    }

}
