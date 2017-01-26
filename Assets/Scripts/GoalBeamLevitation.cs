using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBeamLevitation : MonoBehaviour {
    void OnTriggerEnter(Collider c) {
        if (c.tag == "Player")
        {
            PlayerController pc = c.GetComponent<PlayerController>();
            pc.inputLock = true;
            ConstantForce f = pc.gameObject.AddComponent<ConstantForce>();
            f.force = new Vector3(0, 35, 0);
        }
    }
}
