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
            Transform trans = (Transform)obj;
            transform.position = trans.position;
            transform.rotation = trans.rotation;
            transform.localScale = trans.localScale;
        }
    }

    public override object Save()
    {
        return transform;
    }
}
