using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TRPickUp : TimeReverse<MyEnabled>{
    PickupScript pu;

    void Awake()
    {
        InitTR();
        pu = GetComponent<PickupScript>();
    }

    void Update()
    {
        UpdateTR();
    }

    public override void Load(MyEnabled obj)
    {
        if (obj == null) return;

        

        pu.IsActive = obj.enabled;

    }

    public override MyEnabled Save()
    {
        return new MyEnabled(pu.IsActive);
    }

}

public class MyEnabled
{
    public bool enabled;

    public MyEnabled(bool en)
    {
        enabled = en;
    }
}