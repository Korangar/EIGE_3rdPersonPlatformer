using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBeamLevitation : MonoBehaviour {
    void OnTriggerEnter(Collider c) {
        if (c.tag == "Player")
        {
            PlayerController pc = c.GetComponent<PlayerController>();
            if (pc)
            {
                pc.inputLock = true;
            }
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb)
            {
                rb.velocity = Vector3.zero;
            }
            ConstantForce f = pc.gameObject.AddComponent<ConstantForce>();
            f.force = new Vector3(0, 35, 0);
        }
    }
}
